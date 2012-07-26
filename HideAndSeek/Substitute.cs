using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek {
    class Substitute {
        List<Session> _ar = new List<Session>();
        int _port;
        Log _log;
        public Adapter Adapter { set; private get; }

        public Substitute(Capture capture,int port,Log log) {
            _port = port;
            _log = log;
            capture.OnCapture += new OnCaptureHandler(capture_OnCapture);
        }

        void capture_OnCapture(RecvPacket recvPacket) {
            lock (this) {

                //対象ポート以外のパケットは処理しない
                if (recvPacket.Port[(int)Sd.Dst] != _port)
                    return;

                //自分あてのパケット以外は処理しない
                bool hit = false;
                foreach (var ip in Adapter.Ip) {
                    if (Util.Ip2Str(recvPacket.Ip[(int)Sd.Dst]) == ip) {
                        hit = true;
                        break;
                    }
                }
                if (!hit)
                    return;


                if (recvPacket.Flg == 0x02) {//SYNパケット到着
                    //新しいセッションの開始
                    _ar.Add(new Session(recvPacket, _log));

                    //SYN/ACKの送信


                } else {//それ以外のパケット
                    //当該セッションに追加
                    for (int i = 0; i < _ar.Count; i++) {
                        if (_ar[i].Append(recvPacket)) {
                            break;
                        }
                    }
                }

                //終了したセッションの清掃
                for (int i = _ar.Count - 1; i >= 0; i--) {
                    if (!_ar[i].Life) {
                        _log.Set(string.Format("ar.RemoteAt(0)", i));
                        _ar.RemoveAt(i);
                    }
                }
            }

        }

        //新しい接続があるかどうか
        public Session Accept(){
            lock (this) {
                foreach (var a in _ar) {
                    if (!a.Accept && a.Life) {
                        a.Accept = true;
                        return a;
                    }
                }
                return null;
            }
        }
        public void Stop() {
        
        }

    }
}

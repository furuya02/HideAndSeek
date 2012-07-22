using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek {
    class Substitute {
        List<Session> ar = new List<Session>();
        int _port;
        Log _log;
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

                //自分あて以外は処理しない
                if (Util.Ip2Str(recvPacket.Ip[(int)Sd.Dst]) != "192.168.0.11")
                    return;

                if (recvPacket.Flg == 0x02) {//SYNパケット到着
                    //新しいセッションの開始
                    ar.Add(new Session(recvPacket, _log));

                    //SYN/ACKの送信


                } else {//それ以外のパケット
                    //当該セッションに追加
                    for (int i = 0; i < ar.Count; i++) {
                        if (ar[i].Append(recvPacket)) {
                            break;
                        }
                    }
                }

                //終了したセッションの清掃
                for (int i = ar.Count - 1; i >= 0; i--) {
                    if (!ar[i].Life) {
                        _log.Set(string.Format("ar.RemoteAt(0)", i));
                        ar.RemoveAt(i);
                    }
                }
            }

        }

        //新しい接続があるかどうか
        public Session Accept(){
            lock (this) {
                foreach (var a in ar) {
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

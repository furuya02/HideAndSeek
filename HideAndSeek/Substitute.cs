using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek {
    class Substitute {
        List<Session> _ar = new List<Session>();
        int _port;
        Log _log;
        bool _arpReplay;
        List<string> _arpReplyList;
        public Adapter Adapter { set; private get; }

        byte[] _myMac = new byte[]{0,1,2,3,4,5,6};

        public Substitute(Capture capture,int port,bool arpReply,List<string> arpReplyList,Log log) {
            _port = port;
            _arpReplay = arpReply;
            _arpReplyList = arpReplyList;
            _log = log;
            capture.OnCapture += new OnCaptureHandler(capture_OnCapture);
        }

        void capture_OnCapture(RecvPacket recvPacket) {
            lock (this) {
                //************************************************************
                //ARPパケット処理
                //************************************************************
                if (_arpReplay) {//ARP応答処理
                    if (recvPacket.Type == PType.ARP) {
                        if (recvPacket.arpHeader.code == 0x0100) {//要求
                            var ip = Util.Ip2Str(recvPacket.arpHeader.dstIp);
                            foreach (var a in _arpReplyList) {
                                if (ip == a) {
                                    var arpReplyPacket = new ArpReplyPacket(_log, recvPacket, _myMac);

                                    WinPcap.Send(arpReplyPacket.Buf);
                                    _log.Set(string.Format("ARP Replay {0}", Util.Ip2Str(recvPacket.arpHeader.dstIp)));
                                    return;
                                }
                            }
                        }
                    }
                }
                //************************************************************
                //TCPパケット処理
                //************************************************************
                //宛先MAC確認(Etherヘッダ)
                var mac = Util.Mac2Str(recvPacket.Mac[(int)Sd.Dst]);
                if (mac.ToUpper() != Util.Mac2Str(_myMac)) {
                    if (mac.ToUpper() != Adapter.Mac.ToUpper()) {
                        return;
                    }
                }
                //プロトコル確認(IPヘッダ)
                if (recvPacket.ipHeader.protocol != 0x06) {
                    return;
                }
                //ポート番号確認(TCPヘッダ)
                if (recvPacket.Port[(int)Sd.Dst] != _port)
                    return;


                if (recvPacket.Flg == 0x02) {//SYNパケット到着
                    //新しいセッションの開始
                    _ar.Add(new Session(recvPacket, _log));

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
    }
}

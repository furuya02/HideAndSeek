using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace HideAndSeek {
    class Session {
        Log _log;
        RecvPacket _recvPacket;
        public bool Life { get; private set; }
        public bool Accept { get; set; }
        
        short ident;
        Random rnd = new Random();
        uint squence;
        uint ack = 0;
        bool CloseFlg = false;

        byte[] buffer = new byte[0];

        public Session(RecvPacket recvPacket,Log log) {
            lock (this) {

                
                Life = true;
                Accept = false;
                _recvPacket = recvPacket;
                _log = log;

                ident = (short)rnd.Next(500);//生成

                //squenceは生成する
                squence = (uint)rnd.Next(99999);
                //相手のsquence+dataLen+(1)でackを初期化
                ack = recvPacket.Squence + recvPacket.Len;
                if (recvPacket.Len == 0)
                    ack++;
                
                Log(string.Format("Create ({0})", Util.Flg2Str(recvPacket.Flg)));
                
                Send(0x12, new byte[0]);// SYN/ACK
            }

        }
        public bool Append(RecvPacket recvPacket) {
            if (!Life)
                return false;

            if (CloseFlg)//もう受け付けない
                return false;

            lock (this) {

                Log(string.Format("Recv ({0}) srcPort={1} len={2} {3}", Util.Flg2Str(recvPacket.Flg), recvPacket.Port[(int)Sd.Src],recvPacket.Len, (recvPacket.Len == 0) ? "" : Encoding.ASCII.GetString(recvPacket.Data)));

                //関係ないパケットは処理しない
                for (int i = 0; i < 2; i++) {
                    if (Util.Mac2Str(recvPacket.Mac[i]) != Util.Mac2Str(_recvPacket.Mac[i])) {
                        return false;
                    }
                    if (Util.Ip2Str(recvPacket.Ip[i]) != Util.Ip2Str(_recvPacket.Ip[i])) {
                        return false;
                    }
                    if (recvPacket.Port[i] != _recvPacket.Port[i]) {
                        return false;
                    }
                }


                if (Util.FIN(recvPacket.Flg)) {
                    
                    //Log(string.Format("Recv ({0})", Util.Flg2Str(recvPacket.Flg)));
                    Send(0x10,new byte[0]);//ACK

                    Life = false;
                } else {

                    //Log(string.Format("Recv ({0}) len={1} {2}", Util.Flg2Str(recvPacket.Flg), recvPacket.Len, (recvPacket.Len==0)?"":Encoding.ASCII.GetString(recvPacket.Data)));

                    if (recvPacket.Len != 0) {

                        if (recvPacket.Len != 0) {
                            var s = Encoding.ASCII.GetString(recvPacket.Data);
                        }

                        //既に受信されているバッファを退避
                        var tmp = new byte[buffer.Length];
                        Buffer.BlockCopy(buffer, 0, tmp, 0, buffer.Length);

                        buffer = new byte[tmp.Length + recvPacket.Len];//新しいサイズを確保
                        Buffer.BlockCopy(tmp, 0, buffer, 0, tmp.Length);//既存のデータを戻す
                        Buffer.BlockCopy(recvPacket.Data, 0, buffer, tmp.Length, recvPacket.Len);//新しいデータを追加
                    }

                    
                    //相手のackでsquenceを初期化する
                    squence = recvPacket.Ack;
                    //相手のsquence+dataLen+1でackを初期化
                    ack = recvPacket.Squence + recvPacket.Len;
                    if (recvPacket.Len == 0)
                        ack++;
                }
                return true;
            }
        }

        //public void Close() {
        //    CloseFlg = true;
        //    //Send(0x11, new byte[0]);//FIN/ACK
        //    //Life = false;
        //}
        
        public int Read(byte[] buf, int offset,int max) {
            if (buffer.Length == 0)
                return 0;
            lock (this) {
                int l = buffer.Length;
                if(l>max){
                    l = max;
                }
                Buffer.BlockCopy(buffer,0,buf,offset,l);
                
                //送りきれなかったバイト数
                var remains = buffer.Length-l;
                if(remains>0){
                    var tmp = new byte [remains]; 
                    Buffer.BlockCopy(buffer,l,tmp,0,remains);
                    buffer = tmp;
                }else{
                    buffer = new byte[0];
                }
                return l;
            }
        }
        public int Write(byte[] buf, int offset, int len,bool fin) {
            int max = offset + len;
            int p = offset;
            while ((max-p)>0) {
                int l = max - p;
                if (l > 700) {
                    l = 700;
                }
                var data = new byte[l];
                Buffer.BlockCopy(buf, p, data, 0, l);
                byte flg = 0x18;// PSH/ACK
                if (0 >= max - (p + l) && fin) {
                    //if(fin)
                    flg = 0x19; // FIN/PSH/ACK
                }

                Send(flg,data);// PSH/ACK
                p += l;
            }
            return len;
        }
        void Log(string msg) {
            _log.Set(string.Format("[{0}] {1}", _recvPacket.Port[(int)Sd.Src], msg));
        }
        void Send(byte flg,byte[] data) {
            var sendPacket = new SendPacket(_log, _recvPacket, ident++, squence, ack, flg,data);
            WinPcap.Send(sendPacket.Buf);
            Log(string.Format("Send {0} Len={1} {2}", Util.Flg2Str(flg), data.Length, Encoding.ASCII.GetString(data)));

            squence += (uint)data.Length;
        }

    }
}

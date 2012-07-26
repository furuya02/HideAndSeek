using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net.Sockets;

namespace HideAndSeek {
    class Session:Stream{
        Log _log;
        RecvPacket _recvPacket;
        short _ident;
        Random _rnd = new Random();
        uint _squence;
        uint _ack = 0;
        byte[] _buffer = new byte[0];
        
        public bool Life { get; private set; }
        public bool Accept { get; set; }
        

        public Session(RecvPacket recvPacket,Log log) {
            lock (this) {


                Life = true;
                Accept = false;
                _recvPacket = recvPacket;
                _log = log;

                _ident = (short)_rnd.Next(500);//生成

                //squenceは生成する
                _squence = (uint)_rnd.Next(99999);
                //相手のsquence+dataLen+(1)でackを初期化
                _ack = recvPacket.Squence + recvPacket.Len;
                if (recvPacket.Len == 0)
                    _ack++;
                
                Log(string.Format("Create ({0})", Util.Flg2Str(recvPacket.Flg)));
                
                Send(0x12, new byte[0]);// SYN/ACK
            }

        }
        public bool Append(RecvPacket recvPacket) {
            if (!Life)
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
                        var tmp = new byte[_buffer.Length];
                        Buffer.BlockCopy(_buffer, 0, tmp, 0, _buffer.Length);

                        _buffer = new byte[tmp.Length + recvPacket.Len];//新しいサイズを確保
                        Buffer.BlockCopy(tmp, 0, _buffer, 0, tmp.Length);//既存のデータを戻す
                        Buffer.BlockCopy(recvPacket.Data, 0, _buffer, tmp.Length, recvPacket.Len);//新しいデータを追加
                    }

                    
                    //相手のackでsquenceを初期化する
                    _squence = recvPacket.Ack;
                    //相手のsquence+dataLen+1でackを初期化
                    _ack = recvPacket.Squence + recvPacket.Len;
                    if (recvPacket.Len == 0)
                        _ack++;
                }
                return true;
            }
        }
        
        override public int Read(byte[] buf, int offset,int max) {
            if (_buffer.Length == 0)
                return 0;
            lock (this) {
                int l = _buffer.Length;
                if(l>max){
                    l = max;
                }
                Buffer.BlockCopy(_buffer,0,buf,offset,l);
                
                //送りきれなかったバイト数
                var remains = _buffer.Length-l;
                if(remains>0){
                    var tmp = new byte [remains]; 
                    Buffer.BlockCopy(_buffer,l,tmp,0,remains);
                    _buffer = tmp;
                }else{
                    _buffer = new byte[0];
                }
                return l;
            }
        }
        override public void Write(byte[] buf, int offset, int len) {
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
                if (0 >= max - (p + l)) {
                    flg = 0x19; // FIN/PSH/ACK
                }

                Send(flg,data);// PSH/ACK
                p += l;
            }
        }
        void Log(string msg) {
            _log.Set(string.Format("[{0}] {1}", _recvPacket.Port[(int)Sd.Src], msg));
        }

        void Send(byte flg,byte[] data) {
            var sendPacket = new SendPacket(_log, _recvPacket, _ident++, _squence, _ack, flg,data);
            WinPcap.Send(sendPacket.Buf);
            Log(string.Format("Send {0} Len={1} {2}", Util.Flg2Str(flg), data.Length, Encoding.ASCII.GetString(data)));

            _squence += (uint)data.Length;
        }

        public override void SetLength(long value) {
            throw new NotImplementedException();
        }
        public override long Seek(long offset, SeekOrigin origin) {
            throw new NotImplementedException();
        }
        public override void Flush() {
            throw new NotImplementedException();
        }
        public override bool CanWrite {
            get { throw new NotImplementedException(); }
        }
        public override bool CanRead {
            get { throw new NotImplementedException(); }
        }
        public override bool CanSeek {
            get { throw new NotImplementedException(); }
        }
        public override long Length {
            get { throw new NotImplementedException(); }
        }
        public override long Position {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }


    }
}

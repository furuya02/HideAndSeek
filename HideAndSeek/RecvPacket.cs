using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace HideAndSeek {
    enum Sd {
        Src=0,Dst=1
    }
    
    class RecvPacket {
        public byte [] Buf{ get; private set; }
        public ushort Len { get; private set; }

        public EtherHeader etherHeader{ get; private set; }
        public List<byte[]> Mac{ get; private set; }
        public PType Type { get; private set; }

        public IpHeader ipHeader{ get; private set; }
        public Protocol Protocol{ get; private set; }
        public List<byte[]> Ip { get; private set; }

        public TcpHeader tcpHeader{ get; private set; }
        public List<ushort> Port { get; private set; }
        public uint Squence { get; private set; }
        public uint Ack { get; private set; }
        public byte Flg { get; private set; }
        public byte [] Data { get; private set; }


        public RecvPacket(byte [] b){
            
            etherHeader = new EtherHeader();
            ipHeader = new IpHeader();
            tcpHeader = new TcpHeader();

            Mac = new List<byte[]> { null,null};
            Ip = new List<byte[]> { null, null };
            Port = new List<ushort> { 0, 0 };
            for (int i = 0; i < 2; i++) {
                Mac[i] = new byte[] { 0, 0, 0, 0, 0, 0 };
                Ip[i] = new byte[] { 0, 0, 0, 0 };
            }

            this.Buf = new byte[b.Length];
            Buffer.BlockCopy(b,0,Buf,0,b.Length);
            
            unsafe {
                int offSet = 0;
                fixed (byte* p = b) {
                    // Etherヘッダ処理
                    int etherHeaderSize = Marshal.SizeOf(etherHeader);
                    if (offSet + etherHeaderSize > b.Length) //  受信バイト数超過
                        return;
                    etherHeader = (EtherHeader)Marshal.PtrToStructure((IntPtr)(p + offSet), typeof(EtherHeader));
                    offSet += etherHeaderSize;

                    // Ether dst mac
                    Buffer.BlockCopy(etherHeader.dstMac, 0, Mac[(int)Sd.Dst], 0, 6);
                    Buffer.BlockCopy(etherHeader.srcMac, 0, Mac[(int)Sd.Src], 0, 6);
                    
                    // Ether type
                    Type = (PType)Util.htons(etherHeader.type);
                    
                    if (Type == PType.IP) {//IP
            
                        // IPヘッダ処理
                        int ipHeaderSize = Marshal.SizeOf(ipHeader);

                        if (offSet + ipHeaderSize > b.Length) //  受信バイト数超過
                            return;
                        ipHeader = (IpHeader)Marshal.PtrToStructure((IntPtr)(p + offSet), typeof(IpHeader));
                        var ipHeaderLen  = (ipHeader.verLen & 0x0F) * 4;
                        offSet += ipHeaderLen;

                        Len = Util.htons(ipHeader.totalLen);
                        Len -= (ushort)ipHeaderLen;
                        
                        // プロトコル
                        Protocol = (Protocol)ipHeader.protocol;
                        
                        // 送信元　IPアドレス
                        Buffer.BlockCopy(ipHeader.srcIp, 0, Ip[(int)Sd.Src], 0, 4);
                        // 送信先　IPアドレス
                        Buffer.BlockCopy(ipHeader.dstIp, 0, Ip[(int)Sd.Dst], 0, 4);
                        
                        if (ipHeader.protocol == 6) { // TCP

                            // TCPヘッダ処理
                            int tcpHeaderSize = Marshal.SizeOf(tcpHeader);

                            if (offSet + tcpHeaderSize > b.Length) //  受信バイト数超過
                                return;
                            tcpHeader = (TcpHeader)Marshal.PtrToStructure((IntPtr)(p + offSet), typeof(TcpHeader));

                            var l = tcpHeader.offset >> 4;
                            var hedderLen = l*4;
                            Len -= (ushort)hedderLen;

                            if (Len != 0) {
                                Data = new byte[Len];
                                for (int i = 0; i < Len; i++) {
                                    Data[i] = (byte)*(p + offSet + hedderLen + i);
                                }
                            }

                            // 送信元ポート
                            Port[(int)Sd.Src] = Util.htons(tcpHeader.srcPort);
                            // 送信先ポート
                            Port[(int)Sd.Dst] = Util.htons(tcpHeader.dstPort);

                            Squence = Util.htons(tcpHeader.squence);
                            Ack = Util.htons(tcpHeader.ack);

                            Flg = tcpHeader.flg;
                        }
                    } else if (Type == PType.ARP) { //ARP
                        return; // 未対応
                    } else {
                        return; // 未対応
                    }
                }
            }
        }
    }
}

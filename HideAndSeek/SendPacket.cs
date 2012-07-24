using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;

namespace HideAndSeek {
    class SendPacket {
        public byte[] Buf{get;private set;}

        Log _log;

        public SendPacket(Log log,RecvPacket recvPacket,short ident,uint squence,uint ack,byte flg,byte [] data) {
            _log = log;

            var dataLen = data.Length;
            var etherHeaderLen = 14;
            var ipHeaderLen = 20;
            var tcpHeaderLen = 20;

            Buf = new byte[etherHeaderLen + ipHeaderLen + tcpHeaderLen + dataLen];

            EtherHeader etherHeader = new EtherHeader();
            etherHeader.dstMac = new byte[] { 0, 0, 0, 0, 0, 0 };
            etherHeader.srcMac = new byte[] { 0, 0, 0, 0, 0, 0 };
            Buffer.BlockCopy(recvPacket.etherHeader.srcMac,0,etherHeader.dstMac,0,6);
            Buffer.BlockCopy(recvPacket.etherHeader.dstMac,0,etherHeader.srcMac,0,6);
            etherHeader.type = recvPacket.etherHeader.type;

            IpHeader ipHeader = new IpHeader(); 
            ipHeader.srcIp = new byte[] { 0, 0, 0, 0 };
            ipHeader.dstIp = new byte[] { 0, 0, 0, 0 };
            Buffer.BlockCopy(recvPacket.ipHeader.srcIp, 0, ipHeader.dstIp, 0, 4);
            Buffer.BlockCopy(recvPacket.ipHeader.dstIp, 0, ipHeader.srcIp, 0, 4);
            ipHeader.verLen = recvPacket.ipHeader.verLen;
            ipHeader.flags = recvPacket.ipHeader.flags;
            ipHeader.ident = ident;
            ipHeader.protocol = recvPacket.ipHeader.protocol;
            ipHeader.ttl = recvPacket.ipHeader.ttl;
            ipHeader.totalLen = Util.htons((ushort)(ipHeaderLen + tcpHeaderLen + dataLen));//IP/TCPヘッダのみ

            TcpHeader tcpHeader = new TcpHeader();
            tcpHeader.urgent = recvPacket.tcpHeader.urgent;
            tcpHeader.window = 0x0020;
            tcpHeader.srcPort = recvPacket.tcpHeader.dstPort;
            tcpHeader.dstPort = recvPacket.tcpHeader.srcPort;
            tcpHeader.ack =  Util.htons(ack);
            tcpHeader.squence = Util.htons(squence);
            tcpHeader.offset = 0x50;// TcpHeaderLen=20 byte
            tcpHeader.flg = flg;

            //チェックサム計算方法
            //http://ja.wikipedia.org/wiki/Transmission_Control_Protocol


            //IPチェックサム
            var b = new byte[ipHeaderLen];
            Buffer.BlockCopy(GetBytes(ipHeader), 0, b, 0, ipHeaderLen);
            ipHeader.checkSum = (short)Util.htons(CreateChecksum(b, 0, ipHeaderLen));


            //TCPチェックサム
            //擬似ヘッダ + TcpHeader + TCPデータ
            b = new byte[12 + tcpHeaderLen + dataLen];//擬似ヘッダ+TcpHeader
            Buffer.BlockCopy(GetBytes(tcpHeader), 0, b, 12, tcpHeaderLen);
            Buffer.BlockCopy(data, 0, b, 32, dataLen);
            
            //擬似ヘッダ編集
            Buffer.BlockCopy(recvPacket.ipHeader.srcIp, 0, b, 0, 4);
            Buffer.BlockCopy(recvPacket.ipHeader.dstIp, 0, b, 4, 4);
            b[9]=6;//TCP
            int size = tcpHeaderLen + dataLen;
            b[10] = (byte)((size & 0xFF00) >> 8);
            b[11] = (byte)(size & 0x00FF);
            
            tcpHeader.checkSum = (short)Util.htons(CreateChecksum(b, 0, b.Length));
            
            unsafe {
                fixed (byte* p = Buf) {
                    var offSet=0;
                    Marshal.StructureToPtr(etherHeader, new IntPtr(p + offSet), true);
                    offSet += 14;
                    Marshal.StructureToPtr(ipHeader, new IntPtr(p + offSet), true);
                    offSet += 20;
                    Marshal.StructureToPtr(tcpHeader, new IntPtr(p + offSet), true);
                }
                if(dataLen>0)
                    Buffer.BlockCopy(data, 0,Buf, 54, dataLen);
            }
        }

        byte[] GetBytes(IpHeader p) {
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IpHeader)));
            Marshal.StructureToPtr(p, ptr, false);
            byte[] ih = new byte[Marshal.SizeOf(typeof(IpHeader))];
            Marshal.Copy(ptr, ih, 0, Marshal.SizeOf(typeof(IpHeader)));
            Marshal.FreeHGlobal(ptr);
            return ih;
        }
        byte[] GetBytes(TcpHeader p) {
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TcpHeader)));
            Marshal.StructureToPtr(p, ptr, false);
            byte[] ih = new byte[Marshal.SizeOf(typeof(TcpHeader))];
            Marshal.Copy(ptr, ih, 0, Marshal.SizeOf(typeof(TcpHeader)));
            Marshal.FreeHGlobal(ptr);
            return ih;
        }
        ushort CreateChecksum(byte[] buf, int start, int len) {
            ushort d;
            long sum = 0;
            for (int i = start; i < (len + start); i += 2) {
                if (i + 1 >= len + start) {
                    d = (ushort)(((buf[i] << 8) & 0xFF00));
                } else {
                    d = (ushort)(((buf[i] << 8) & 0xFF00) + (buf[i + 1] & 0xFF));
                }
                sum += (long)d;
            }
            while ((sum >> 16) != 0) {
                sum = (sum & 0xFFFF) + (sum >> 16);
            }
            sum = ~sum;
            return (ushort)sum;
        }
    }
}

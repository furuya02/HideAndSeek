using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace HideAndSeek {
    class ArpReplyPacket {
        public byte[] Buf { get; private set; }
        public ArpReplyPacket(Log log,RecvPacket recvPacket,byte [] mac) {

            EtherHeader etherHeader = new EtherHeader();
            etherHeader.dstMac = new byte[] { 0, 0, 0, 0, 0, 0 };
            etherHeader.srcMac = new byte[] { 0, 0, 0, 0, 0, 0 };

            ArpHeader arpHeader = new ArpHeader();
            arpHeader.srcIp = new byte[] { 0, 0, 0, 0 };
            arpHeader.dstIp = new byte[] { 0, 0, 0, 0 };
            arpHeader.srcMac = new byte[] { 0, 0, 0, 0, 0, 0 };
            arpHeader.dstMac = new byte[] { 0, 0, 0, 0, 0, 0 };
            
            var arpHeaderLen = Marshal.SizeOf(arpHeader);
            var etherHeaderLen = Marshal.SizeOf(etherHeader);

            Buf = new byte[etherHeaderLen + arpHeaderLen];

            Buffer.BlockCopy(recvPacket.etherHeader.srcMac, 0, etherHeader.dstMac, 0, 6);
            Buffer.BlockCopy(mac, 0, etherHeader.srcMac, 0, 6);
            etherHeader.type = recvPacket.etherHeader.type;

            
            Buffer.BlockCopy(recvPacket.arpHeader.srcMac, 0, arpHeader.dstMac, 0, 6);
            Buffer.BlockCopy(recvPacket.arpHeader.srcIp, 0, arpHeader.dstIp, 0, 4);
            Buffer.BlockCopy(mac, 0, arpHeader.srcMac, 0, 6);
            Buffer.BlockCopy(recvPacket.arpHeader.dstIp, 0, arpHeader.srcIp, 0, 4);

            arpHeader.code = 0x0200;//応答
            arpHeader.hwLen = recvPacket.arpHeader.hwLen;
            arpHeader.hwType = recvPacket.arpHeader.hwType;
            arpHeader.protoLen = recvPacket.arpHeader.protoLen;
            arpHeader.type = recvPacket.arpHeader.type;

            
            unsafe {
                fixed (byte* p = Buf) {
                    var offSet=0;
                    Marshal.StructureToPtr(etherHeader, new IntPtr(p + offSet), true);
                    offSet += 14;
                    Marshal.StructureToPtr(arpHeader, new IntPtr(p + offSet), true);
                }
            }

        
        }
    }
}

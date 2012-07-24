using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek {
    class Util {
        /**************************************************************/
        // Utility
        /**************************************************************/

        static public ushort htons(ushort i) {
            return (ushort)((i << 8) + (i >> 8));
        }
        static public uint htons(uint i) {
            return (uint)((i >> 24) + ((i & 0x00FF0000) >> 8) + ((i & 0x0000FF00) << 8) + (i << 24));
        }

        static public string Mac2Str(byte[] mac) {
            return string.Format("{0:x2}:{1:x2}:{2:x2}:{3:x2}:{4:x2}:{5:x2}"
                 , mac[0], mac[1], mac[2]
                 , mac[3], mac[4], mac[5]);
        }
        static public string Ip2Str(byte[] ip) {
            return string.Format("{0}.{1}.{2}.{3}"
                 , ip[0], ip[1], ip[2], ip[3]);
        }
        static public string Flg2Str(byte flg) {
            var sb = new StringBuilder();
            if (FIN(flg))
                sb.Append("/FIN");
            if (SYN(flg))
                sb.Append("/SYN");
            if (RST(flg))
                sb.Append("/RST");
            if (RSH(flg))
                sb.Append("/RSH");
            if (ACK(flg))
                sb.Append("/ACK");
            if (URG(flg))
                sb.Append("/URG");
            if (flg != 0) {
                sb.Append(string.Format("[0x{0:X}]", flg));
            }
            return sb.ToString();
        }
        static public bool FIN(byte flg) {
            if ((flg & 0x01) != 0) {
                return true;
            }
            return false;
        }
        static public bool SYN(byte flg) {
            if ((flg & 0x02) != 0) {
                return true;
            }
            return false;
        }
        static public bool RST(byte flg) {
            if ((flg & 0x04) != 0) {
                return true;
            }
            return false;
        }
        static public bool RSH(byte flg) {
            if ((flg & 0x08) != 0) {
                return true;
            }
            return false;
        }
        static public bool ACK(byte flg) {
            if ((flg & 0x010) != 0) {
                return true;
            }
            return false;
        }
        static public bool URG(byte flg) {
            if ((flg & 0x20) != 0) {
                return true;
            }
            return false;
        }
    }
}

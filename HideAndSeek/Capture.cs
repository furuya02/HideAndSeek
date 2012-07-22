using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;

namespace HideAndSeek {

    delegate void OnCaptureHandler(RecvPacket p);

    class Capture:IDisposable {
        public event OnCaptureHandler OnCapture = null;
       
        public Capture() {

            // イベントハンドラの追加
            WinPcap.OnRecv += new WinPcap.OnRecvHandler(OnRecv);
        }
        public void Dispose() {
            WinPcap.OnRecv -= OnRecv;
            WinPcap.Stop();
        }

        public List<Adapter> GetAdapterList() {
            var ar = new List<Adapter>();

            foreach (var a in WinPcap.GetDeviceList()) {
                ar.Add(new Adapter(a.description,a.name));
                //TODO IPアドレスを取得
            }
            return ar;
        }
        public void Start(string name,bool promiscuous) {
            WinPcap.Start(name,promiscuous);
        }
        
        //ushort htons(ushort i) {
        //    return (ushort)((i << 8) + (i >> 8));
        //}
        //uint htons(uint i) {
        //    return (uint)((i >> 24) + ((i & 0x00FF0000) >> 8) + ((i & 0x0000FF00) << 8) + (i << 24));
        //}
        
        //パケット受信時のイベントハンドラ
        void OnRecv(IntPtr pkt_hdr, IntPtr pkt_data) {
            
            WinPcap.pcap_pkthdr hdr = (WinPcap.pcap_pkthdr)Marshal.PtrToStructure(pkt_hdr, typeof(WinPcap.pcap_pkthdr));
            byte[] data = new byte[hdr.caplen];
            Marshal.Copy(pkt_data, data, 0, (int)hdr.caplen);
            var recvPacket = new RecvPacket(data);
            if (OnCapture != null) {
                OnCapture(recvPacket);
            }
        }
    }
}

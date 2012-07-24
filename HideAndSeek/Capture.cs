using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.Management;//参照設定 System.Management

namespace HideAndSeek {

    delegate void OnCaptureHandler(RecvPacket p);

    class Capture:IDisposable {
        public event OnCaptureHandler OnCapture = null;
        public Adapter Adapter { set; private get; }
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

            var ms = new ManagementObjectSearcher("select * from Win32_NetworkAdapterConfiguration");
            foreach (var m in ms.Get()) {
                foreach (var a in ar) {
                    if (a.Name.IndexOf((string)(m["SettingID"])) != -1) {
                        if (m["IPAddress"] != null) {
                            foreach (var s in (string[])(m["IPAddress"])) {
                                a.SetIp(s);
                           }
                        }
                        if (m["MACAddress"] != null) {
                            a.SetMac((string)(m["MACAddress"]));
                        }
                        break;
                    }
                }
            }
            return ar;
        }

        public void Start(string name,bool promiscuous) {
            WinPcap.Start(name,promiscuous);
        }
        
           
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

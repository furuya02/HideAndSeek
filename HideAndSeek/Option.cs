using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek {

    public class Option {
        public RunMode RunMode { get; set; }
        public bool AckReply { get; set; }
        public List<string> ArpReplyList { get; set; }
        public List<string> AdapterList { get; set; }
        public int AdapterIndex{ get; set; }
        public Option() {
            RunMode = RunMode.Bind;
            AckReply = false;
            
            AdapterIndex = 0;
            AdapterList = new List<string>();

            Capture capture = new Capture();
            var ar = capture.GetAdapterList();
            foreach (var a in ar) {
                var sb = new StringBuilder();
                sb.Append(a.Description);
                sb.Append(" ");
                foreach (var s in a.Ip) {
                    sb.Append(s);
                    sb.Append(" , ");
                }
                AdapterList.Add(sb.ToString());
            }

            ArpReplyList = new List<string>();
            //100..200
            for (int i = 0; i <= 100; i++) {
                ArpReplyList.Add(string.Format("192.168.64.{0}", i + 101));
            }
            try {
                var ip = ar[AdapterIndex].Ip[0];
                var tmp = ip.Split('.');
                if (tmp.Length == 4) {
                    ArpReplyList.Clear();
                    for (int i = 0; i <= 100; i++) {
                        ArpReplyList.Add(string.Format("{0}.{1}.{2}.{3}", tmp[0], tmp[1], tmp[2], i + 101));
                    }
                }
            } catch {

            }
        }
    }
}

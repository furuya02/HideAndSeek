using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HideAndSeek {

    public class Option {
        public RunMode RunMode { get; set; }
        public bool AckReply { get; set; }
        public List<string> ArpReplyList { get; set; }
        public int AdapterIndex{ get; set; }

        string _fileName;
        public Option() {
            RunMode = RunMode.None;
            AckReply = false;
            AdapterIndex = 0;
            ArpReplyList = new List<string>();


            _fileName = string.Format("{0}\\Option.dat", Directory.GetCurrentDirectory());

            Read();
        }
        
        public void Save() {
            var lines = new List<string>();
            lines.Add(string.Format("RunMode={0}", (int)RunMode));
            lines.Add(string.Format("AckReply={0}", AckReply));
            lines.Add(string.Format("AdapterIndex={0}", AdapterIndex));
            var sb = new StringBuilder();
            foreach (var ip in ArpReplyList) {
                sb.Append(ip);
                sb.Append(",");
            }
            lines.Add(string.Format("ArpReplyList={0}", sb.ToString()));

            File.WriteAllLines(_fileName, lines);
        }
        void Read() {
            if (!File.Exists(_fileName))
                return;
            var lines = File.ReadAllLines(_fileName);
            foreach (var l in lines) {
                var tmp = l.Split(new char[]{'='}, StringSplitOptions.RemoveEmptyEntries);
                if (tmp.Length == 2) {
                    switch (tmp[0]) {
                        case "RunMode":
                            RunMode = (RunMode)(Int32.Parse(tmp[1]));
                            break;
                        case "AckReply":
                            AckReply = Boolean.Parse(tmp[1]);
                            break;
                        case "AdapterIndex":
                            AdapterIndex = Int32.Parse(tmp[1]);
                            break;
                        case "ArpReplyList":
                            var t = tmp[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            ArpReplyList = new List<string>();
                            foreach (var ip in t) {
                                ArpReplyList.Add(ip);
                            }
                            break;
                    }
                }
            }

        
        }
    }
}

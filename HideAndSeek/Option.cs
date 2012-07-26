using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek {

    public class Option {
        public RunMode RunMode { get; set; }
        public bool AckReply { get; set; }
        public List<string> ArpReplyList { get; set; }
        public Option() {
            RunMode = RunMode.Bind;
            AckReply = false;
            ArpReplyList = new List<string>();
            //100..200
            for (int i = 0; i <= 100; i++) {
                ArpReplyList.Add(string.Format("192.168.64.{0}",i+100));
            }
        }
    }
}

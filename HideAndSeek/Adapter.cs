using System;
using System.Collections.Generic;
using System.Text;

namespace HideAndSeek {
    class Adapter {
        public string Description { get; private set; }
        public string Name { get; private set; }
        public List<string> Ip { get; private set; }
        public string Mac { get; private set; }
        public Adapter(string description, string name) {
            Description = description;
            Name = name;
            Ip = new List<string>();
        }
        public void SetIp(string ip) {
            Ip.Add(ip);
        }
        public void SetMac(string mac) {
            Mac = mac;
        }
    }
}

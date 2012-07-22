using System;
using System.Collections.Generic;
using System.Text;

namespace HideAndSeek {
    class Adapter {
        public string Description { get; private set; }
        public string Name { get; private set; }
        public Adapter(string description,string name) {
            Description = description;
            Name = name;
        }
    }
}

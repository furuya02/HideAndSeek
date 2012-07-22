using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HideAndSeek {
    class Log {
        ListBox listBox;
        public Log(ListBox listBox) {
            this.listBox = listBox;
        }

        public void Set(string msg) {
            if (listBox.InvokeRequired) {// 別スレッドから呼び出された場合
                listBox.BeginInvoke(new MethodInvoker(() => Set(msg)));
            } else {
                listBox.Items.Add(msg);
                listBox.TopIndex = listBox.Items.Count - listBox.Height / listBox.ItemHeight;
            }

        }
        public void Clear() {
            listBox.Items.Clear();
        }
    }
}

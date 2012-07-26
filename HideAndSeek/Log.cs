using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HideAndSeek {
    class Log {
        ListBox _listBox;
        public Log(ListBox listBox) {
            this._listBox = listBox;
        }

        public void Set(string msg) {
            if (_listBox.InvokeRequired) {// 別スレッドから呼び出された場合
                _listBox.BeginInvoke(new MethodInvoker(() => Set(msg)));
            } else {
                _listBox.Items.Add(msg);
                _listBox.TopIndex = _listBox.Items.Count - _listBox.Height / _listBox.ItemHeight;
            }

        }
        public void Clear() {
            _listBox.Items.Clear();
        }
    }
}

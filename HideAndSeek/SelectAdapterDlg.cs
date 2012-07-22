using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HideAndSeek {
    public partial class Form2 : Form {
        public ListBox ListBox {
            get { return listBox1; }
        }
        public bool Promiscuous {
            get { return checkBox1.Checked; }
        }
        public Form2() {
            InitializeComponent();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                DialogResult = DialogResult.OK;
            }
        }

    }
}

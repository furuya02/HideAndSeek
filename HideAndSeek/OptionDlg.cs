using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HideAndSeek {
    public partial class OptionDlg : Form {
        
        public Option Option{get;private set;}
        
        public OptionDlg(Option option) {
            InitializeComponent();

            Option = option;

            radioButtonBind.Checked = option.RunMode == RunMode.Bind;
            radioButtonPcap.Checked = option.RunMode == RunMode.Pcap;

            checkBoxAckReply.Checked = option.AckReply;
            var sb = new StringBuilder();
            foreach(var ip in option.IpList){
                sb.Append(ip);
                sb.Append("\n");
            }
            textBox1.Text = sb.ToString();

            initDisplay();
        }

        private void buttonOk_Click(object sender, EventArgs e) {
            if (radioButtonBind.Checked)
                Option.RunMode = RunMode.Bind;
            else
                Option.RunMode = RunMode.Pcap;

            Option.AckReply = checkBoxAckReply.Checked;
            Option.IpList = new List<string>();

            var lines = textBox1.Text.Split('\n');
            foreach (var l in lines) {
                Option.IpList.Add(l);
            }

        }

        private void checkBoxAckReply_CheckedChanged(object sender, EventArgs e) {
            initDisplay();
        }

        void initDisplay() {
            checkBoxAckReply.Enabled = radioButtonPcap.Checked;
            textBox1.Enabled = (radioButtonPcap.Checked && checkBoxAckReply.Checked);
        }

    }
}

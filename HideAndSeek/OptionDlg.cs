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
            foreach(var ip in option.ArpReplyList){
                sb.Append(ip);
                sb.Append("\n");
            }
            textBox1.Text = sb.ToString();


            Capture capture = new Capture();
            var ar = capture.GetAdapterList();
            foreach (var a in ar) {
                sb = new StringBuilder();
                sb.Append(a.Description);
                sb.Append(" ");
                foreach (var s in a.Ip) {
                    sb.Append(s);
                    sb.Append(" , ");
                }
                listBoxAdapter.Items.Add(sb.ToString());
            }
            if (listBoxAdapter.Items.Count > 0) {
                listBoxAdapter.SelectedIndex = 0;
                if (listBoxAdapter.Items.Count > option.AdapterIndex)
                    listBoxAdapter.SelectedIndex = option.AdapterIndex;
            } else {
                //アダプタが列挙できていないときは、Pcapモードは使用できない
                radioButtonPcap.Enabled = false;
            }
            initDisplay();
        }

        private void buttonOk_Click(object sender, EventArgs e) {
            if (radioButtonBind.Checked)
                Option.RunMode = RunMode.Bind;
            else
                Option.RunMode = RunMode.Pcap;

            Option.AckReply = checkBoxAckReply.Checked;
            Option.ArpReplyList = new List<string>();

            var ipList = textBox1.Text.Split(new char[]{'\n','\r'},StringSplitOptions.RemoveEmptyEntries);
            foreach (var l in ipList) {
                //IPv4のオクテットを確認する（無効な指定は排除する）
                var tmp = l.Split('.');
                if(tmp.Length==4)
                    Option.ArpReplyList.Add(l);
            }
            Option.AdapterIndex = listBoxAdapter.SelectedIndex;

        }

        private void checkBoxAckReply_CheckedChanged(object sender, EventArgs e) {
            initDisplay();
        }

        void initDisplay() {
            listBoxAdapter.Enabled = radioButtonPcap.Checked;
            checkBoxAckReply.Enabled = radioButtonPcap.Checked;
            textBox1.Enabled = (radioButtonPcap.Checked && checkBoxAckReply.Checked);

        }

    }
}

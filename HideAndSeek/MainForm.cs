using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace HideAndSeek {
    public partial class MainForm : Form {

        WebServer webServer = null;
        CaptureView captureView = null;
        Log log = null;
        int port = 80;

        public MainForm() {
            InitializeComponent();

            log = new Log(listBoxLog);
            captureView = new CaptureView(listViewCapture);
            
            InitializeWebServer(Mode.Bind);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            webServer.Dispose();
        }
        
        //クリア
        private void MainMenuClear_Click(object sender, EventArgs e) {
            captureView.Clear();
            log.Clear();
        }
        //終了
        private void MainMenuExit_Click(object sender, EventArgs e) {
            Close();
        }


        private void MainMenuModeBind_Click(object sender, EventArgs e) {
            InitializeWebServer(Mode.Bind);
        }

        private void MainMenuModePcap_Click(object sender, EventArgs e) {
            InitializeWebServer(Mode.Pcap);
        }

        void InitializeWebServer(Mode mode) {
            if(webServer!=null)
                webServer.Dispose();
            webServer = new WebServer(port, log, captureView,mode);
            MainMenuModeBind.Checked = (mode == Mode.Bind);
            MainMenuModePcap.Checked = (mode == Mode.Pcap);
        }
    }
}

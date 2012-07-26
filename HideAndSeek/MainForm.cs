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

        Option _option;
        WebServer _webServer = null;
        CaptureView _captureView = null;
        Log _log = null;
        int _port = 80;

        public MainForm() {
            InitializeComponent();

            _option = new Option();
            _log = new Log(listBoxLog);
            _captureView = new CaptureView(listViewCapture);
            
            InitializeWebServer();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            _webServer.Dispose();
        }
        
        //クリア
        private void MainMenuClear_Click(object sender, EventArgs e) {
            _captureView.Clear();
            _log.Clear();
        }
        //終了
        private void MainMenuExit_Click(object sender, EventArgs e) {
            Close();
        }

        void InitializeWebServer() {
            if(_webServer!=null)
                _webServer.Dispose();
            _webServer = new WebServer(_port, _log, _captureView,_option);
        }

        private void MainMenuOption_Click(object sender, EventArgs e) {
            var dlg = new OptionDlg(_option);
            if (DialogResult.OK == dlg.ShowDialog()) {
                _option = dlg.Option;
                InitializeWebServer();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;

namespace HideAndSeek {
    public enum RunMode {
        Bind = 0,
        Pcap = 1,
    }
    class WebServer:IDisposable {

        Log _log;
        Thread _t = null;
        bool _life = true;
        int _port;
        string _documentRoot = Directory.GetCurrentDirectory();
        Substitute _substitute = null;
        Capture _capture = null;
        RunMode _runMode;
        CaptureView _captureView;

        public WebServer(int port, Log log, CaptureView captureView,Option option) {
            _log = log;
            _port = port;
            _runMode = option.RunMode;
            _captureView = captureView;

            _documentRoot = Path.GetFullPath(Directory.GetCurrentDirectory() + "\\..\\..\\..\\www");

            _log.Clear();
            log.Set(string.Format("Mode={0}",_runMode));

            _t = new Thread(Loop) { IsBackground = true };
            if (_runMode == RunMode.Bind) {
                _captureView.Enable= false;
            } else {
                _capture = new Capture();
                _substitute = new Substitute(_capture, _port,option.AckReply,option.ArpReplyList, _log);

                _capture.OnCapture += new OnCaptureHandler(_capture_OnCapture);
                _captureView.Enable = true;

                var ar = _capture.GetAdapterList();
                _captureView.Adapter = null;
                int index = option.AdapterIndex;
                bool promiscuous = false;
                _capture.Start(ar[index].Name, promiscuous);
                _captureView.Adapter = ar[index];
                _substitute.Adapter = ar[index];

            }
            _t.Start();
        }

        void _capture_OnCapture(RecvPacket p) {
            _captureView.Set(p);
        }
        public void Dispose() {
            if (_t != null) {//起動されている場合
                _life = false;//スイッチを切るとLoop内の無限ループからbreakする
                if (_t.IsAlive) {
                    Thread.Sleep(100);
                }
            }
            _t = null;

            if (_runMode == RunMode.Pcap) {
                _capture.OnCapture -= _capture_OnCapture;
                _capture.Dispose();
                _capture = null;
                _substitute = null;
            }
        }
        void Loop() {
            TcpListener listener = null;
            if (_runMode == RunMode.Bind) {
                listener = new TcpListener(_port);
                listener.Start();//待ち受け開始
            }

            while (_life) {

                Stream ns = null;
                TcpClient tcp = null;
                if (_runMode == RunMode.Bind) {
                    if (!listener.Pending()) {//接続が無い場合は、処理なし
                        Thread.Sleep(100);
                        continue;
                    }
                    tcp = listener.AcceptTcpClient();
                    ns = tcp.GetStream();
                } else {
                    ns = _substitute.Accept();
                    if (ns == null) {
                        Thread.Sleep(100);
                        continue;
                    }
                }

                _log.Set(string.Format("ns!=null"));
                try {

                    
                    var buf = new byte[2048];//リクエストは2048バイト程度で収まるだろうとキメうちする
                    var size = 0;
                    int count = 0;
                    while (size <= 0) {
                        size = ns.Read(buf, 0, buf.Length);

                        _log.Set(string.Format("count={0} size={1}",count,size));

                        if (size == 0) {
                            Thread.Sleep(100);
                            count++;
                        }
                        if (count > 3) {//タイムアウト処理
                            _log.Set(string.Format("goto end count={0}", count));
                            ns.Write(new byte[0], 0, 0);//FIN/ACK
                            goto end;
                        }
                    }
                    string request = Encoding.ASCII.GetString(buf, 0, size);

                    Job(request, ns);
                    

                } catch { }
            end:
                ns.Close();
                
                if(tcp!=null)
                    tcp.Close();
            }
            if (listener != null) {
                listener.Stop();
            }
        }
        void Job(string request, Stream ns) {
            //リクエストからファイル名を抽出する
            var lines = request.Split('\n');
            if (lines.Length <= 0) {
                return;
            }
            var s = lines[0].Split(' ');
            if (s.Length != 3) {
                return;
            }
            var fileName = s[1];

            //リクエストが/の場合index,htmlに修正する
            if (fileName == "/") {
                fileName = "/index.html";
            }

            //ドキュメントルートからフルパスを取得
            var path = string.Format("{0}{1}", _documentRoot, fileName);

            _log.Set(path);

            if (File.Exists(path)) {

                var info = new FileInfo(path);

                var sb = new StringBuilder();
                sb.Append("HTTP/1.1 200 OK\r\n");
                sb.Append(string.Format("Content-Length: {0}\r\n", info.Length));
                switch (Path.GetExtension(path).ToLower()) {
                    case ".html":
                        sb.Append("Content-Type: text/html\r\n");
                        break;
                    case ".jpg":
                        sb.Append("Content-Type: image/jpg\r\n");
                        break;
                }

                sb.Append("Connection: close\r\n");
                sb.Append("\r\n");
                var header = Encoding.ASCII.GetBytes(sb.ToString());
                var file = File.ReadAllBytes(path);
                
                var buf = new byte[ header.Length + info.Length ];
                Buffer.BlockCopy(header, 0, buf, 0, header.Length);
                Buffer.BlockCopy(file, 0, buf, header.Length, file.Length);

                ns.Write(buf, 0, buf.Length);

            } else {
                var sb = new StringBuilder();
                sb.Append("HTTP/1.1 404 Not Found\r\n");
                sb.Append("\r\n");
                var buf = Encoding.ASCII.GetBytes(sb.ToString());
                ns.Write(buf, 0, buf.Length);
            }
        }
    }
}

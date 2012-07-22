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
        //Capture capture = null;
        CaptureView captureView = null;
        Log log = null;

        //Substitute substitute = null;
        int port = 80;

        public MainForm() {
            InitializeComponent();

            log = new Log(listBoxLog);
            captureView = new CaptureView(listViewCapture);
            webServer = new WebServer(port, log,Mode.Bind);
          
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            webServer.Dispose();
        }
        private void MainMenuOnlyTcp_Click(object sender, EventArgs e) {
            MainMenuOnlyTcp.Checked = captureView.ToggleTcpOnly();
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
            webServer.Dispose();
            webServer = new WebServer(port, log, Mode.Bind);
            MainMenuModeBind.Checked = true;
            MainMenuModePcap.Checked = false;
        }

        private void MainMenuModePcap_Click(object sender, EventArgs e) {
            webServer.Dispose();
            webServer = new WebServer(port, log, Mode.Pcap);
            MainMenuModeBind.Checked = false;
            MainMenuModePcap.Checked = true;
        }

        
        //リストビューの選択
        /*
         * private void listView1_SelectedIndexChanged(object sender, EventArgs e) {
            			if(listView1.SelectedIndices.Count<=0)
				return;
			treeView.BeginUpdate();

			// ツリービューのクリア
			treeView.Nodes.Clear();

			byte [] data = (byte [] )packetList[listView1.SelectedIndices[0]];
			int len = data.Length;
			unsafe{
				TreeNode node;
				int offSet=0;
				fixed(byte *p = data){

					node = treeView.Nodes.Add("Etherヘッダ");
					// Etherヘッダ処理
					EtherHeader etherHeader = new EtherHeader();
					int etherHeaderSize = Marshal.SizeOf(etherHeader);

					if(offSet+etherHeaderSize>len) //  受信バイト数超過
						goto end;
					etherHeader = (EtherHeader)Marshal.PtrToStructure((IntPtr)(p+offSet),typeof(EtherHeader));
					offSet+=etherHeaderSize;
						
					// Ether dst mac
					node.Nodes.Add(string.Format("送信先MACアドレス {0:x2}-{1:x2}-{2:x2}-{3:x2}-{4:x2}-{5:x2}"
						,etherHeader.dstMac[0],etherHeader.dstMac[1],etherHeader.dstMac[2]
						,etherHeader.dstMac[3],etherHeader.dstMac[4],etherHeader.dstMac[5]));
					// Ether src mac
					node.Nodes.Add(string.Format("送信元MACアドレス {0:x2}-{1:x2}-{2:x2}-{3:x2}-{4:x2}-{5:x2}"
						,etherHeader.srcMac[0],etherHeader.srcMac[1],etherHeader.srcMac[2]
						,etherHeader.srcMac[3],etherHeader.srcMac[4],etherHeader.srcMac[5]));
					// Ether type
					ushort type = htons(etherHeader.type);
					node.Nodes.Add(string.Format("イーサネットタイプ 0x{0:X4}",type));
					node.Expand();

					if(type==0x0800){//IP
						node = treeView.Nodes.Add("IPヘッダ");

						// IPヘッダ処理
						IpHeader ipHeader = new IpHeader();
						int ipHeaderSize = Marshal.SizeOf(ipHeader);

						if(offSet+ipHeaderSize>len) //  受信バイト数超過
							goto end;
						ipHeader = (IpHeader)Marshal.PtrToStructure((IntPtr)(p+offSet),typeof(IpHeader));
							
						offSet += (ipHeader.verLen & 0x0F)*4;
						// プロトコルバージョン
						node.Nodes.Add(string.Format("プロトコルバージョン {0}",ipHeader.verLen>>4));
						// ヘッダ長
						node.Nodes.Add(string.Format("ヘッダ長 0x{0:X2} ({1}オクテット)",ipHeader.verLen & 0x0F,(ipHeader.verLen & 0x0F)*4));
						// サービスタイプ
						node.Nodes.Add(string.Format("サービスタイプ 0x{0:X2} ({1})",ipHeader.tos,ipHeader.tos));
						// 全データ長
                        node.Nodes.Add(string.Format("全データ長 0x{0:X4} ({1}オクテット)", htons((ushort)ipHeader.totalLen), htons((ushort)ipHeader.totalLen)));
						// 識別子
                        node.Nodes.Add(string.Format("識別子 0x{0:X4}", htons((ushort)ipHeader.ident)));
						// フラグメント
                        node.Nodes.Add(string.Format("フラグメント 0x{0:X4}", htons((ushort)ipHeader.flags)));
						// TTL
						node.Nodes.Add(string.Format("TTL(生存時間) 0x{0:X2} ({1})",ipHeader.ttl,ipHeader.ttl));
						// プロトコル
						node.Nodes.Add(string.Format("プロトコル {0} ({1})",ipHeader.protocol,Enum.GetName(typeof(Protocol),ipHeader.protocol)));
						// チェックサム
                        node.Nodes.Add(string.Format("チェックサム 0x{0:X4}", htons((ushort)ipHeader.checkSum)));
						// 送信元　IPアドレス
						node.Nodes.Add(string.Format("送信元IPアドレス {0}.{1}.{2}.{3}"
							,ipHeader.srcIp[0],ipHeader.srcIp[1],ipHeader.srcIp[2],ipHeader.srcIp[3]));
						// 送信先　IPアドレス
						node.Nodes.Add(string.Format("送信先IPアドレス {0}.{1}.{2}.{3}"
							,ipHeader.dstIp[0],ipHeader.dstIp[1],ipHeader.dstIp[2],ipHeader.dstIp[3]));

						node.Expand();
					
						if(ipHeader.protocol==6){ // TCP

							node = treeView.Nodes.Add("TCPヘッダ");
							// TCPヘッダ処理
							TcpHeader tcpHeader = new TcpHeader();
							int tcpHeaderSize = Marshal.SizeOf(tcpHeader);

							if(offSet+tcpHeaderSize>len) //  受信バイト数超過
								goto end;
							tcpHeader = (TcpHeader)Marshal.PtrToStructure((IntPtr)(p+offSet),typeof(TcpHeader));
							// 送信元ポート
							node.Nodes.Add(string.Format("接続元ポート番号 {0}",htons(tcpHeader.srcPort)));
							// 送信先ポート
							node.Nodes.Add(string.Format("接続先ポート番号 {0}",htons(tcpHeader.dstPort)));
							// シーケンス番号
							node.Nodes.Add(string.Format("シーケンス番号 0x{0:X8} ({0})",htons((uint)tcpHeader.squence)));
							// ACK番号
							node.Nodes.Add(string.Format("ACK番号 0x{0:X8} ({0})",htons((uint)tcpHeader.ack)));
							// ヘッダ長
							node.Nodes.Add(string.Format("ヘッダ長 0x{0:X2} ({1}オクテット)",tcpHeader.offset>>4,(tcpHeader.offset>>4)*4));
							// フラグ
							node.Nodes.Add(string.Format("フラグ 0x{0:X4}",tcpHeader.flg));
							// ウインドウサイズ
							node.Nodes.Add(string.Format("ウインドウサイズ 0x{0:X4} ({0})",htons((ushort)tcpHeader.window)));
							// チェックサム
							node.Nodes.Add(string.Format("チェックサム 0x{0:X4}",htons((ushort)tcpHeader.checkSum)));
							// 緊急ポインタ
							node.Nodes.Add(string.Format("緊急ポインタ 0x{0:X4} ({0})",htons((ushort)tcpHeader.urgent)));
						
							node.Expand();
						}else if(ipHeader.protocol==17){ //UDP
							node = treeView.Nodes.Add("UDPヘッダ");
							// UDPヘッダ処理
							UdpHeader udpHeader = new UdpHeader();
							int udpHeaderSize = Marshal.SizeOf(udpHeader);

							if(offSet+udpHeaderSize>len) //  受信バイト数超過
								goto end;
							udpHeader = (UdpHeader)Marshal.PtrToStructure((IntPtr)(p+offSet),typeof(UdpHeader));
							// 送信元ポート
							node.Nodes.Add(string.Format("送信元ポート番号 {0}",htons(udpHeader.srcPort)));
							// 送信先ポート
							node.Nodes.Add(string.Format("送信先ポート番号 {0}",htons(udpHeader.dstPort)));
							// データ長
							node.Nodes.Add(string.Format("データ長 0x{0:X4} ({0}オクテット)",htons((ushort)udpHeader.udpLen)));
							// チェックサム
							node.Nodes.Add(string.Format("チェックサム 0x{0:X4}",htons((ushort)udpHeader.checkSum)));

							node.Expand();
						}
					}else if(type==0x0806){ //ARP
						node = treeView.Nodes.Add("ARPヘッダ");
						
						// ARPッダ処理
						ArpHeader arpHeader = new ArpHeader();
						int arpHeaderSize = Marshal.SizeOf(arpHeader);

						if(offSet+arpHeaderSize>len) //  受信バイト数超過
							goto end;
						arpHeader = (ArpHeader)Marshal.PtrToStructure((IntPtr)(p+offSet),typeof(ArpHeader));

						// ハードウエアタイプ
						node.Nodes.Add(string.Format("ハードウエアタイプ 0x{0:X4}",htons((ushort)arpHeader.hwType)));
						// プロトコルタイプ
						node.Nodes.Add(string.Format("プロトコルタイプ 0x{0:X4}",htons((ushort)arpHeader.type)));
						// ハードウエアアドレス長
						node.Nodes.Add(string.Format("ハードウエアアドレス長 {0}オクテット",arpHeader.hwLen));
						// プロトコルアドレス長
						node.Nodes.Add(string.Format("プロトコルアドレス長 {0}オクテット",arpHeader.protoLen));
						// オペレーションコード
						node.Nodes.Add(string.Format("オペレーションコード 0x{0} ()",htons((ushort)arpHeader.code)));
						// 送信元MACアドレス
						node.Nodes.Add(string.Format("送信元MACアドレス {0:x2}-{1:x2}-{2:x2}-{3:x2}-{4:x2}-{5:x2}"
							,arpHeader.srcMac[0],arpHeader.srcMac[1],arpHeader.srcMac[2]
							,arpHeader.srcMac[3],arpHeader.srcMac[4],arpHeader.srcMac[5]));
						// 送信元　IPアドレス
						node.Nodes.Add(string.Format("送信元IPアドレス {0}.{1}.{2}.{3}"
							,arpHeader.srcIp[0],arpHeader.srcIp[1],arpHeader.srcIp[2],arpHeader.srcIp[3]));
						// 送信先MACアドレス
						node.Nodes.Add(string.Format("送信先MACアドレス {0:x2}-{1:x2}-{2:x2}-{3:x2}-{4:x2}-{5:x2}"
							,arpHeader.dstMac[0],arpHeader.dstMac[1],arpHeader.dstMac[2]
							,arpHeader.dstMac[3],arpHeader.dstMac[4],arpHeader.dstMac[5]));
						// 送信先　IPアドレス
						node.Nodes.Add(string.Format("送信先IPアドレス {0}.{1}.{2}.{3}"
							,arpHeader.dstIp[0],arpHeader.dstIp[1],arpHeader.dstIp[2],arpHeader.dstIp[3]));

						node.Expand();
					}else{
						goto end; // 未対応
					}
				}
			}
			end:
				treeView.EndUpdate();
		}
         * */
        //public ushort htons(ushort i) {
        //    return (ushort)((i << 8) + (i >> 8));
        //}
        //public uint htons(uint i) {
        //    return (uint)((i >> 24) + ((i & 0x00FF0000) >> 8) + ((i & 0x0000FF00) << 8) + (i << 24));
        //}

    }
}

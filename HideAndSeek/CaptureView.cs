using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace HideAndSeek {
    class CaptureView {
        ListView listView;
        
        bool tcpOnly = true;

        public CaptureView(ListView listView) {
            tcpOnly = true;
            this.listView = listView;

            listView.OwnerDraw = true;
            listView.DrawSubItem+=new DrawListViewSubItemEventHandler(listView_DrawSubItem);
            listView.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(listView_DrawColumnHeader);

        }

        void listView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e) {
            e.DrawDefault = true; 
        }

        void listView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e) {
            // 選択されている場合
            if (e.Item.Selected) {
                e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
            } else {
                // 奇数行だけ色を変える
                if (e.Item.Tag.ToString()=="192.168.0.12")
                    e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds);
            }

            // テキストを描画
            e.DrawText();
        }

        /*void listView_DrawItem(object sender, DrawListViewItemEventArgs e) {
            // 選択されている場合
            if (e.Item.Selected) {
                e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
            } else {
                // 奇数行だけ色を変える
                if (e.ItemIndex % 2 > 0)
                    e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds);
            }

            // テキストを描画
            e.DrawText();
        }
        */
        public bool ToggleTcpOnly() {
            tcpOnly = tcpOnly ? false : true;
            return tcpOnly;
        }
        public void Set(RecvPacket p) {
            if (tcpOnly && p.Protocol != Protocol.TCP){
                return;//表示は、TCPのみ
            }
            //DEBUG
            if ((Util.Ip2Str(p.Ip[(int)Sd.Src]) == "192.168.0.12" && Util.Ip2Str(p.Ip[(int)Sd.Dst]) == "192.168.0.11")
             || (Util.Ip2Str(p.Ip[(int)Sd.Dst]) == "192.168.0.12" && Util.Ip2Str(p.Ip[(int)Sd.Src]) == "192.168.0.11")) {

                 var item = listView.Items.Add(Util.Mac2Str(p.Mac[(int)Sd.Dst]));
                 item.SubItems.Add(Util.Mac2Str(p.Mac[(int)Sd.Src]));
                item.SubItems.Add(p.Type.ToString());
                item.SubItems.Add(p.Protocol.ToString());
                item.SubItems.Add(Util.Ip2Str(p.Ip[(int)Sd.Src]));
                item.SubItems.Add(Util.Ip2Str(p.Ip[(int)Sd.Dst]));
                item.SubItems.Add(p.Port[(int)Sd.Src].ToString());
                item.SubItems.Add(p.Port[(int)Sd.Dst].ToString());
                item.SubItems.Add(p.Len.ToString());
                item.SubItems.Add(p.Squence.ToString());
                item.SubItems.Add(p.Ack.ToString());
                item.SubItems.Add(Util.Flg2Str(p.Flg));
                item.Tag = Util.Ip2Str(p.Ip[(int)Sd.Src]);


                //自動スクロール
                var rect = listView.ClientRectangle;//ListViewの高さ取得
                var bounds = listView.Items[0].Bounds;//１行の高さ取得
                var row = rect.Height / bounds.Height;//表示されている行数取得
                var top = listView.Items.Count - row;//１行目のItemIndex
                if (listView.TopItem.Index == top) {
                    //最下行が表示されているので、自動スクロールする
                    listView.EnsureVisible(listView.Items.Count - 1);
                }
            }        
        }
        public void Clear() {
            listView.Items.Clear();
        }
    }
}

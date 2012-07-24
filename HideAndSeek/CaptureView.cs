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

        public CaptureView(ListView listView) {
            this.listView = listView;

            listView.OwnerDraw = true;
            listView.DrawSubItem+=new DrawListViewSubItemEventHandler(listView_DrawSubItem);
            listView.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(listView_DrawColumnHeader);

        }
        public bool Enable { 
            set{
                listView.Enabled = value;
            }
        }
        public Adapter Adapter {set;private get;}

        void listView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e) {
            e.DrawDefault = true; 
        }

        void listView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e) {
            // 選択されている場合
            if (e.Item.Selected) {
                e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
            } else {
                // 自デバイス宛のパケットだけ色を変える
                if (Adapter != null) {
                    if(e.Item.Tag.ToString().ToUpper() == Adapter.Mac.ToUpper()){
                        e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds);
                    }
                }
            }

            // テキストを描画
            e.DrawText();
        }
        public void Set(RecvPacket p) {

            if (listView.InvokeRequired) {// 別スレッドから呼び出された場合
                listView.BeginInvoke(new MethodInvoker(() => Set(p)));
            } else {
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
                item.Tag = Util.Mac2Str(p.Mac[(int)Sd.Dst]);

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

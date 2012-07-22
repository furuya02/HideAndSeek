namespace HideAndSeek {
    partial class MainForm {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.MainMenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuClear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.MainMenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuView = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuOnlyTcp = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuMode = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuModeBind = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuModePcap = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.listViewCapture = new System.Windows.Forms.ListView();
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader20 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainMenuFile,
            this.MainMenuView,
            this.MainMenuMode});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1031, 26);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // MainMenuFile
            // 
            this.MainMenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainMenuClear,
            this.toolStripMenuItem2,
            this.MainMenuExit});
            this.MainMenuFile.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.MainMenuFile.Name = "MainMenuFile";
            this.MainMenuFile.Size = new System.Drawing.Size(85, 22);
            this.MainMenuFile.Text = "ファイル(&F)";
            // 
            // MainMenuClear
            // 
            this.MainMenuClear.Name = "MainMenuClear";
            this.MainMenuClear.Size = new System.Drawing.Size(130, 22);
            this.MainMenuClear.Text = "クリア(&C)";
            this.MainMenuClear.Click += new System.EventHandler(this.MainMenuClear_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(127, 6);
            // 
            // MainMenuExit
            // 
            this.MainMenuExit.Name = "MainMenuExit";
            this.MainMenuExit.Size = new System.Drawing.Size(130, 22);
            this.MainMenuExit.Text = "終了(&X)";
            this.MainMenuExit.Click += new System.EventHandler(this.MainMenuExit_Click);
            // 
            // MainMenuView
            // 
            this.MainMenuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainMenuOnlyTcp});
            this.MainMenuView.Name = "MainMenuView";
            this.MainMenuView.Size = new System.Drawing.Size(60, 22);
            this.MainMenuView.Text = "表示(&V)";
            // 
            // MainMenuOnlyTcp
            // 
            this.MainMenuOnlyTcp.Checked = true;
            this.MainMenuOnlyTcp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MainMenuOnlyTcp.Name = "MainMenuOnlyTcp";
            this.MainMenuOnlyTcp.Size = new System.Drawing.Size(180, 22);
            this.MainMenuOnlyTcp.Text = "TCPのみ表示する(&T)";
            this.MainMenuOnlyTcp.Click += new System.EventHandler(this.MainMenuOnlyTcp_Click);
            // 
            // MainMenuMode
            // 
            this.MainMenuMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainMenuModeBind,
            this.MainMenuModePcap});
            this.MainMenuMode.Name = "MainMenuMode";
            this.MainMenuMode.Size = new System.Drawing.Size(72, 22);
            this.MainMenuMode.Text = "モード(&M)";
            // 
            // MainMenuModeBind
            // 
            this.MainMenuModeBind.Checked = true;
            this.MainMenuModeBind.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MainMenuModeBind.Name = "MainMenuModeBind";
            this.MainMenuModeBind.Size = new System.Drawing.Size(152, 22);
            this.MainMenuModeBind.Text = "Bind";
            this.MainMenuModeBind.Click += new System.EventHandler(this.MainMenuModeBind_Click);
            // 
            // MainMenuModePcap
            // 
            this.MainMenuModePcap.Name = "MainMenuModePcap";
            this.MainMenuModePcap.Size = new System.Drawing.Size(152, 22);
            this.MainMenuModePcap.Text = "Pcap";
            this.MainMenuModePcap.Click += new System.EventHandler(this.MainMenuModePcap_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 453);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1031, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 26);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBoxLog);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listViewCapture);
            this.splitContainer1.Size = new System.Drawing.Size(1031, 427);
            this.splitContainer1.SplitterDistance = 210;
            this.splitContainer1.TabIndex = 7;
            // 
            // listBoxLog
            // 
            this.listBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxLog.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.ItemHeight = 17;
            this.listBoxLog.Location = new System.Drawing.Point(0, 0);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new System.Drawing.Size(1031, 210);
            this.listBoxLog.TabIndex = 0;
            // 
            // listViewCapture
            // 
            this.listViewCapture.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader16,
            this.columnHeader17,
            this.columnHeader18,
            this.columnHeader19,
            this.columnHeader20,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewCapture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewCapture.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listViewCapture.FullRowSelect = true;
            this.listViewCapture.HideSelection = false;
            this.listViewCapture.Location = new System.Drawing.Point(0, 0);
            this.listViewCapture.Name = "listViewCapture";
            this.listViewCapture.Size = new System.Drawing.Size(1031, 213);
            this.listViewCapture.TabIndex = 0;
            this.listViewCapture.UseCompatibleStateImageBehavior = false;
            this.listViewCapture.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "送信先MACアドレス";
            this.columnHeader12.Width = 111;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "送信元MACアドレス";
            this.columnHeader13.Width = 109;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "タイプ";
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "プロトコル";
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "送信元IPアドレス";
            this.columnHeader16.Width = 99;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "送信先IPアドレス";
            this.columnHeader17.Width = 100;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "接続元ポート";
            this.columnHeader18.Width = 77;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "接続先ポート";
            this.columnHeader19.Width = 80;
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "サイズ";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Squence";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Ack";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "フラグ";
            this.columnHeader3.Width = 131;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1031, 475);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Hide&Seek";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem MainMenuFile;
        private System.Windows.Forms.ToolStripMenuItem MainMenuExit;
        private System.Windows.Forms.ToolStripMenuItem MainMenuClear;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.ListView listViewCapture;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.ColumnHeader columnHeader17;
        private System.Windows.Forms.ColumnHeader columnHeader18;
        private System.Windows.Forms.ColumnHeader columnHeader19;
        private System.Windows.Forms.ColumnHeader columnHeader20;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStripMenuItem MainMenuView;
        private System.Windows.Forms.ToolStripMenuItem MainMenuOnlyTcp;
        private System.Windows.Forms.ToolStripMenuItem MainMenuMode;
        private System.Windows.Forms.ToolStripMenuItem MainMenuModeBind;
        private System.Windows.Forms.ToolStripMenuItem MainMenuModePcap;
    }
}


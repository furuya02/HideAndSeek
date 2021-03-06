﻿namespace HideAndSeek {
    partial class OptionDlg {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.radioButtonBind = new System.Windows.Forms.RadioButton();
            this.radioButtonPcap = new System.Windows.Forms.RadioButton();
            this.checkBoxAckReply = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listBoxAdapter = new System.Windows.Forms.ListBox();
            this.radioButtonNone = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(12, 241);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(93, 241);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "キャンセル";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // radioButtonBind
            // 
            this.radioButtonBind.AutoSize = true;
            this.radioButtonBind.Location = new System.Drawing.Point(83, 12);
            this.radioButtonBind.Name = "radioButtonBind";
            this.radioButtonBind.Size = new System.Drawing.Size(46, 16);
            this.radioButtonBind.TabIndex = 8;
            this.radioButtonBind.Text = "Bind";
            this.radioButtonBind.UseVisualStyleBackColor = true;
            this.radioButtonBind.CheckedChanged += new System.EventHandler(this.checkBoxAckReply_CheckedChanged);
            // 
            // radioButtonPcap
            // 
            this.radioButtonPcap.AutoSize = true;
            this.radioButtonPcap.Location = new System.Drawing.Point(135, 12);
            this.radioButtonPcap.Name = "radioButtonPcap";
            this.radioButtonPcap.Size = new System.Drawing.Size(48, 16);
            this.radioButtonPcap.TabIndex = 9;
            this.radioButtonPcap.Text = "Pcap";
            this.radioButtonPcap.UseVisualStyleBackColor = true;
            this.radioButtonPcap.CheckedChanged += new System.EventHandler(this.checkBoxAckReply_CheckedChanged);
            // 
            // checkBoxAckReply
            // 
            this.checkBoxAckReply.AutoSize = true;
            this.checkBoxAckReply.Location = new System.Drawing.Point(21, 54);
            this.checkBoxAckReply.Name = "checkBoxAckReply";
            this.checkBoxAckReply.Size = new System.Drawing.Size(86, 16);
            this.checkBoxAckReply.TabIndex = 10;
            this.checkBoxAckReply.Text = "ACK Replay";
            this.checkBoxAckReply.UseVisualStyleBackColor = true;
            this.checkBoxAckReply.CheckedChanged += new System.EventHandler(this.checkBoxAckReply_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(113, 54);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(133, 89);
            this.textBox1.TabIndex = 11;
            // 
            // listBoxAdapter
            // 
            this.listBoxAdapter.FormattingEnabled = true;
            this.listBoxAdapter.ItemHeight = 12;
            this.listBoxAdapter.Location = new System.Drawing.Point(12, 159);
            this.listBoxAdapter.Name = "listBoxAdapter";
            this.listBoxAdapter.Size = new System.Drawing.Size(488, 76);
            this.listBoxAdapter.TabIndex = 12;
            // 
            // radioButtonNone
            // 
            this.radioButtonNone.AutoSize = true;
            this.radioButtonNone.Checked = true;
            this.radioButtonNone.Location = new System.Drawing.Point(21, 12);
            this.radioButtonNone.Name = "radioButtonNone";
            this.radioButtonNone.Size = new System.Drawing.Size(49, 16);
            this.radioButtonNone.TabIndex = 13;
            this.radioButtonNone.TabStop = true;
            this.radioButtonNone.Text = "None";
            this.radioButtonNone.UseVisualStyleBackColor = true;
            // 
            // OptionDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 276);
            this.Controls.Add(this.radioButtonNone);
            this.Controls.Add(this.listBoxAdapter);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkBoxAckReply);
            this.Controls.Add(this.radioButtonPcap);
            this.Controls.Add(this.radioButtonBind);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "オプション設定";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.RadioButton radioButtonBind;
        private System.Windows.Forms.RadioButton radioButtonPcap;
        private System.Windows.Forms.CheckBox checkBoxAckReply;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox listBoxAdapter;
        private System.Windows.Forms.RadioButton radioButtonNone;
    }
}
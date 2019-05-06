namespace ClipHistory {
    partial class Form1 {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.labelNext = new System.Windows.Forms.Label();
            this.labelMain = new System.Windows.Forms.Label();
            this.labelPrevious = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelNext
            // 
            this.labelNext.ForeColor = System.Drawing.Color.Snow;
            this.labelNext.Location = new System.Drawing.Point(0, 7);
            this.labelNext.Name = "labelNext";
            this.labelNext.Padding = new System.Windows.Forms.Padding(20, 10, 20, 5);
            this.labelNext.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelNext.Size = new System.Drawing.Size(217, 55);
            this.labelNext.TabIndex = 4;
            this.labelNext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMain
            // 
            this.labelMain.AutoEllipsis = true;
            this.labelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMain.ForeColor = System.Drawing.Color.Snow;
            this.labelMain.Location = new System.Drawing.Point(-8, 62);
            this.labelMain.Name = "labelMain";
            this.labelMain.Padding = new System.Windows.Forms.Padding(20, 10, 20, 5);
            this.labelMain.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelMain.Size = new System.Drawing.Size(235, 62);
            this.labelMain.TabIndex = 5;
            this.labelMain.Text = "label1";
            this.labelMain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPrevious
            // 
            this.labelPrevious.ForeColor = System.Drawing.Color.Snow;
            this.labelPrevious.Location = new System.Drawing.Point(0, 124);
            this.labelPrevious.Name = "labelPrevious";
            this.labelPrevious.Padding = new System.Windows.Forms.Padding(20, 10, 20, 5);
            this.labelPrevious.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelPrevious.Size = new System.Drawing.Size(217, 60);
            this.labelPrevious.TabIndex = 6;
            this.labelPrevious.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelPrevious);
            this.panel1.Controls.Add(this.labelMain);
            this.panel1.Controls.Add(this.labelNext);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(221, 188);
            this.panel1.TabIndex = 4;
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "notifyIcon1";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(222, 188);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResizeBegin += new System.EventHandler(this.Form1_ResizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_Keydown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelNext;
        private System.Windows.Forms.Label labelMain;
        private System.Windows.Forms.Label labelPrevious;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}


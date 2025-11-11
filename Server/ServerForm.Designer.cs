namespace Server
{
    partial class ServerForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtLog = new TextBox();
            lstClients = new ListBox();
            lblStatus = new Label();
            BtnStart = new Button();
            BtnStop = new Button();
            txtPort = new TextBox();
            txtIP = new TextBox();
            lable1 = new Label();
            label2 = new Label();
            btnSend = new Button();
            SuspendLayout();
            // 
            // txtLog
            // 
            txtLog.BackColor = SystemColors.Window;
            txtLog.Cursor = Cursors.IBeam;
            txtLog.Font = new Font("Microsoft Sans Serif", 7.8F);
            txtLog.Location = new Point(13, 100);
            txtLog.Margin = new Padding(4);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(568, 303);
            txtLog.TabIndex = 30;
            txtLog.TabStop = false;
            // 
            // lstClients
            // 
            lstClients.FormattingEnabled = true;
            lstClients.Location = new Point(638, 12);
            lstClients.Name = "lstClients";
            lstClients.Size = new Size(150, 344);
            lstClients.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(112, 16);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(50, 20);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "lable1";
            // 
            // BtnStart
            // 
            BtnStart.Location = new Point(12, 12);
            BtnStart.Name = "BtnStart";
            BtnStart.Size = new Size(94, 29);
            BtnStart.TabIndex = 3;
            BtnStart.Text = "Start";
            BtnStart.UseVisualStyleBackColor = true;
            BtnStart.Click += BtnStart_Click;
            // 
            // BtnStop
            // 
            BtnStop.Location = new Point(12, 47);
            BtnStop.Name = "BtnStop";
            BtnStop.Size = new Size(94, 29);
            BtnStop.TabIndex = 4;
            BtnStop.Text = "Stop";
            BtnStop.UseVisualStyleBackColor = true;
            BtnStop.Click += BtnStop_Click;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(479, 66);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(125, 27);
            txtPort.TabIndex = 5;
            txtPort.Text = "5000";
            // 
            // txtIP
            // 
            txtIP.Location = new Point(479, 25);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(125, 27);
            txtIP.TabIndex = 6;
            txtIP.Text = "127.0.0.1";
            // 
            // lable1
            // 
            lable1.AutoSize = true;
            lable1.Location = new Point(423, 28);
            lable1.Name = "lable1";
            lable1.Size = new Size(24, 20);
            lable1.TabIndex = 7;
            lable1.Text = "IP:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(423, 69);
            label2.Name = "label2";
            label2.Size = new Size(38, 20);
            label2.TabIndex = 8;
            label2.Text = "Port:";
            // 
            // btnSend
            // 
            btnSend.Location = new Point(610, 402);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(94, 29);
            btnSend.TabIndex = 9;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            // 
            // ServerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSend);
            Controls.Add(label2);
            Controls.Add(lable1);
            Controls.Add(txtIP);
            Controls.Add(txtPort);
            Controls.Add(BtnStop);
            Controls.Add(BtnStart);
            Controls.Add(lblStatus);
            Controls.Add(lstClients);
            Controls.Add(txtLog);
            Name = "ServerForm";
            Text = "Server";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtLog;
        private ListBox lstClients;
        private Label lblStatus;
        private Button BtnStart;
        private Button BtnStop;
        private TextBox txtIP;
        private TextBox txtPort;
        private Label lable1;
        private Label label2;
        private Button btnSend;
    }
}

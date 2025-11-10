namespace TcpChatServerWinForms
{
    partial class MainFormSV
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
            txtIP = new TextBox();
            txtPort = new TextBox();
            txtBroadcast = new TextBox();
            txtLog = new TextBox();
            btnStart = new Button();
            btnStop = new Button();
            btnSendAll = new Button();
            listUsers = new ListBox();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // txtIP
            // 
            txtIP.Location = new Point(56, 6);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(125, 27);
            txtIP.TabIndex = 0;
            txtIP.Text = "0.0.0.0";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(56, 42);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(125, 27);
            txtPort.TabIndex = 1;
            txtPort.Text = "5000";
            // 
            // txtBroadcast
            // 
            txtBroadcast.Location = new Point(313, 222);
            txtBroadcast.Name = "txtBroadcast";
            txtBroadcast.Size = new Size(125, 27);
            txtBroadcast.TabIndex = 2;
            // 
            // txtLog
            // 
            txtLog.Dock = DockStyle.Bottom;
            txtLog.Location = new Point(0, 423);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(800, 27);
            txtLog.TabIndex = 3;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 90);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(94, 29);
            btnStart.TabIndex = 4;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Enabled = false;
            btnStop.Location = new Point(112, 90);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(94, 29);
            btnStop.TabIndex = 5;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // btnSendAll
            // 
            btnSendAll.Location = new Point(461, 222);
            btnSendAll.Name = "btnSendAll";
            btnSendAll.Size = new Size(94, 29);
            btnSendAll.TabIndex = 6;
            btnSendAll.Text = "Send All";
            btnSendAll.UseVisualStyleBackColor = true;
            btnSendAll.Click += btnSendAll_Click;
            // 
            // listUsers
            // 
            listUsers.FormattingEnabled = true;
            listUsers.Location = new Point(638, 15);
            listUsers.Name = "listUsers";
            listUsers.Size = new Size(150, 104);
            listUsers.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(24, 20);
            label1.TabIndex = 8;
            label1.Text = "IP:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 42);
            label2.Name = "label2";
            label2.Size = new Size(38, 20);
            label2.TabIndex = 9;
            label2.Text = "Port:";
            // 
            // MainFormSV
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(listUsers);
            Controls.Add(btnSendAll);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(txtLog);
            Controls.Add(txtBroadcast);
            Controls.Add(txtPort);
            Controls.Add(txtIP);
            Name = "MainFormSV";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtIP;
        private TextBox txtPort;
        private TextBox txtBroadcast;
        private TextBox txtLog;
        private Button btnStart;
        private Button btnStop;
        private Button btnSendAll;
        private ListBox listUsers;
        private Label label1;
        private Label label2;
    }
}

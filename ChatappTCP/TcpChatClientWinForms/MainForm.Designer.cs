namespace TcpChatClientWinForms
{
    partial class MainForm
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
            txtServerIP = new TextBox();
            txtPort = new TextBox();
            txtUsername = new TextBox();
            Connect = new Button();
            btnDisconnect = new Button();
            lstUsers = new ListBox();
            chkPrivate = new CheckBox();
            txtChat = new TextBox();
            txtInput = new TextBox();
            btnSend = new Button();
            SuspendLayout();
            // 
            // txtServerIP
            // 
            txtServerIP.Location = new Point(517, 132);
            txtServerIP.Name = "txtServerIP";
            txtServerIP.Size = new Size(125, 27);
            txtServerIP.TabIndex = 0;
            txtServerIP.Text = "127.0.0.1";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(517, 179);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(125, 27);
            txtPort.TabIndex = 1;
            txtPort.Text = "5000";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(517, 227);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(125, 27);
            txtUsername.TabIndex = 2;
            // 
            // Connect
            // 
            Connect.Location = new Point(369, 65);
            Connect.Name = "Connect";
            Connect.Size = new Size(94, 29);
            Connect.TabIndex = 3;
            Connect.Text = "Connect";
            Connect.UseVisualStyleBackColor = true;
            Connect.Click += Connect_Click;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Enabled = false;
            btnDisconnect.Location = new Point(369, 115);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(94, 29);
            btnDisconnect.TabIndex = 4;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // lstUsers
            // 
            lstUsers.FormattingEnabled = true;
            lstUsers.Location = new Point(333, 237);
            lstUsers.Name = "lstUsers";
            lstUsers.Size = new Size(150, 104);
            lstUsers.TabIndex = 5;
            // 
            // chkPrivate
            // 
            chkPrivate.AutoSize = true;
            chkPrivate.Location = new Point(192, 155);
            chkPrivate.Name = "chkPrivate";
            chkPrivate.Size = new Size(153, 24);
            chkPrivate.TabIndex = 6;
            chkPrivate.Text = "Private to selected";
            chkPrivate.UseVisualStyleBackColor = true;
            // 
            // txtChat
            // 
            txtChat.Dock = DockStyle.Top;
            txtChat.Location = new Point(0, 0);
            txtChat.Multiline = true;
            txtChat.Name = "txtChat";
            txtChat.ReadOnly = true;
            txtChat.ScrollBars = ScrollBars.Vertical;
            txtChat.Size = new Size(800, 34);
            txtChat.TabIndex = 7;
            // 
            // txtInput
            // 
            txtInput.Location = new Point(142, 314);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(125, 27);
            txtInput.TabIndex = 8;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(369, 155);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(94, 29);
            btnSend.TabIndex = 9;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSend);
            Controls.Add(txtInput);
            Controls.Add(txtChat);
            Controls.Add(chkPrivate);
            Controls.Add(lstUsers);
            Controls.Add(btnDisconnect);
            Controls.Add(Connect);
            Controls.Add(txtUsername);
            Controls.Add(txtPort);
            Controls.Add(txtServerIP);
            Name = "MainForm";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtServerIP;
        private TextBox txtPort;
        private TextBox txtUsername;
        private Button Connect;
        private Button btnDisconnect;
        private ListBox lstUsers;
        private CheckBox chkPrivate;
        private TextBox txtChat;
        private TextBox txtInput;
        private Button btnSend;
    }
}

namespace chatappTCP
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            UserDis = new Button();
            txtPort = new TextBox();
            label2 = new Label();
            label1 = new Label();
            txtIP = new TextBox();
            textBox1 = new TextBox();
            label3 = new Label();
            label4 = new Label();
            textBox2 = new TextBox();
            btnSendBroadcast = new Button();
            txtBroadcast = new TextBox();
            txtLog = new TextBox();
            SuspendLayout();
            // 
            // UserDis
            // 
            UserDis.Location = new Point(12, 17);
            UserDis.Name = "UserDis";
            UserDis.Size = new Size(123, 31);
            UserDis.TabIndex = 0;
            UserDis.Text = "Disconnect";
            UserDis.UseVisualStyleBackColor = true;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(937, 17);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(114, 27);
            txtPort.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(896, 20);
            label2.Name = "label2";
            label2.Size = new Size(35, 20);
            label2.TabIndex = 8;
            label2.Text = "Port";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(661, 20);
            label1.Name = "label1";
            label1.Size = new Size(78, 20);
            label1.TabIndex = 7;
            label1.Text = "IP Address";
            // 
            // txtIP
            // 
            txtIP.Location = new Point(745, 17);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(112, 27);
            txtIP.TabIndex = 6;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(937, 63);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(114, 27);
            textBox1.TabIndex = 13;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(896, 66);
            label3.Name = "label3";
            label3.Size = new Size(33, 20);
            label3.TabIndex = 12;
            label3.Text = "Key";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(661, 66);
            label4.Name = "label4";
            label4.Size = new Size(75, 20);
            label4.TabIndex = 11;
            label4.Text = "Username";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(745, 63);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(112, 27);
            textBox2.TabIndex = 10;
            // 
            // btnSendBroadcast
            // 
            btnSendBroadcast.Location = new Point(904, 665);
            btnSendBroadcast.Name = "btnSendBroadcast";
            btnSendBroadcast.Size = new Size(126, 29);
            btnSendBroadcast.TabIndex = 16;
            btnSendBroadcast.Text = "Send";
            btnSendBroadcast.UseVisualStyleBackColor = true;
            // 
            // txtBroadcast
            // 
            txtBroadcast.Location = new Point(38, 667);
            txtBroadcast.Name = "txtBroadcast";
            txtBroadcast.Size = new Size(860, 27);
            txtBroadcast.TabIndex = 15;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(38, 132);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(992, 529);
            txtLog.TabIndex = 14;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientActiveCaption;
            ClientSize = new Size(1063, 701);
            Controls.Add(btnSendBroadcast);
            Controls.Add(txtBroadcast);
            Controls.Add(txtLog);
            Controls.Add(textBox1);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(textBox2);
            Controls.Add(txtPort);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtIP);
            Controls.Add(UserDis);
            MaximizeBox = false;
            Name = "Form2";
            Text = "Form2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button UserDis;
        private TextBox txtPort;
        private Label label2;
        private Label label1;
        private TextBox txtIP;
        private TextBox textBox1;
        private Label label3;
        private Label label4;
        private TextBox textBox2;
        private Button btnSendBroadcast;
        private TextBox txtBroadcast;
        private TextBox txtLog;
    }
}
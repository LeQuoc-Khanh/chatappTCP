namespace chatappTCP
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
            txtIP = new TextBox();
            btnStart = new Button();
            btnStop = new Button();
            label1 = new Label();
            label2 = new Label();
            txtPort = new TextBox();
            label3 = new Label();
            label4 = new Label();
            txtLog = new TextBox();
            txtBroadcast = new TextBox();
            btnSendBroadcast = new Button();
            dataGridView1 = new DataGridView();
            ColName = new DataGridViewTextBoxColumn();
            ColKick = new DataGridViewButtonColumn();
            DisAll = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // txtIP
            // 
            txtIP.Location = new Point(475, 12);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(112, 27);
            txtIP.TabIndex = 0;
            txtIP.TextChanged += textBox1_TextChanged;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(25, 60);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(112, 31);
            btnStart.TabIndex = 1;
            btnStart.Text = "Start Server";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += button1_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(143, 62);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(114, 29);
            btnStop.TabIndex = 2;
            btnStop.Text = "Stop Server";
            btnStop.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(391, 15);
            label1.Name = "label1";
            label1.Size = new Size(78, 20);
            label1.TabIndex = 3;
            label1.Text = "IP Address";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(432, 48);
            label2.Name = "label2";
            label2.Size = new Size(35, 20);
            label2.TabIndex = 4;
            label2.Text = "Port";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(473, 45);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(114, 27);
            txtPort.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(755, 15);
            label3.Name = "label3";
            label3.Size = new Size(100, 20);
            label3.TabIndex = 6;
            label3.Text = "Clients Online";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(25, 110);
            label4.Name = "label4";
            label4.Size = new Size(107, 20);
            label4.TabIndex = 9;
            label4.Text = "Server Console";
            // 
            // txtLog
            // 
            txtLog.Location = new Point(25, 133);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(724, 495);
            txtLog.TabIndex = 10;
            // 
            // txtBroadcast
            // 
            txtBroadcast.Location = new Point(25, 635);
            txtBroadcast.Name = "txtBroadcast";
            txtBroadcast.Size = new Size(624, 27);
            txtBroadcast.TabIndex = 11;
            // 
            // btnSendBroadcast
            // 
            btnSendBroadcast.Location = new Point(655, 634);
            btnSendBroadcast.Name = "btnSendBroadcast";
            btnSendBroadcast.Size = new Size(94, 29);
            btnSendBroadcast.TabIndex = 12;
            btnSendBroadcast.Text = "Send";
            btnSendBroadcast.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeight = 29;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ColName, ColKick });
            dataGridView1.Location = new Point(760, 50);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView1.Size = new Size(289, 608);
            dataGridView1.TabIndex = 13;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // ColName
            // 
            ColName.HeaderText = "Name";
            ColName.MinimumWidth = 6;
            ColName.Name = "ColName";
            ColName.ReadOnly = true;
            ColName.Width = 125;
            // 
            // ColKick
            // 
            ColKick.HeaderText = "Disconnect";
            ColKick.MinimumWidth = 6;
            ColKick.Name = "ColKick";
            ColKick.Text = "Kick";
            ColKick.Width = 125;
            // 
            // DisAll
            // 
            DisAll.Location = new Point(629, 96);
            DisAll.Name = "DisAll";
            DisAll.Size = new Size(122, 32);
            DisAll.TabIndex = 14;
            DisAll.Text = "Disconnect All";
            DisAll.UseVisualStyleBackColor = true;
            // 
            // ServerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientActiveCaption;
            ClientSize = new Size(1063, 701);
            Controls.Add(DisAll);
            Controls.Add(dataGridView1);
            Controls.Add(btnSendBroadcast);
            Controls.Add(txtBroadcast);
            Controls.Add(txtLog);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(txtPort);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(txtIP);
            ForeColor = SystemColors.ControlText;
            MaximizeBox = false;
            Name = "ServerForm";
            Text = "Server";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtIP;
        private Button btnStart;
        private Button btnStop;
        private Label label1;
        private Label label2;
        private TextBox txtPort;
        private Label label3;
        private Label label4;
        private TextBox txtLog;
        private TextBox txtBroadcast;
        private Button btnSendBroadcast;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn ColName;
        private DataGridViewButtonColumn ColKick;
        private Button DisAll;
    }
}

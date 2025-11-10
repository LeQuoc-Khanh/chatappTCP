namespace ServerApp
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
            label1 = new Label();
            label2 = new Label();
            txtIP = new TextBox();
            txtPort = new TextBox();
            dataGridView1 = new DataGridView();
            btnServer = new Button();
            txtLog = new TextBox();
            txtBroadcast = new TextBox();
            btnSendBroadcast = new Button();
            DisAll = new Button();
            ColName = new DataGridViewTextBoxColumn();
            ColKick = new DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(577, 28);
            label1.Name = "label1";
            label1.Size = new Size(62, 20);
            label1.TabIndex = 0;
            label1.Text = "Address";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(604, 67);
            label2.Name = "label2";
            label2.Size = new Size(35, 20);
            label2.TabIndex = 1;
            label2.Text = "Port";
            // 
            // txtIP
            // 
            txtIP.Location = new Point(645, 28);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(125, 27);
            txtIP.TabIndex = 2;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(645, 64);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(125, 27);
            txtPort.TabIndex = 3;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ColName, ColKick });
            dataGridView1.Location = new Point(787, 28);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(236, 606);
            dataGridView1.TabIndex = 4;
            // 
            // btnServer
            // 
            btnServer.Location = new Point(12, 46);
            btnServer.Name = "btnServer";
            btnServer.Size = new Size(114, 45);
            btnServer.TabIndex = 5;
            btnServer.Text = "Start Server";
            btnServer.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(12, 145);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(769, 445);
            txtLog.TabIndex = 6;
            // 
            // txtBroadcast
            // 
            txtBroadcast.Location = new Point(12, 607);
            txtBroadcast.Name = "txtBroadcast";
            txtBroadcast.Size = new Size(627, 27);
            txtBroadcast.TabIndex = 7;
            // 
            // btnSendBroadcast
            // 
            btnSendBroadcast.Location = new Point(645, 605);
            btnSendBroadcast.Name = "btnSendBroadcast";
            btnSendBroadcast.Size = new Size(136, 29);
            btnSendBroadcast.TabIndex = 8;
            btnSendBroadcast.Text = "Send";
            btnSendBroadcast.UseVisualStyleBackColor = true;
            // 
            // DisAll
            // 
            DisAll.Location = new Point(645, 110);
            DisAll.Name = "DisAll";
            DisAll.Size = new Size(125, 29);
            DisAll.TabIndex = 9;
            DisAll.Text = "Disconnect All";
            DisAll.UseVisualStyleBackColor = true;
            // 
            // ColName
            // 
            ColName.HeaderText = "Name";
            ColName.MinimumWidth = 6;
            ColName.Name = "ColName";
            ColName.ReadOnly = true;
            // 
            // ColKick
            // 
            ColKick.HeaderText = "Disconnect";
            ColKick.MinimumWidth = 6;
            ColKick.Name = "ColKick";
            // 
            // ServerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientActiveCaption;
            ClientSize = new Size(1035, 646);
            Controls.Add(DisAll);
            Controls.Add(btnSendBroadcast);
            Controls.Add(txtBroadcast);
            Controls.Add(txtLog);
            Controls.Add(btnServer);
            Controls.Add(dataGridView1);
            Controls.Add(txtPort);
            Controls.Add(txtIP);
            Controls.Add(label2);
            Controls.Add(label1);
            MaximizeBox = false;
            Name = "ServerForm";
            Text = "Server";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtIP;
        private TextBox txtPort;
        private DataGridView dataGridView1;
        private Button btnServer;
        private TextBox txtLog;
        private TextBox txtBroadcast;
        private Button btnSendBroadcast;
        private Button DisAll;
        private DataGridViewTextBoxColumn ColName;
        private DataGridViewButtonColumn ColKick;
    }
}

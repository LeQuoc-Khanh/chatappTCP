namespace Client
{
    partial class Client
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Client));
            addrTextBox = new TextBox();
            portTextBox = new TextBox();
            usernameTextBox = new TextBox();
            keyTextBox = new TextBox();
            logTextBox = new TextBox();
            sendTextBox = new TextBox();
            connectButton = new Button();
            clearButton = new Button();
            localaddrLabel = new Label();
            usernameLabel = new Label();
            logLabel = new Label();
            portLabel = new Label();
            keyLabel = new Label();
            sendLabel = new Label();
            SuspendLayout();
            // 
            // addrTextBox
            // 
            addrTextBox.Cursor = Cursors.IBeam;
            resources.ApplyResources(addrTextBox, "addrTextBox");
            addrTextBox.Name = "addrTextBox";
            addrTextBox.TabStop = false;
            // 
            // portTextBox
            // 
            portTextBox.Cursor = Cursors.IBeam;
            resources.ApplyResources(portTextBox, "portTextBox");
            portTextBox.Name = "portTextBox";
            portTextBox.TabStop = false;
            // 
            // usernameTextBox
            // 
            resources.ApplyResources(usernameTextBox, "usernameTextBox");
            usernameTextBox.Name = "usernameTextBox";
            // 
            // keyTextBox
            // 
            keyTextBox.Cursor = Cursors.IBeam;
            resources.ApplyResources(keyTextBox, "keyTextBox");
            keyTextBox.Name = "keyTextBox";
            keyTextBox.TabStop = false;
            // 
            // logTextBox
            // 
            logTextBox.Cursor = Cursors.IBeam;
            resources.ApplyResources(logTextBox, "logTextBox");
            logTextBox.Name = "logTextBox";
            logTextBox.ReadOnly = true;
            logTextBox.TabStop = false;
            // 
            // sendTextBox
            // 
            sendTextBox.Cursor = Cursors.IBeam;
            resources.ApplyResources(sendTextBox, "sendTextBox");
            sendTextBox.Name = "sendTextBox";
            sendTextBox.TabStop = false;
            // 
            // connectButton
            // 
            resources.ApplyResources(connectButton, "connectButton");
            connectButton.Name = "connectButton";
            connectButton.TabStop = false;
            connectButton.UseVisualStyleBackColor = true;
            connectButton.Click += connectButton_Click;
            // 
            // clearButton
            // 
            resources.ApplyResources(clearButton, "clearButton");
            clearButton.Name = "clearButton";
            clearButton.TabStop = false;
            clearButton.UseVisualStyleBackColor = true;
            clearButton.Click += clearButton_Click;
            // 
            // localaddrLabel
            // 
            resources.ApplyResources(localaddrLabel, "localaddrLabel");
            localaddrLabel.Name = "localaddrLabel";
            // 
            // usernameLabel
            // 
            resources.ApplyResources(usernameLabel, "usernameLabel");
            usernameLabel.Name = "usernameLabel";
            // 
            // logLabel
            // 
            resources.ApplyResources(logLabel, "logLabel");
            logLabel.Name = "logLabel";
            // 
            // portLabel
            // 
            resources.ApplyResources(portLabel, "portLabel");
            portLabel.Name = "portLabel";
            // 
            // keyLabel
            // 
            resources.ApplyResources(keyLabel, "keyLabel");
            keyLabel.Name = "keyLabel";
            // 
            // sendLabel
            // 
            resources.ApplyResources(sendLabel, "sendLabel");
            sendLabel.Name = "sendLabel";
            // 
            // Client
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.GradientActiveCaption;
            resources.ApplyResources(this, "$this");
            Controls.Add(sendLabel);
            Controls.Add(keyLabel);
            Controls.Add(portLabel);
            Controls.Add(logLabel);
            Controls.Add(usernameLabel);
            Controls.Add(localaddrLabel);
            Controls.Add(clearButton);
            Controls.Add(connectButton);
            Controls.Add(sendTextBox);
            Controls.Add(logTextBox);
            Controls.Add(keyTextBox);
            Controls.Add(usernameTextBox);
            Controls.Add(portTextBox);
            Controls.Add(addrTextBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Client";
            ShowIcon = false;
            Load += Client_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox addrTextBox;
        private TextBox portTextBox;
        private TextBox usernameTextBox;
        private TextBox keyTextBox;
        private TextBox logTextBox;
        private TextBox sendTextBox;
        private Button connectButton;
        private Button clearButton;
        private Label localaddrLabel;
        private Label usernameLabel;
        private Label logLabel;
        private Label portLabel;
        private Label keyLabel;
        private Label sendLabel;
    }
}

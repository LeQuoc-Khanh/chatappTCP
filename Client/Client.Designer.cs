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
            textBox3 = new TextBox();
            keyTextBox = new TextBox();
            logTextBox = new TextBox();
            sendTextButton = new TextBox();
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
            // textBox3
            // 
            resources.ApplyResources(textBox3, "textBox3");
            textBox3.Name = "textBox3";
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
            // sendTextButton
            // 
            sendTextButton.Cursor = Cursors.IBeam;
            resources.ApplyResources(sendTextButton, "sendTextButton");
            sendTextButton.Name = "sendTextButton";
            sendTextButton.TabStop = false;
            // 
            // connectButton
            // 
            resources.ApplyResources(connectButton, "connectButton");
            connectButton.Name = "connectButton";
            connectButton.TabStop = false;
            connectButton.UseVisualStyleBackColor = true;
            // 
            // clearButton
            // 
            resources.ApplyResources(clearButton, "clearButton");
            clearButton.Name = "clearButton";
            clearButton.TabStop = false;
            clearButton.UseVisualStyleBackColor = true;
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
            Controls.Add(sendTextButton);
            Controls.Add(logTextBox);
            Controls.Add(keyTextBox);
            Controls.Add(textBox3);
            Controls.Add(portTextBox);
            Controls.Add(addrTextBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Client";
            ShowIcon = false;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox addrTextBox;
        private TextBox portTextBox;
        private TextBox textBox3;
        private TextBox keyTextBox;
        private TextBox logTextBox;
        private TextBox sendTextButton;
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

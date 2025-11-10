namespace chatappTCP
{
    partial class LoginForm
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
            label1 = new Label();
            label2 = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            label3 = new Label();
            lblSignUp = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(203, 93);
            label1.Name = "label1";
            label1.Size = new Size(75, 20);
            label1.TabIndex = 0;
            label1.Text = "Username";
            label1.Click += Username_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(208, 161);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 1;
            label2.Text = "Password";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(332, 93);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(237, 27);
            txtUsername.TabIndex = 2;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(332, 161);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(237, 27);
            txtPassword.TabIndex = 3;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(383, 236);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(94, 29);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(275, 291);
            label3.Name = "label3";
            label3.Size = new Size(163, 20);
            label3.TabIndex = 5;
            label3.Text = "Don't have an account?";
            // 
            // lblSignUp
            // 
            lblSignUp.AutoSize = true;
            lblSignUp.Cursor = Cursors.Hand;
            lblSignUp.Font = new Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point, 163);
            lblSignUp.ForeColor = SystemColors.ActiveCaptionText;
            lblSignUp.Location = new Point(444, 291);
            lblSignUp.Name = "lblSignUp";
            lblSignUp.Size = new Size(91, 20);
            lblSignUp.TabIndex = 6;
            lblSignUp.Text = "Sign up now";
            lblSignUp.Click += lblSignUp_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientActiveCaption;
            ClientSize = new Size(800, 450);
            Controls.Add(lblSignUp);
            Controls.Add(label3);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(label2);
            Controls.Add(label1);
            MaximizeBox = false;
            Name = "LoginForm";
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label label3;
        private Label lblSignUp;
    }
}
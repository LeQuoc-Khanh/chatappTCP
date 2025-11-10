namespace ClientApp
{
    partial class RegisterForm
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
            label3 = new Label();
            label4 = new Label();
            btnUsername = new TextBox();
            btnEmail = new TextBox();
            btnPassword = new TextBox();
            btnCPassword = new TextBox();
            btnRegister = new Button();
            label5 = new Label();
            label6 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(264, 55);
            label1.Name = "label1";
            label1.Size = new Size(75, 20);
            label1.TabIndex = 0;
            label1.Text = "Username";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(293, 100);
            label2.Name = "label2";
            label2.Size = new Size(46, 20);
            label2.TabIndex = 1;
            label2.Text = "Email";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(269, 148);
            label3.Name = "label3";
            label3.Size = new Size(70, 20);
            label3.TabIndex = 2;
            label3.Text = "Password";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(212, 203);
            label4.Name = "label4";
            label4.Size = new Size(127, 20);
            label4.TabIndex = 3;
            label4.Text = "Confirm Password";
            // 
            // btnUsername
            // 
            btnUsername.Location = new Point(423, 55);
            btnUsername.Name = "btnUsername";
            btnUsername.Size = new Size(171, 27);
            btnUsername.TabIndex = 4;
            // 
            // btnEmail
            // 
            btnEmail.Location = new Point(423, 100);
            btnEmail.Name = "btnEmail";
            btnEmail.Size = new Size(171, 27);
            btnEmail.TabIndex = 5;
            // 
            // btnPassword
            // 
            btnPassword.Location = new Point(423, 148);
            btnPassword.Name = "btnPassword";
            btnPassword.Size = new Size(171, 27);
            btnPassword.TabIndex = 6;
            // 
            // btnCPassword
            // 
            btnCPassword.Location = new Point(423, 203);
            btnCPassword.Name = "btnCPassword";
            btnCPassword.Size = new Size(171, 27);
            btnCPassword.TabIndex = 7;
            // 
            // btnRegister
            // 
            btnRegister.Location = new Point(350, 268);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(94, 29);
            btnRegister.TabIndex = 8;
            btnRegister.Text = "Register";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += button1_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(289, 336);
            label5.Name = "label5";
            label5.Size = new Size(128, 20);
            label5.TabIndex = 9;
            label5.Text = "Have an Account?";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Cursor = Cursors.Hand;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point, 163);
            label6.Location = new Point(423, 336);
            label6.Name = "label6";
            label6.Size = new Size(78, 20);
            label6.TabIndex = 10;
            label6.Text = "Login now";
            label6.Click += label6_Click;
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientActiveCaption;
            ClientSize = new Size(800, 450);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(btnRegister);
            Controls.Add(btnCPassword);
            Controls.Add(btnPassword);
            Controls.Add(btnEmail);
            Controls.Add(btnUsername);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            MaximizeBox = false;
            Name = "RegisterForm";
            Text = "Register";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox btnUsername;
        private TextBox btnEmail;
        private TextBox btnPassword;
        private TextBox btnCPassword;
        private Button btnRegister;
        private Label label5;
        private Label label6;
    }
}
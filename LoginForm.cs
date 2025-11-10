using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chatappTCP
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void Username_Click(object sender, EventArgs e)
        {

        }

        private void lblSignUp_Click(object sender, EventArgs e)
        {
            SignUpForm signup = new SignUpForm();
            signup.Show();
            this.Hide(); // ẩn LoginForm
        }
    }
}

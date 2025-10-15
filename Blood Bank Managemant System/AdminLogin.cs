using System;
using System.Windows.Forms;

namespace Blood_Bank_Managemant_System
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        // Go back to normal login form
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }




        // Admin login button click
        private void button1_Click(object sender, EventArgs e)
        {
            // Hardcoded admin password 
            string correctAdminPassword = "pass"; 

            string enteredPassword = AdminPassword.Text.Trim();

            if (enteredPassword == correctAdminPassword)
            {
                this.Hide();
                Employees employees = new Employees();
                employees.Show();
            }
            else
            {
                MessageBox.Show(" Incorrect password. Please try again.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AdminPassword.Clear();
                AdminPassword.Focus();
            }
        }

        private void Password_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void AdminLogin_Load(object sender, EventArgs e)
        {

        }
    }
}

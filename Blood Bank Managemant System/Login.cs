using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Blood_Bank_Managemant_System
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get textbox values
            string username = Username.Text.Trim();
            string password = Password.Text.Trim();

            // Get the single instance of the Connection class
            Connection db = Connection.GetInstance();




            // Step 1: Check connection
            if (!db.TestConnection())
            {
                MessageBox.Show("Unable to connect to the database.\nPlease check your connection settings.",
                    "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Step 2: Validate username and password from Users table
            using (SqlConnection conn = db.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                try
                {
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {

                        this.Hide();
                        MainForm mainform = new MainForm();
                        mainform.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.\nPlease try again.",
                            "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while logging in:\n" + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            AdminLogin adminLogin = new AdminLogin();
            adminLogin.Show();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blood_Bank_Managemant_System
{
    public partial class Dashboard : Form
    {
        // 🔗 SQL connection — update server name if needed
        SqlConnection Con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=BloodBankDB;Integrated Security=True");

        public Dashboard()
        {
            InitializeComponent();
            LoadDashboardData();
            LoadBloodStock();
        }
        private void LoadDashboardData()
        {
            try
            {
                Con.Open();

                // 🩸 Count Donors
                SqlCommand cmdDonor = new SqlCommand("SELECT COUNT(*) FROM Donor", Con);
                int donorCount = (int)cmdDonor.ExecuteScalar();
                label12.Text = donorCount.ToString();

                // 👥 Count Patients (employees)
                SqlCommand cmdPatient = new SqlCommand("SELECT COUNT(*) FROM Patient", Con);
                int patientCount = (int)cmdPatient.ExecuteScalar();
                label14.Text = patientCount.ToString();

                // 💉 Count Transfers (if you create that table later)
                SqlCommand cmdTransfer = new SqlCommand("IF OBJECT_ID('Transfers', 'U') IS NOT NULL SELECT COUNT(*) FROM Transfers ELSE SELECT 0", Con);
                int transferCount = (int)cmdTransfer.ExecuteScalar();
                label13.Text = transferCount.ToString();

                Con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard data: " + ex.Message);
            }
        }

        private void LoadBloodStock()
        {
            string connectionString = "Data Source=YOUR_SERVER_NAME;Initial Catalog=BloodBankDB;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT BloodGroup, BStock FROM BloodStock";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string group = reader["BloodGroup"].ToString();
                    int stock = Convert.ToInt32(reader["BStock"]);

                    switch (group)
                    {
                        case "O+":
                            guna2CircleProgressBar1.Value = stock;
                            guna2CircleProgressBar1.Text = stock.ToString();
                            break;

                        case "A+":
                            guna2CircleProgressBar3.Value = stock;
                            guna2CircleProgressBar3.Text = stock.ToString();
                            break;
                        case "B+":
                            guna2CircleProgressBar4.Value = stock;
                            guna2CircleProgressBar4.Text = stock.ToString();
                            break;

                        case "AB+":
                            guna2CircleProgressBar2.Value = stock;
                            guna2CircleProgressBar2.Text = stock.ToString();
                            break;

                        case "O-":
                            guna2CircleProgressBar6.Value = stock;
                            guna2CircleProgressBar6.Text = stock.ToString();
                            break;
                        
                        case "AB-":
                            guna2CircleProgressBar5.Value = stock;
                            guna2CircleProgressBar5.Text = stock.ToString();
                            break;
                    }
                }

                reader.Close();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }
    }
}

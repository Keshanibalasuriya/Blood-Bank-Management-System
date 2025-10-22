using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Blood_Bank_Managemant_System
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            LoadDashboardData();
            LoadBloodStock();
        }

        //  Load total counts for Donors, Patients, and Transfers
        private void LoadDashboardData()
        {
            try
            {
                using (SqlConnection conn = Connection.GetInstance().GetConnection())
                {
                    conn.Open();

                    //  Count Donors
                    using (SqlCommand cmdDonor = new SqlCommand("SELECT COUNT(*) FROM Donor", conn))
                    {
                        label12.Text = ((int)cmdDonor.ExecuteScalar()).ToString();
                    }

                    //  Count Patients
                    using (SqlCommand cmdPatient = new SqlCommand("SELECT COUNT(*) FROM Users", conn))
                    {
                        label14.Text = ((int)cmdPatient.ExecuteScalar()).ToString();
                    }

                    //  Count Transfers (if table exists)
                    using (SqlCommand cmdTransfer = new SqlCommand(
                        "IF OBJECT_ID('Transfers', 'U') IS NOT NULL SELECT COUNT(*) FROM Transfers ELSE SELECT 0", conn))
                    {
                        label13.Text = ((int)cmdTransfer.ExecuteScalar()).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard data: " + ex.Message);
            }
        }

        //  Load blood stock levels into progress bars
        private void LoadBloodStock()
        {
            try
            {
                using (SqlConnection conn = Connection.GetInstance().GetConnection())
                {
                    conn.Open();
                    string query = "SELECT BloodGroup, BStock FROM BloodStock";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
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

                                case "A-":
                                    guna2CircleProgressBar7.Value = stock;
                                    guna2CircleProgressBar7.Text = stock.ToString();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading blood stock: " + ex.Message);
            }
        }

        // Optional UI event handlers
        private void Dashboard_Load(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void guna2Panel1_Paint(object sender, PaintEventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Donor doner = new Donor(); 
            doner.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();
            ViewDonor view = new ViewDonor();
            view.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Patient patient = new Patient();
            patient.Show();

        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Hide();
            ViewPatient viewPatient = new ViewPatient();
            viewPatient.Show();

        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Hide();
            BloodStock bloodStock = new BloodStock();
            bloodStock.Show();

        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Transfer tr = new Transfer();
            tr.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login lo = new Login();
            lo.Show();
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {
            this.Hide();
            BloodDonations bd = new BloodDonations();
            bd.Show();
        }
    }
}

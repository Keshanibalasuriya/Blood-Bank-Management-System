using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Blood_Bank_Managemant_System
{
    public partial class BloodStock : Form
    {
        public BloodStock()
        {
            InitializeComponent();
            LoadBloodStockCounts(); // Load counts when form loads
        }

        private void BloodStock_Load(object sender, EventArgs e)
        {
        }

        // Load blood stock directly from BloodStock table
        private void LoadBloodStockCounts()
        {
            try
            {
                using (SqlConnection conn = Connection.GetInstance().GetConnection())
                {
                    conn.Open();

                    textBox12.Text = GetBloodStock(conn, "A+").ToString();
                    textBox13.Text = GetBloodStock(conn, "A-").ToString();
                    textBox14.Text = GetBloodStock(conn, "AB+").ToString();
                    textBox16.Text = GetBloodStock(conn, "AB-").ToString();
                    textBox15.Text = GetBloodStock(conn, "B+").ToString();
                    textBox17.Text = GetBloodStock(conn, "B-").ToString();
                    textBox18.Text = GetBloodStock(conn, "O+").ToString();
                    textBox19.Text = GetBloodStock(conn, "O-").ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading blood stock: " + ex.Message);
            }
        }

        //  Get stock from BloodStock table
        private int GetBloodStock(SqlConnection conn, string bloodGroup)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT BStock FROM BloodStock WHERE BloodGroup = @bg", conn))
            {
                cmd.Parameters.AddWithValue("@bg", bloodGroup);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        //  Reduce stock for a blood group
        private void ReduceBloodStock(string bloodGroup, int quantity)
        {
            try
            {
                using (SqlConnection conn = Connection.GetInstance().GetConnection())
                {
                    conn.Open();

                    int currentStock = GetBloodStock(conn, bloodGroup);
                    if (currentStock < quantity)
                    {
                        MessageBox.Show($"Not enough stock for {bloodGroup}. Available: {currentStock}");
                        return;
                    }

                    using (SqlCommand cmd = new SqlCommand(
                        "UPDATE BloodStock SET BStock = BStock - @qty WHERE BloodGroup = @bg", conn))
                    {
                        cmd.Parameters.AddWithValue("@qty", quantity);
                        cmd.Parameters.AddWithValue("@bg", bloodGroup);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"{quantity} units of {bloodGroup} blood issued.");
                    LoadBloodStockCounts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reducing blood stock: " + ex.Message);
            }
        }

        //  Issue blood
        private void IssueBloodButton_Click(object sender, EventArgs e)
        {
            string selectedGroup = comboBox1.Text;
            int quantity = 1; // or take from textbox if needed

            if (!string.IsNullOrEmpty(selectedGroup))
            {
                ReduceBloodStock(selectedGroup, quantity);
            }
            else
            {
                MessageBox.Show("Please select a blood group.");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedGroup = comboBox1.Text;
            if (!string.IsNullOrEmpty(selectedGroup))
            {
                try
                {
                    using (SqlConnection conn = Connection.GetInstance().GetConnection())
                    {
                        conn.Open();
                        int count = GetBloodStock(conn, selectedGroup);
                        MessageBox.Show($"Current stock for {selectedGroup}: {count} units.",
                                        "Blood Stock Information",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching count: " + ex.Message);
                }
            }
        }

        // Navigation buttons (unchanged)
        private void label4_Click(object sender, EventArgs e)
        {
            this.Hide();
            ViewPatient viewPatient = new ViewPatient();
            viewPatient.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();
            ViewDonor viewDonor = new ViewDonor();
            viewDonor.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Patient patient = new Patient();
            patient.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Hide();
            BloodStock bloodStock = new BloodStock();
            bloodStock.ShowDialog();
            this.Show();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Transfer transfers = new Transfer();
            transfers.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dashboard dashbd = new Dashboard();
            dashbd.Show();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            this.Hide();
            ViewDonor viewDonor = new ViewDonor();
            viewDonor.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
        }

        private void label18_Click(object sender, EventArgs e)
        {
            this.Hide();
            BloodDonations bloodDonation = new BloodDonations();
            bloodDonation.Show();
        }
    }
}

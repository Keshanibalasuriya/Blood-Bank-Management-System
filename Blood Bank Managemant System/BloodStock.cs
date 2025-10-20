using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Blood_Bank_Managemant_System
{
    public partial class BloodStock : Form
    {
        private readonly string connString =
            "Data Source=KALPANI\\SQLEXPRESS;Initial Catalog=BloodBankDB;Integrated Security=True;TrustServerCertificate=True;";

        public BloodStock()
        {
            InitializeComponent();
            LoadBloodStockCounts(); // Load counts when form loads
        }

        private void BloodStock_Load(object sender, EventArgs e)
        {
            // Already loading counts in constructor
        }

        // Load all blood stock counts into textboxes
        private void LoadBloodStockCounts()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    textBox12.Text = GetBloodGroupCount(conn, "A+").ToString();
                    textBox13.Text = GetBloodGroupCount(conn, "A-").ToString();
                    textBox14.Text = GetBloodGroupCount(conn, "AB+").ToString();
                    textBox16.Text = GetBloodGroupCount(conn, "AB-").ToString();
                    textBox15.Text = GetBloodGroupCount(conn, "B+").ToString();
                    textBox17.Text = GetBloodGroupCount(conn, "B-").ToString();
                    textBox18.Text = GetBloodGroupCount(conn, "O+").ToString();
                    textBox19.Text = GetBloodGroupCount(conn, "O-").ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading blood stock: " + ex.Message);
            }
        }

        // Get count of a specific blood group
        private int GetBloodGroupCount(SqlConnection conn, string bloodGroup)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Donor WHERE BloodGroup = @bloodGroup", conn))
            {
                cmd.Parameters.AddWithValue("@bloodGroup", bloodGroup);
                return (int)cmd.ExecuteScalar();
            }
        }

        // Reduce stock for a blood group
        private void ReduceBloodStock(string bloodGroup, int quantity)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // Check current stock
                    int currentStock = GetBloodGroupCount(conn, bloodGroup);
                    if (currentStock < quantity)
                    {
                        MessageBox.Show($"Not enough stock for {bloodGroup}. Available: {currentStock}");
                        return;
                    }

                    // Delete donors equal to quantity (or mark as used)
                    using (SqlCommand cmd = new SqlCommand(
                        "DELETE TOP(@qty) FROM Donor WHERE BloodGroup = @bloodGroup", conn))
                    {
                        cmd.Parameters.AddWithValue("@bloodGroup", bloodGroup);
                        cmd.Parameters.AddWithValue("@qty", quantity);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        MessageBox.Show($"{rowsAffected} units of {bloodGroup} blood issued.");
                    }

                    // Reload updated counts
                    LoadBloodStockCounts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reducing blood stock: " + ex.Message);
            }
        }

        // Example: issue blood button click
        private void IssueBloodButton_Click(object sender, EventArgs e)
        {
            string selectedGroup = comboBox1.Text;
            int quantity = 1; // You can also get this from a textbox if issuing multiple units
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
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        int count = GetBloodGroupCount(conn, selectedGroup);
                        MessageBox.Show($"{selectedGroup} Blood Stock : {count} units 😎");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching count: " + ex.Message);
                }
            }
        }

        // Navigation buttons
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
            Transfers transfers = new Transfers();
            transfers.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.Show();
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

    }
}

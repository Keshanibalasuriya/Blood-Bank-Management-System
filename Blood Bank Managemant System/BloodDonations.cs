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
    public partial class BloodDonations : Form
    {
        public BloodDonations()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Donor donor = new Donor();
            donor.Show();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        //search donor by name btn
        private void button4_Click(object sender, EventArgs e)
        {
            string donorName = SearchTxt.Text.Trim();

            if (string.IsNullOrEmpty(donorName))
            {
                MessageBox.Show("Please enter a donor name to search.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Connection db = Connection.GetInstance();
                using (SqlConnection conn = db.GetConnection())
                {
                    string query = "SELECT DonorID, Name, Age, Gender, PhoneNo, Address, BloodGroup " +
                                   "FROM Donor WHERE LOWER(Name) LIKE LOWER(@Name + '%')";  // case-insensitive search

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", donorName);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No donor found with that name.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        guna2DataGridView1.DataSource = null; // clear previous data if any
                    }
                    else
                    {
                        guna2DataGridView1.DataSource = dt; // show results in grid
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BloodDonations_Load(object sender, EventArgs e)
        {
            LoadDonations(); // show existing donation records when form opens
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Transfer transfer = new Transfer();
            transfer.Show();
        }


        //Donated list
        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        //blood donated 
        private void transferbtn_Click(object sender, EventArgs e)
        {
            // Check if a donor row is selected
            if (guna2DataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a donor first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];

            int donorID = Convert.ToInt32(selectedRow.Cells["DonorID"].Value);
            string donorName = selectedRow.Cells["Name"].Value.ToString();
            string bloodGroup = selectedRow.Cells["BloodGroup"].Value.ToString();
            DateTime donatedDate = DateTime.Now;

            DialogResult result = MessageBox.Show(
                $"Confirm blood donation for {donorName} ({bloodGroup})?",
                "Confirm Donation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Connection db = Connection.GetInstance();
                    using (SqlConnection conn = db.GetConnection())
                    {
                        string insertQuery = "INSERT INTO Donations (DonorID, DonorName, BloodGroup, DonatedDate) " +
                                             "VALUES (@DonorID, @DonorName, @BloodGroup, @DonatedDate)";

                        SqlCommand cmd = new SqlCommand(insertQuery, conn);
                        cmd.Parameters.AddWithValue("@DonorID", donorID);
                        cmd.Parameters.AddWithValue("@DonorName", donorName);
                        cmd.Parameters.AddWithValue("@BloodGroup", bloodGroup);
                        cmd.Parameters.AddWithValue("@DonatedDate", donatedDate);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Donation recorded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh donation list after adding new one
                        LoadDonations();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to record donation.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadDonations()
        {
            try
            {
                Connection db = Connection.GetInstance();
                using (SqlConnection conn = db.GetConnection())
                {
                    string query = "SELECT DonorID, DonorName, BloodGroup, DonatedDate " +
                                   "FROM Donations ORDER BY DonatedDate DESC"; // latest first

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    guna2DataGridView2.DataSource = dt;
                    guna2DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load donations.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();
            ViewDonor viewDonor = new ViewDonor();
            viewDonor.Show();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            this.Show();
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

        private void label8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }
    }
}

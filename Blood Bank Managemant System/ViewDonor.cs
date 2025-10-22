using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Blood_Bank_Managemant_System
{
    public partial class ViewDonor : Form
    {
        // To store the currently selected donor’s ID
        private int selectedDonorID = -1;

        public ViewDonor()
        {
            InitializeComponent();
        }

        private void ViewDonor_Load_1(object sender, EventArgs e)
        {
            LoadDonors();
        }

        // ✅ Load Donors from database
        private void LoadDonors()
        {
            try
            {
                Connection db = Connection.GetInstance();

                using (SqlConnection conn = db.GetConnection())
                {
                    string query = "SELECT DonorID, Name, Age, Gender, PhoneNo, Address, BloodGroup FROM Donor";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    guna2DataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load donor data.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ When user clicks a row in the DataGridView
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedDonorID = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["DonorID"].Value);
                NameTxt.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                AgeTxt.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["Age"].Value.ToString();
                GenderCmb.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["Gender"].Value.ToString();
                PhoneNoTxt.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["PhoneNo"].Value.ToString();
                AddressTxt.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["Address"].Value.ToString();
                BloodGroupCmb.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["BloodGroup"].Value.ToString();
            }
        }

        // ✅ Update Donor
        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedDonorID == -1)
            {
                MessageBox.Show("Please select a donor to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(NameTxt.Text) || string.IsNullOrEmpty(AgeTxt.Text) ||
                string.IsNullOrEmpty(GenderCmb.Text) || string.IsNullOrEmpty(PhoneNoTxt.Text) ||
                string.IsNullOrEmpty(AddressTxt.Text) || string.IsNullOrEmpty(BloodGroupCmb.Text))
            {
                MessageBox.Show("Please fill all fields before updating.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Connection db = Connection.GetInstance();
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE Donor 
                                     SET Name=@Name, Age=@Age, Gender=@Gender, PhoneNo=@PhoneNo, 
                                         Address=@Address, BloodGroup=@BloodGroup 
                                     WHERE DonorID=@DonorID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", NameTxt.Text.Trim());
                    cmd.Parameters.AddWithValue("@Age", Convert.ToInt32(AgeTxt.Text.Trim()));
                    cmd.Parameters.AddWithValue("@Gender", GenderCmb.Text);
                    cmd.Parameters.AddWithValue("@PhoneNo", PhoneNoTxt.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", AddressTxt.Text.Trim());
                    cmd.Parameters.AddWithValue("@BloodGroup", BloodGroupCmb.Text);
                    cmd.Parameters.AddWithValue("@DonorID", selectedDonorID);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Donor updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadDonors();
                selectedDonorID = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update donor.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Delete Donor
        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedDonorID == -1)
            {
                MessageBox.Show("Please select a donor to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this donor?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Connection db = Connection.GetInstance();
                    using (SqlConnection conn = db.GetConnection())
                    {
                        conn.Open();
                        string query = "DELETE FROM Donor WHERE DonorID=@DonorID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@DonorID", selectedDonorID);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Donor deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadDonors();
                    selectedDonorID = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to delete donor.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ✅ Clear textboxes and combo boxes
        private void ClearFields()
        {
            NameTxt.Clear();
            AgeTxt.Clear();
            PhoneNoTxt.Clear();
            AddressTxt.Clear();
            GenderCmb.SelectedIndex = -1;
            BloodGroupCmb.SelectedIndex = -1;
        }

        // ✅ Navigation section
        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Donor donor = new Donor();
            donor.Show();
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
            Transfer transfers = new Transfer();
            transfers.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dashboard dashbd = new Dashboard();
            dashbd.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

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
                    string query = "SELECT Name, Age, Gender, PhoneNo, Address, BloodGroup " +
                                   "FROM Donor WHERE LOWER(Name) LIKE LOWER(@Name + '%')";  // case-insensitive

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", donorName);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No donor found with that name.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dt.Rows.Count == 1)
                    {
                        DataRow row = dt.Rows[0];
                        string details = $"Name: {row["Name"]}\n" +
                                         $"Age: {row["Age"]}\n" +
                                         $"Gender: {row["Gender"]}\n" +
                                         $"Phone: {row["PhoneNo"]}\n" +
                                         $"Address: {row["Address"]}\n" +
                                         $"Blood Group: {row["BloodGroup"]}";

                        MessageBox.Show($"Donor found!\n\n{details}", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"There are {dt.Rows.Count} donors with that name.\n" +
                                        "Please enter the full name (e.g., 'Nimal Perera').",
                                        "Multiple Matches Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void button4_Click_1(object sender, EventArgs e)
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
                    string query = "SELECT Name, Age, Gender, PhoneNo, Address, BloodGroup " +
                                   "FROM Donor WHERE LOWER(Name) LIKE LOWER(@Name + '%')";  // case-insensitive

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", donorName);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No donor found with that name.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dt.Rows.Count == 1)
                    {
                        DataRow row = dt.Rows[0];
                        string details = $"Name: {row["Name"]}\n" +
                                         $"Age: {row["Age"]}\n" +
                                         $"Gender: {row["Gender"]}\n" +
                                         $"Phone: {row["PhoneNo"]}\n" +
                                         $"Address: {row["Address"]}\n" +
                                         $"Blood Group: {row["BloodGroup"]}";

                        MessageBox.Show($"Donor found!\n\n{details}", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"There are {dt.Rows.Count} donors with that name.\n" +
                                        "Please enter the full name",
                                        "Multiple Matches Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {
            this.Hide();
            BloodDonations bloodDonation = new BloodDonations();
            bloodDonation.Show();
        }
    }
}

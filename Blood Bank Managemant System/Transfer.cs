using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Blood_Bank_Managemant_System
{
    public partial class Transfer : Form
    {
        private readonly Connection conn = Connection.GetInstance();

        public Transfer()
        {
            InitializeComponent();
        }

        //  Load Patient IDs + Transfer Records on Form Load
        private void Transfer_Load(object sender, EventArgs e)
        {
            LoadPatients();
            LoadTransferData(); // Always load latest transfers (most recent first)
        }

        // ✅ Load all patients into combo box
        private void LoadPatients()
        {
            Pid_ComboBox.Items.Clear();

            try
            {
                using (SqlConnection con = conn.GetConnection())
                {
                    con.Open();
                    string query = "SELECT PatientID FROM Patient";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Pid_ComboBox.Items.Add(reader["PatientID"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading patients: " + ex.Message);
            }
        }

        // ✅ When a patient is selected, load name & blood group + check stock
        private void Pid_ComboBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (Pid_ComboBox.SelectedIndex == -1) return;

            string patientID = Pid_ComboBox.SelectedItem.ToString();

            try
            {
                using (SqlConnection con = conn.GetConnection())
                {
                    con.Open();

                    // Get patient details
                    string queryPatient = "SELECT Pname, PBloodGroup FROM Patient WHERE PatientID = @pid";
                    string bloodGroup = "";

                    using (SqlCommand cmd = new SqlCommand(queryPatient, con))
                    {
                        cmd.Parameters.AddWithValue("@pid", patientID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pNametxt.Text = reader["Pname"].ToString();
                                bloodGroup = reader["PBloodGroup"].ToString();
                                bloodGrptxt.Text = bloodGroup;
                            }
                        }
                    }

                    // Check blood stock
                    string queryStock = "SELECT BStock FROM BloodStock WHERE BloodGroup = @bg";
                    using (SqlCommand cmdStock = new SqlCommand(queryStock, con))
                    {
                        cmdStock.Parameters.AddWithValue("@bg", bloodGroup);
                        object result = cmdStock.ExecuteScalar();
                        int stock = result != null ? Convert.ToInt32(result) : 0;

                        if (stock > 0)
                        {
                            stockStatus.Text = $"Stock Available: {stock}";
                            stockStatus.ForeColor = System.Drawing.Color.Green;
                            transferbtn.Enabled = true;
                        }
                        else
                        {
                            stockStatus.Text = "Stock Not Available";
                            stockStatus.ForeColor = System.Drawing.Color.Red;
                            transferbtn.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching data: " + ex.Message);
            }
        }

        //  Transfer blood to patient
        private void transferbtn_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(bloodGrptxt.Text))
            {
                MessageBox.Show("Please select a patient first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = conn.GetConnection())
                {
                    con.Open();

                    // Check stock
                    string checkQuery = "SELECT BStock FROM BloodStock WHERE BloodGroup = @bg";
                    using (SqlCommand cmdCheck = new SqlCommand(checkQuery, con))
                    {
                        cmdCheck.Parameters.AddWithValue("@bg", bloodGrptxt.Text);
                        int stock = Convert.ToInt32(cmdCheck.ExecuteScalar() ?? 0);

                        if (stock <= 0)
                        {
                            MessageBox.Show("Transfer failed. No stock available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Update BloodStock (reduce by 1)
                    string updateQuery = "UPDATE BloodStock SET BStock = BStock - 1 WHERE BloodGroup = @bg";
                    using (SqlCommand cmdUpdate = new SqlCommand(updateQuery, con))
                    {
                        cmdUpdate.Parameters.AddWithValue("@bg", bloodGrptxt.Text);
                        cmdUpdate.ExecuteNonQuery();
                    }

                    // 3️⃣ Insert transfer record
                    string insertQuery = "INSERT INTO Transfers (PatientID, PatientName, BloodGroup) VALUES (@pid, @pname, @bg)";
                    using (SqlCommand cmdInsert = new SqlCommand(insertQuery, con))
                    {
                        cmdInsert.Parameters.AddWithValue("@pid", Pid_ComboBox.SelectedItem.ToString());
                        cmdInsert.Parameters.AddWithValue("@pname", pNametxt.Text);
                        cmdInsert.Parameters.AddWithValue("@bg", bloodGrptxt.Text);
                        cmdInsert.ExecuteNonQuery();
                    }

                    MessageBox.Show("Blood transfer recorded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh UI
                    stockStatus.Text = "Transfer Completed";
                    stockStatus.ForeColor = System.Drawing.Color.Blue;
                    transferbtn.Enabled = false;

                    // Refresh table to show newest first
                    LoadTransferData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during transfer: " + ex.Message);
            }
        }

        //  Load all transfer records (latest first)
        private void LoadTransferData()
        {
            try
            {
                using (SqlConnection con = conn.GetConnection())
                {
                    con.Open();

                    // ORDER BY TransferID DESC → latest transfer first
                    string query = "SELECT TransferID, PatientID, PatientName, BloodGroup, TransferDate FROM Transfers ORDER BY TransferID DESC";

                    using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        guna2DataGridView1.DataSource = dt;

                        // Table styling
                        guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        guna2DataGridView1.ReadOnly = true;
                        guna2DataGridView1.AllowUserToAddRows = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading transfer data: " + ex.Message);
            }
        }

        // Navigation buttons
        private void label2_Click(object sender, EventArgs e) { this.Hide(); new Donor().Show(); }
        private void label3_Click(object sender, EventArgs e) { this.Hide(); new ViewDonor().Show(); }
        private void label5_Click(object sender, EventArgs e) { this.Hide(); new Patient().Show(); }
        private void label4_Click(object sender, EventArgs e) { this.Hide(); new ViewPatient().Show(); }
        private void label6_Click(object sender, EventArgs e) { this.Hide(); new BloodStock().Show(); }
        private void label7_Click(object sender, EventArgs e) { MessageBox.Show("Already in Transfer page", "Info"); }
        private void label8_Click(object sender, EventArgs e) { this.Hide(); new Dashboard().Show(); }
        private void button1_Click(object sender, EventArgs e) { this.Hide(); new Login().Show(); }
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void label18_Click(object sender, EventArgs e)
        {
            this.Hide();
            BloodDonations bloodDonation = new BloodDonations();
            bloodDonation.Show();
        }
    }
}

using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Blood_Bank_Managemant_System
{
    public partial class Transfer : Form
    {
        // Singleton connection instance
        private readonly Connection conn = Connection.GetInstance();

        public Transfer()
        {
            InitializeComponent();
        }

        // Load all Patient IDs into ComboBox
        private void Transfer_Load(object sender, EventArgs e)
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


        
        // 🔹 Navigation labels and buttons
        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Donor donor = new Donor();
            donor.Show();
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
            BloodStock bloodstock = new BloodStock();
            bloodstock.Show();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are already in the Transfer page", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        // When a patient is selected from ComboBox

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
                                string patientName = reader["Pname"].ToString();
                                bloodGroup = reader["PBloodGroup"].ToString();

                                // Fill textboxes
                                pNametxt.Text = patientName;
                                bloodGrptxt.Text = bloodGroup;

                                // ✅ Show a message box with patient details
                                MessageBox.Show(
                                    $"Patient Name: {patientName}\nBlood Group: {bloodGroup}",
                                    "Patient Details",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information
                                );
                            }
                        }
                    }

                    // Check stock after closing reader
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


        // 🔹 Transfer button click — reduce blood stock by 1
        
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

                    string updateQuery = "UPDATE BloodStock SET BStock = BStock - 1 WHERE BloodGroup = @bg AND BStock > 0";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@bg", bloodGrptxt.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Blood transfer successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Refresh stock display
                            string queryStock = "SELECT BStock FROM BloodStock WHERE BloodGroup = @bg";
                            using (SqlCommand cmdStock = new SqlCommand(queryStock, con))
                            {
                                cmdStock.Parameters.AddWithValue("@bg", bloodGrptxt.Text);
                                int newStock = Convert.ToInt32(cmdStock.ExecuteScalar() ?? 0);

                                if (newStock > 0)
                                {
                                    stockStatus.Text = $"Stock Available: {newStock}";
                                    stockStatus.ForeColor = System.Drawing.Color.Green;
                                }
                                else
                                {
                                    stockStatus.Text = "Stock Not Available";
                                    stockStatus.ForeColor = System.Drawing.Color.Red;
                                    transferbtn.Enabled = false;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Transfer failed. No stock available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            transferbtn.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during transfer: " + ex.Message);
            }
        }
    }
}

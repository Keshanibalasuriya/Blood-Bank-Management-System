using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;  // For SQL Server connectivity
using System.Configuration;   // If you’re using connection string from app.config
using System.Windows.Forms;

namespace Blood_Bank_Managemant_System
{
    public partial class transfer_btn : Form
    {
        public transfer_btn()
        {
            InitializeComponent();
        }


        private void transfer_btn_Load(object sender, EventArgs e)
        {
            Pid_ComboBox.Items.Clear(); // Clear any previous items

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["BloodBankDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT PatientID FROM Patient";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
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

        private void Pid_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Pid_ComboBox.SelectedIndex == -1) return;

            string patientID = Pid_ComboBox.SelectedItem.ToString();

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["BloodBankDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // 1️⃣ Get patient name and blood group
                    string queryPatient = "SELECT Pname, PBloodGroup FROM Patient WHERE PatientID=@pid";
                    using (SqlCommand cmd = new SqlCommand(queryPatient, conn))
                    {
                        cmd.Parameters.AddWithValue("@pid", patientID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pNametxt.Text = reader["Pname"].ToString();
                                bloodGrptxt.Text = reader["PBloodGroup"].ToString();
                            }
                        }
                    }

                    //  Check blood stock
                    string queryStock = "SELECT BStock FROM BloodStock WHERE BloodGroup=@bg";
                    using (SqlCommand cmdStock = new SqlCommand(queryStock, conn))
                    {
                        cmdStock.Parameters.AddWithValue("@bg", bloodGrptxt.Text);
                        int stock = Convert.ToInt32(cmdStock.ExecuteScalar() ?? 0);

                        if (stock > 0)
                        {
                            stockStatus.Text = "Stock Available: " + stock;
                            transferbtn.Enabled = true;
                        }
                        else
                        {
                            stockStatus.Text = "Stock Not Available";
                            transferbtn.Enabled = false;
                        }
                    }
                }

              

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }








        private void donor_Click(object sender, EventArgs e)
        {
            Donor doronForm = new Donor();
            doronForm.Show();
            this.Hide();
        }

        private void viewdonors_Click(object sender, EventArgs e)
        {
            ViewDonor viewDonorForm = new ViewDonor();
            viewDonorForm.Show();
            this.Hide();
        }

        private void patient_Click(object sender, EventArgs e)
        {
            Patient patientForm = new Patient();
            patientForm.Show();
            this.Hide();
        }

        private void viewpatient_Click(object sender, EventArgs e)
        {
            ViewPatient viewPatientForm = new ViewPatient();
            viewPatientForm.Show();
            this.Hide();

        }

        private void bloodstock_Click(object sender, EventArgs e)
        {
            BloodStock bloodStockForm = new BloodStock();
            bloodStockForm.Show();
            this.Hide();
        }

        private void dashboard_Click(object sender, EventArgs e)
        {
            //Dashboard dashboardForm = new Dashboard();
            //dashboardForm.Show();
            this.Hide();
        }

        private void transfer_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are already in the Transfer page", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        

        private void transferbtn_Click(object sender, EventArgs e)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["BloodBankDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // Decrease stock by 1
                    string updateQuery = "UPDATE BloodStock SET BStock = BStock - 1 WHERE BloodGroup = @bg AND BStock > 0";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@bg", bloodGrptxt.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Blood transfer successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                           
                            string queryStock = "SELECT BStock FROM BloodStock WHERE BloodGroup = @bg";
                            using (SqlCommand cmdStock = new SqlCommand(queryStock, conn))
                            {
                                cmdStock.Parameters.AddWithValue("@bg", bloodGrptxt.Text);
                                int newStock = Convert.ToInt32(cmdStock.ExecuteScalar() ?? 0);

                                stockStatus.Text = "Stock Available: " + newStock;
                                if (newStock <= 0)
                                {
                                    stockStatus.Text = "Stock Not Available";
                                    transferbtn.Enabled = false;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Transfer failed: no stock available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            transferbtn.Enabled = false;
                        }
                    }
                }

                Pid_ComboBox.SelectedIndex = -1;
                pNametxt.Clear();
                bloodGrptxt.Clear();
                stockStatus.Text = "";
                transferbtn.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during transfer: " + ex.Message);
            }
        }
    }
}

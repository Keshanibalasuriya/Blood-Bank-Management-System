using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Blood_Bank_Managemant_System
{
    public partial class ViewPatient : Form
    {
        private int selectedPatientID = -1; // store selected PatientID

        public ViewPatient()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Donor donor = new Donor();
            donor.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            ViewDonor viewDonor = new ViewDonor();
            viewDonor.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Patient patient = new Patient();
            patient.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            BloodStock bloodStock = new BloodStock();
            bloodStock.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Transfer transfers = new Transfer();
            transfers.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dashboard dashbd = new Dashboard();
            dashbd.Show();
        }

        private void ViewPatient_Load(object sender, EventArgs e)
        {
            LoadPatients();
        }

        private void LoadPatients()
        {
            try
            {
                Connection db = Connection.GetInstance();

                using (SqlConnection conn = db.GetConnection())
                {
                    string query = "SELECT PatientID, Pname, Page, Pphone, Paddress, Pgender, PBloodGroup FROM Patient";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    ViewpatienGV.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load patient data.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewpatienGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = ViewpatienGV.Rows[e.RowIndex];
                selectedPatientID = Convert.ToInt32(row.Cells["PatientID"].Value);

                // Fill textboxes
                Pname.Text = row.Cells["Pname"].Value.ToString();
                Page.Text = row.Cells["Page"].Value.ToString();
                Pphone.Text = row.Cells["Pphone"].Value.ToString();
                Paddress.Text = row.Cells["Paddress"].Value.ToString();
                Pgender.Text = row.Cells["Pgender"].Value.ToString();
                PBloodGroup.Text = row.Cells["PBloodGroup"].Value.ToString();
            }
        }

        // ✅ UPDATE
        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedPatientID == -1)
            {
                MessageBox.Show("Please select a patient record to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Pname.Text) || string.IsNullOrWhiteSpace(Page.Text))
            {
                MessageBox.Show("Please fill all required fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Connection db = Connection.GetInstance();
                using (SqlConnection conn = db.GetConnection())
                {
                    string query = @"UPDATE Patient
                                     SET Pname=@Pname, Page=@Page, Pphone=@Pphone, 
                                         Paddress=@Paddress, Pgender=@Pgender, PBloodGroup=@PBloodGroup
                                     WHERE PatientID=@PatientID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Pname", Pname.Text);
                    cmd.Parameters.AddWithValue("@Page", Page.Text);
                    cmd.Parameters.AddWithValue("@Pphone", Pphone.Text);
                    cmd.Parameters.AddWithValue("@Paddress", Paddress.Text);
                    cmd.Parameters.AddWithValue("@Pgender", Pgender.Text);
                    cmd.Parameters.AddWithValue("@PBloodGroup", PBloodGroup.Text);
                    cmd.Parameters.AddWithValue("@PatientID", selectedPatientID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                MessageBox.Show("Patient record updated successfully.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPatients();
                ClearFields();
                selectedPatientID = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update patient.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ DELETE
        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedPatientID == -1)
            {
                MessageBox.Show("Please select a patient record to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this patient?",
                                                   "Confirm Delete",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    Connection db = Connection.GetInstance();
                    using (SqlConnection conn = db.GetConnection())
                    {
                        string query = "DELETE FROM Patient WHERE PatientID=@PatientID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@PatientID", selectedPatientID);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    MessageBox.Show("Patient record deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPatients();
                    ClearFields();
                    selectedPatientID = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to delete patient.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ✅ Clear Textboxes
        private void ClearFields()
        {
            Pname.Clear();
            Page.Clear();
            Pphone.Clear();
            Paddress.Clear();
            Pgender.SelectedIndex = -1; 
            PBloodGroup.SelectedIndex = -1;
        }

        private void label18_Click(object sender, EventArgs e)
        {
            this.Hide();
            BloodDonations bd = new BloodDonations();
            bd.Show();
        }
    }
    }

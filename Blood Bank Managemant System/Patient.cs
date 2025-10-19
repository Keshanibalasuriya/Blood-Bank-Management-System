using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Blood_Bank_Managemant_System
{
    public partial class Patient : Form
    {
        public Patient()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            Donor donor = new Donor();
            donor.Show();
            this. Hide();
                 
        }

        private void label3_Click(object sender, EventArgs e)
        {
            ViewDonor viewDonor = new ViewDonor();
            viewDonor.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            ViewPatient viewPatient = new ViewPatient();
            viewPatient.Show();
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
            Transfers transfers = new Transfers();
            transfers.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                if (string.IsNullOrWhiteSpace(PName.Text) ||
                    string.IsNullOrWhiteSpace(PAge.Text) ||
                     string.IsNullOrWhiteSpace(PPhone.Text) ||
                    string.IsNullOrWhiteSpace(PAddress.Text) ||
                    PGender.SelectedIndex == -1 ||
                    PBloodgroup.SelectedIndex == -1)
                {
                    MessageBox.Show("Please fill all fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (SqlConnection conn = Connection.GetInstance().GetConnection())
                    {
                        conn.Open();

                        string query = "INSERT INTO Patient (Pname, Page, Pphone, Paddress, Pgender, PBloodGroup) " +
                                       "VALUES (@Pname, @Page, @Pphone, @Paddress, @Pgender,  @PBloodGroup)";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Pname", PName.Text.Trim());
                            cmd.Parameters.AddWithValue("@Page", int.Parse(PAge.Text.Trim()));
                            cmd.Parameters.AddWithValue("@Pphone", PPhone.Text.Trim());
                            cmd.Parameters.AddWithValue("@Paddress", PAddress.Text.Trim());
                            cmd.Parameters.AddWithValue("@Pgender", PGender.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@PBloodGroup", PBloodgroup.SelectedItem.ToString());
                            
                           

                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Patient added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Clear fields
                    PName.Clear();
                    PAge.Clear();
                    PPhone.Clear();
                    PAddress.Clear();
                    PGender.SelectedIndex = -1;
                    PBloodgroup.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(" Error adding PAtient: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

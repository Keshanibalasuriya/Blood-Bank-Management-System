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
    public partial class Donor : Form
    {
        public Donor()
        {
            InitializeComponent();
        }

        private void Donor_Load(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

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
            Transfers transfers = new Transfers();
            transfers.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void button2_Click(object sender, EventArgs e)

      
        {
            if (string.IsNullOrWhiteSpace(NameTxt.Text) ||
                string.IsNullOrWhiteSpace(AgeTxt.Text) ||
                string.IsNullOrWhiteSpace(PhoneNoTxt.Text) ||
                string.IsNullOrWhiteSpace(AddressTxt.Text) ||
                GenderCmb.SelectedIndex == -1 ||
                BloodGroupCmb.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = Connection.GetInstance().GetConnection())
                {
                    conn.Open();

                    string query = "INSERT INTO Donor (Name, Age, Gender, PhoneNo, Address, BloodGroup) " +
                                   "VALUES (@Name, @Age, @Gender, @PhoneNo, @Address, @BloodGroup)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", NameTxt.Text.Trim());
                        cmd.Parameters.AddWithValue("@Age", int.Parse(AgeTxt.Text.Trim()));
                        cmd.Parameters.AddWithValue("@Gender", GenderCmb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@PhoneNo", PhoneNoTxt.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", AddressTxt.Text.Trim());
                        cmd.Parameters.AddWithValue("@BloodGroup", BloodGroupCmb.SelectedItem.ToString());

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Donor added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Clear fields
                NameTxt.Clear();
                AgeTxt.Clear();
                PhoneNoTxt.Clear();
                AddressTxt.Clear();
                GenderCmb.SelectedIndex = -1;
                BloodGroupCmb.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Error adding donor: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    }

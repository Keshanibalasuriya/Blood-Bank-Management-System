using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Blood_Bank_Managemant_System
{
    public partial class ViewDonor : Form
    {
        public ViewDonor()
        {
            InitializeComponent();
        }

        //  This is the correct event that runs when form opens
        private void ViewDonor_Load_1(object sender, EventArgs e)
        {
            LoadDonors();
        }

        //  Load donor data from database
        private void LoadDonors()
        {
            try
            {
                Connection db = Connection.GetInstance();

                using (SqlConnection conn = db.GetConnection())
                {
                    string query = "SELECT Name, Age, Gender, PhoneNo, Address, BloodGroup FROM Donor";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    guna2DataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to load donor data.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //  Navigation buttons
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
    }
}

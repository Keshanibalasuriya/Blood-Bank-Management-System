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
    public partial class ViewPatient : Form
    {
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
            Transfers transfers = new Transfers();
            transfers.Show();
            this.Hide();
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
                    string query = "SELECT Pname, Page, Pphone, Paddress, Pgender, PBloodGroup FROM Patient";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    ViewpatienGV.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to load Patient data.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
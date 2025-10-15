using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blood_Bank_Managemant_System
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ViewDonor vie = new ViewDonor();
            vie.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();
            ViewDonor vie = new ViewDonor();
            vie.Show();
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
    }
}

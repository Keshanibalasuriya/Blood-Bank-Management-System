using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Blood_Bank_Managemant_System
{
    public partial class Employees : Form
    {
        // ✅ Single shared connection instance
        private readonly Connection connection = Connection.GetInstance();

        // ✅ To store selected UserID for update/delete
        private int selectedUserID = -1;

        public Employees()
        {
            InitializeComponent();
        }

        private void Employees_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        // ✅ Load data into DataGridView
        private void LoadUsers()
        {
            try
            {
                using (SqlConnection conn = connection.GetConnection())
                {
                    string query = "SELECT UserID, Username, Password FROM Users";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    guna2DataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ Failed to load users.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        // ✅ Add Employee
        private void AddEmployee_Click(object sender, EventArgs e)
        {
            string username = Username.Text.Trim();
            string password = Password.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both Username and Password.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = connection.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show(" User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Username.Clear();
                Password.Clear();
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to add user.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Load selected row data into textboxes
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];
                selectedUserID = Convert.ToInt32(row.Cells["UserID"].Value);
                Username.Text = row.Cells["Username"].Value.ToString();
                Password.Text = row.Cells["Password"].Value.ToString();
            }
        }

        // ✅ Update selected user
        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedUserID == -1)
            {
                MessageBox.Show("Please select a user to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newUsername = Username.Text.Trim();
            string newPassword = Password.Text.Trim();

            if (string.IsNullOrEmpty(newUsername) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Please enter Username and Password to update.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = connection.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE Users SET Username=@Username, Password=@Password WHERE UserID=@UserID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", newUsername);
                    cmd.Parameters.AddWithValue("@Password", newPassword);
                    cmd.Parameters.AddWithValue("@UserID", selectedUserID);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Username.Clear();
                Password.Clear();
                selectedUserID = -1;
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update user.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Delete selected user
        private void button3_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                int userId = Convert.ToInt32(guna2DataGridView1.SelectedRows[0].Cells["UserID"].Value);
                string username = guna2DataGridView1.SelectedRows[0].Cells["Username"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete '{username}'?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection con = connection.GetConnection())
                        {
                            con.Open();
                            SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE UserID=@UserID", con);
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show(" User deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Username.Clear();
                        Password.Clear();
                        selectedUserID = -1;
                        LoadUsers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(" Failed to delete user.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                // Store selected UserID
                selectedUserID = Convert.ToInt32(row.Cells["UserID"].Value);

                // Load selected values to textboxes
                Username.Text = row.Cells["Username"].Value.ToString();
                Password.Text = row.Cells["Password"].Value.ToString();
            }
        }
    }
}

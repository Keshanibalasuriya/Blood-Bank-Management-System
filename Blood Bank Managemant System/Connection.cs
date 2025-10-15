using System;
using System.Data.SqlClient;

namespace Blood_Bank_Managemant_System
{
    internal class Connection
    {
        // Step 1: Single instance (Singleton)
        private static Connection _instance;

        // Step 2: Connection string
        private readonly string connString =
            "Data Source=LAPTOP-VTLPEAPH;Initial Catalog=BloodBankDB;Integrated Security=True;TrustServerCertificate=True;";

        // Step 3: Private constructor prevents creating object with 'new'
        private Connection() { }

        // Step 4: Method to get single instance
        public static Connection GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Connection();
            }
            return _instance;
        }

        // Step 5: Method to get a SQL connection
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connString);
        }


        // Step 6: Method to test connection
        public bool TestConnection()
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}

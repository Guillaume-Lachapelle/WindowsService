using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService.Helpers
{
    public class ScriptHandler
    {

        private DataTable dataTable = new DataTable();

        public void ExecuteCreateTableOrDatabaseFile(string connectionString, string filePath)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string data = File.ReadAllText(filePath);

                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = data;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public DataTable ExecuteGetRequestContentFile(string connectionString, string filePath, string ID = "")
        {
            dataTable.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string data = File.ReadAllText(filePath);

                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.CommandText = data;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    // this will query your datatable and return the result to your datatable
                    adapter.Fill(dataTable);
                    adapter.Dispose();
                }
                conn.Close();
            }
            return dataTable;
        }

    }
}

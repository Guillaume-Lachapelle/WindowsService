using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService.Models;

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

        public DataTable ExecuteGetRequestContentFile(string connectionString, string filePath, string ID = "", string Email = "")
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
                    cmd.Parameters.AddWithValue("@Email", Email);
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

        public void ExecuteCreateStudentFile(string connectionString, string filePath, StudentDataModel studentDataModel)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string data = File.ReadAllText(filePath);

                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.Parameters.Clear();
                    // Add non-nullable elements
                    cmd.Parameters.AddWithValue("@ID", studentDataModel.ID);
                    cmd.Parameters.AddWithValue("@FirstName", studentDataModel.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", studentDataModel.LastName);
                    cmd.Parameters.AddWithValue("@Program", studentDataModel.Program);
                    cmd.Parameters.AddWithValue("@SchoolEmail", studentDataModel.SchoolEmail);
                    cmd.Parameters.AddWithValue("@YearOfAdmission", studentDataModel.YearOfAdmission);
                    cmd.Parameters.AddWithValue("@Graduated", studentDataModel.Graduated);
                    // Add nullable elements and check if they are null or empty. If they are, set their value to NULL in database
                    cmd.Parameters.AddWithValue("@Classes", string.IsNullOrEmpty(studentDataModel.Classes) ? SqlString.Null : studentDataModel.Classes);
                    cmd.CommandText = data;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

    }
}

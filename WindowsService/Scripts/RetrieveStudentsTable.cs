using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Data;
using WindowsService.Models;
using System.Collections.Generic;

namespace WindowsService.Scripts
{
    public class RetrieveStudentsTable
    {
        private DataTable dataTable = new DataTable();

        public List<StudentDataModel> RetrieveTableContents()
        {
            var retrievedStudents = new List<StudentDataModel>();

            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePathGetInfoTable = databaseConnection.Connect("GetStudentsContent.sql");
            string server = databaseConnection.server2;

            // Data is stored in dataTable object
            ExecuteGetStudentsContentFile(server, filePathGetInfoTable);
            foreach (DataRow row in dataTable.Rows)
            {
                StudentDataModel studentData = new StudentDataModel()
                {
                    ID = row["ID"].ToString(),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    Program = row["Program"].ToString(),
                    SchoolEmail = row["SchoolEmail"].ToString(),
                    YearOfAdmission = row["YearOfAdmission"].ToString(),
                    Classes = row["Classes"].ToString(),
                    Graduated = row["Graduated"].ToString()
                };
                retrievedStudents.Add(studentData);
            }
            return retrievedStudents;
        }

        private void ExecuteGetStudentsContentFile(string connectionString, string filePath)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string data = File.ReadAllText(filePath);

                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = data;
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    //this will query your datatable and return the result to your datatable
                    adapter.Fill(dataTable);
                    adapter.Dispose();
                }
                conn.Close();
            }
        }

    }
}

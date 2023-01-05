using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Data;
using WindowsService.Models;
using System.Collections.Generic;

namespace WindowsService.Scripts
{
    public class RetrieveTeachersTable
    {
        private DataTable dataTable = new DataTable();

        public List<TeacherDataModel> RetrieveTableContents()
        {
            var retrievedTeachers = new List<TeacherDataModel>();

            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePathGetInfoTable = databaseConnection.Connect("GetTeachersContent.sql");
            string server = databaseConnection.server2;

            // Data is stored in dataTable object
            ExecuteGetTeachersContentFile(server, filePathGetInfoTable);
            foreach (DataRow row in dataTable.Rows)
            {
                TeacherDataModel teacherData = new TeacherDataModel()
                {
                    ID = row["ID"].ToString(),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    Department = row["Department"].ToString(),
                    Salary = row["Salary"].ToString(),
                    HiringDate = row["HiringDate"].ToString(),
                    SchoolEmail = row["SchoolEmail"].ToString(),
                    Classes = row["Classes"].ToString()
                };
                retrievedTeachers.Add(teacherData);
            }
            return retrievedTeachers;
        }

        private void ExecuteGetTeachersContentFile(string connectionString, string filePath)
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

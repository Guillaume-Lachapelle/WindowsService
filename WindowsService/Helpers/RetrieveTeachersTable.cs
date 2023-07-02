using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Data;
using WindowsService.Models;
using System.Collections.Generic;

namespace WindowsService.Helpers
{
    public class RetrieveTeachersTable
    {
        private DataTable dataTable = new DataTable();
        private ScriptHandler scriptHandler = new ScriptHandler();

        public List<TeacherDataModel> RetrieveTableContents()
        {
            List<TeacherDataModel> retrievedTeachers = new List<TeacherDataModel>();

            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePathGetInfoTable = databaseConnection.Connect("GetTeachersContent.sql");
            string server = databaseConnection.server2;

            // Data is stored in dataTable object
            dataTable = scriptHandler.ExecuteGetRequestContentFile(server, filePathGetInfoTable);
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

    }
}

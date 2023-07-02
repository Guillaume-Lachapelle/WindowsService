using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Data;
using WindowsService.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WindowsService.Helpers
{
    public class RetrieveStudents
    {
        private DataTable dataTable = new DataTable();
        private ScriptHandler scriptHandler = new ScriptHandler();

        public List<StudentDataModel> RetrieveTableContents()
        {
            List<StudentDataModel> retrievedStudents = new List<StudentDataModel>();

            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePathGetInfoTable = databaseConnection.Connect("GetStudentsContent.sql");
            string server = databaseConnection.server2;

            // Data is stored in dataTable object
            dataTable = scriptHandler.ExecuteGetRequestContentFile(server, filePathGetInfoTable);
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

        public StudentDataModel FindStudent(string ID)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePathGetInfoTable = databaseConnection.Connect("GetStudent.sql");
            dataTable = scriptHandler.ExecuteGetRequestContentFile(databaseConnection.server2, filePathGetInfoTable, ID);

            if(dataTable.Rows.Count > 0)
            {
                StudentDataModel studentData = new StudentDataModel()
                {
                    ID = dataTable.Rows[0]["ID"].ToString(),
                    FirstName = dataTable.Rows[0]["FirstName"].ToString(),
                    LastName = dataTable.Rows[0]["LastName"].ToString(),
                    Program = dataTable.Rows[0]["Program"].ToString(),
                    SchoolEmail = dataTable.Rows[0]["SchoolEmail"].ToString(),
                    YearOfAdmission = dataTable.Rows[0]["YearOfAdmission"].ToString(),
                    Classes = dataTable.Rows[0]["Classes"].ToString(),
                    Graduated = dataTable.Rows[0]["Graduated"].ToString()
                };
                return studentData;
            }
            else
            {
                return new StudentDataModel();
            }
        }


    }
}

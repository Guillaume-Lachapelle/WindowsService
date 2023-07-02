using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Data;
using WindowsService.Models;
using System.Collections.Generic;

namespace WindowsService.Helpers
{
    public class RetrieveStudentsTable
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
            StudentDataModel studentData = new StudentDataModel();
            List<StudentDataModel> retrievedStudents = RetrieveTableContents();
            foreach (StudentDataModel student in retrievedStudents)
            {
                if (student.ID.Equals(ID))
                {
                    studentData = student;
                }
            }
            return studentData;
        }

    }
}

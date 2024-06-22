﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService.Models;

namespace WindowsService.Helpers
{
    public class RetrieveTable
    {
        private DataTable dataTable = new DataTable();
        private ScriptHandler scriptHandler = new ScriptHandler();

        public List<T> RetrieveTableContents<T>()
        {
            List<T> retrievedData = new List<T>();

            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePathGetInfoTable;
            string server = databaseConnection.server2;
            
            switch (typeof(T))
            {
                case Type studentType when studentType == typeof(StudentDataModel):
                    filePathGetInfoTable = databaseConnection.Connect("GetStudentsContent.sql");
                    break;
                case Type teacherType when teacherType == typeof(TeacherDataModel):
                    filePathGetInfoTable = databaseConnection.Connect("GetTeachersContent.sql");
                    break;
                default:
                    throw new ArgumentException("Invalid type");
            }

            dataTable = scriptHandler.ExecuteGetRequestContentFile(server, filePathGetInfoTable);

            foreach (DataRow row in dataTable.Rows)
            {
                if (typeof(T) == typeof(StudentDataModel))
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
                        Graduated = row["Graduated"].Equals("True")
                    };
                    retrievedData.Add((T)Convert.ChangeType(studentData, typeof(T)));
                }
                else
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
                    retrievedData.Add((T)Convert.ChangeType(teacherData, typeof(T)));
                }
            }
            return retrievedData;
        }
    }
}
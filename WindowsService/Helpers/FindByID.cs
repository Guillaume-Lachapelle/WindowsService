﻿using System;
using System.Data;
using WindowsService.Models;

namespace WindowsService.Helpers
{
    public class FindByID
    {
        private readonly ScriptHandler _scriptHandler = new ScriptHandler();

        public dynamic Find<T>(string id) where T : class, new()
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePathGetInfoTable;

            switch (typeof(T))
            {
                case Type studentType when studentType == typeof(StudentDataModel):
                    filePathGetInfoTable = databaseConnection.Connect("GetStudentByID.sql");
                    break;
                case Type teacherType when teacherType == typeof(TeacherDataModel):
                    filePathGetInfoTable = databaseConnection.Connect("GetTeacherByID.sql");
                    break;
                default:
                    throw new ArgumentException("Invalid type");
            }

            DataTable dataTable = _scriptHandler.ExecuteGetRequestContentFile(databaseConnection.Server2, filePathGetInfoTable, id:id);

            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                if (typeof(T) == typeof(StudentDataModel))
                {
                    return new StudentDataModel
                    {
                        ID = row["ID"].ToString(),
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString(),
                        Program = row["Program"].ToString(),
                        SchoolEmail = row["SchoolEmail"].ToString(),
                        YearOfAdmission = row["YearOfAdmission"].ToString(),
                        Classes = row["Classes"].ToString(),
                        Graduated = Convert.ToBoolean(row["Graduated"])
                    };
                }
                if (typeof(T) == typeof(TeacherDataModel))
                {
                    return new TeacherDataModel
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
                }

            }
            return new T();
        }
    }
}

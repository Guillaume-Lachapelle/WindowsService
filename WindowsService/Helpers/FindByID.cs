using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService.Models;

namespace WindowsService.Helpers
{
    public class FindByID
    {
        private DataTable dataTable = new DataTable();
        private ScriptHandler scriptHandler = new ScriptHandler();

        public dynamic Find<T>(string ID)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePathGetInfoTable;
            bool isStudent = false;

            switch (typeof(T))
            {
                case Type studentType when studentType == typeof(StudentDataModel):
                    filePathGetInfoTable = databaseConnection.Connect("GetStudentByID.sql");
                    isStudent = true;
                    break;
                default:
                    filePathGetInfoTable = databaseConnection.Connect("GetTeacherByID.sql");
                    break;
            }

            dataTable = scriptHandler.ExecuteGetRequestContentFile(databaseConnection.server2, filePathGetInfoTable, ID:ID);

            if (dataTable.Rows.Count > 0)
            {
                if (typeof(T) == typeof(StudentDataModel))
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
                        Graduated = Convert.ToBoolean(dataTable.Rows[0]["Graduated"])
                    };
                    return studentData;
                }
                else
                {
                    TeacherDataModel teacherData = new TeacherDataModel()
                    {
                        ID = dataTable.Rows[0]["ID"].ToString(),
                        FirstName = dataTable.Rows[0]["FirstName"].ToString(),
                        LastName = dataTable.Rows[0]["LastName"].ToString(),
                        Department = dataTable.Rows[0]["Department"].ToString(),
                        Salary = dataTable.Rows[0]["Salary"].ToString(),
                        HiringDate = dataTable.Rows[0]["HiringDate"].ToString(),
                        SchoolEmail = dataTable.Rows[0]["SchoolEmail"].ToString(),
                        Classes = dataTable.Rows[0]["Classes"].ToString()
                    };
                    return teacherData;
                }

            }
            else if (isStudent)
            {
                return new StudentDataModel();
            }
            else
            {
                return new TeacherDataModel();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService.Helpers;
using WindowsService.Models;

namespace WindowsService.Checks
{
    public class CheckEmail
    {
        private FindByEmail findByEmail = new FindByEmail();

        public bool Check(string Email)
        {
            if (Email.Contains("@university.com"))
            {
                DatabaseConnection databaseConnection = new DatabaseConnection();
                string filePathGetInfoStudentsTable = databaseConnection.Connect("GetStudentByEmail.sql");
                string filePathGetInfoTeachersTable = databaseConnection.Connect("GetTeacherByEmail.sql");

                StudentDataModel studentData = findByEmail.Find<StudentDataModel>(Email);
                TeacherDataModel teacherData = findByEmail.Find<TeacherDataModel>(Email);

                if(string.IsNullOrEmpty(studentData.SchoolEmail) && string.IsNullOrEmpty(teacherData.SchoolEmail)){
                    return true;
                }
                
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}

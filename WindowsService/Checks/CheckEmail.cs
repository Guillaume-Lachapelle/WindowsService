using WindowsService.Helpers;
using WindowsService.Models;

namespace WindowsService.Checks
{
    public class CheckEmail
    {
        private readonly FindByEmail _findByEmail = new FindByEmail();

        public bool Check(string email)
        {
            if (email.Contains("@university.com"))
            {
                DatabaseConnection databaseConnection = new DatabaseConnection();
                string filePathGetInfoStudentsTable = databaseConnection.Connect("GetStudentByEmail.sql");
                string filePathGetInfoTeachersTable = databaseConnection.Connect("GetTeacherByEmail.sql");

                // Retrieve student and teacher data by email
                StudentDataModel studentData = _findByEmail.Find<StudentDataModel>(email);
                TeacherDataModel teacherData = _findByEmail.Find<TeacherDataModel>(email);

                // Check if neither student nor teacher with the email exists
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

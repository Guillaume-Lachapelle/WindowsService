using System;
using WindowsService.Models;

namespace WindowsService.Helpers
{
    public class CreationHandler
    {
        private readonly ScriptHandler _scriptHandler = new ScriptHandler();

        public bool Create<T>(T dataModel) where T : class
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePath;

            try
            {
                switch (dataModel)
                {
                    case StudentDataModel studentDataModel:
                        filePath = databaseConnection.Connect("CreateStudent.sql");
                        _scriptHandler.ExecuteCreateOrUpdateStudentFile(databaseConnection.Server2, filePath, studentDataModel, isEdit: false);
                        return true;
                    case TeacherDataModel teacherDataModel:
                        filePath = databaseConnection.Connect("CreateTeacher.sql");
                        _scriptHandler.ExecuteCreateOrUpdateTeacherFile(databaseConnection.Server2, filePath, teacherDataModel, isEdit: false);
                        return true;
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

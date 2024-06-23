using System;
using WindowsService.Models;

namespace WindowsService.Helpers
{
    public class EditHandler
    {
        private readonly ScriptHandler _scriptHandler = new ScriptHandler();

        public bool Edit<T>(string id, T dataModel) where T : class
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePath;

            try
            {
                switch (dataModel)
                {
                    case StudentDataModel studentDataModel:
                        filePath = databaseConnection.Connect("EditStudent.sql");
                        _scriptHandler.ExecuteCreateOrUpdateStudentFile(databaseConnection.Server2, filePath, studentDataModel, isEdit: true);
                        return true;
                    case TeacherDataModel teacherDataModel:
                        filePath = databaseConnection.Connect("EditTeacher.sql");
                        _scriptHandler.ExecuteCreateOrUpdateTeacherFile(databaseConnection.Server2, filePath, teacherDataModel, isEdit: true);
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

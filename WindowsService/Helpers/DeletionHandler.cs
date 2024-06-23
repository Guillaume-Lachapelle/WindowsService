using System;
using WindowsService.Models;

namespace WindowsService.Helpers
{
    public class DeletionHandler
    {
        private readonly ScriptHandler _scriptHandler = new ScriptHandler();

        public bool Delete<T>(string ID) where T : class
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePathGetInfoTable;

            try
            {
                switch (typeof(T))
                {
                    case Type studentType when studentType == typeof(StudentDataModel):
                        filePathGetInfoTable = databaseConnection.Connect("DeleteStudent.sql");
                        _scriptHandler.ExecuteDelete(databaseConnection.Server2, filePathGetInfoTable, ID);
                        return true;
                    case Type studentType when studentType == typeof(TeacherDataModel):
                        filePathGetInfoTable = databaseConnection.Connect("DeleteTeacher.sql");
                        _scriptHandler.ExecuteDelete(databaseConnection.Server2, filePathGetInfoTable, ID);
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

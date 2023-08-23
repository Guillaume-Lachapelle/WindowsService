using System;

namespace WindowsService.Helpers
{
    public class DeleteStudent
    {
        private ScriptHandler scriptHandler = new ScriptHandler();

        public bool Delete(string ID)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePathGetInfoTable = databaseConnection.Connect("DeleteStudent.sql");

            try
            {
                scriptHandler.ExecuteDeleteStudentFile(databaseConnection.server2, filePathGetInfoTable, ID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

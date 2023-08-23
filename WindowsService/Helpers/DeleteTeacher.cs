using System;

namespace WindowsService.Helpers
{
    public class DeleteTeacher
    {
        private ScriptHandler scriptHandler = new ScriptHandler();

        public bool Delete(string ID)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePathGetInfoTable = databaseConnection.Connect("DeleteTeacher.sql");

            try
            {
                scriptHandler.ExecuteDelete(databaseConnection.server2, filePathGetInfoTable, ID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

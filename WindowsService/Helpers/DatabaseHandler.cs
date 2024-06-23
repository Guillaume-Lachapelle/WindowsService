using System.Collections.Generic;

namespace WindowsService.Helpers
{
    public class DatabaseHandler
    {
        private readonly ScriptHandler _scriptHandler = new ScriptHandler();

        //Call CreateDatabaseIfNeeded when DatabaseHandler object is created
        public DatabaseHandler()
        {
            CreateDatabaseIfNeeded();
        }

        private void CreateDatabaseIfNeeded()
        {

            DatabaseConnection databaseConnection = new DatabaseConnection();
            List<string> filePaths = databaseConnection.Connect("CreateDatabase.sql", "CreateTables.sql");
            string filePathCreateDatabase = filePaths[0];
            string filePathCreateTables = filePaths[1];
            string server = databaseConnection.Server;
            string server2 = databaseConnection.Server2;
            //Check if database needs to be created
            _scriptHandler.ExecuteSqlFromFile(server, filePathCreateDatabase);
            //Check if table needs to be created
            _scriptHandler.ExecuteSqlFromFile(server2, filePathCreateTables);

            Logger.MonitoringLogger.Info("Connection to SQL database successful. If SchoolManagement database or " +
                "Teachers table or Students table did not exist, they were created successfully.");
        }
    }
}

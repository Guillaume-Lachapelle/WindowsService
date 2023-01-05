using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Collections.Generic;

namespace WindowsService.Scripts
{
    public class DatabaseHandler
    {

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
            string server = databaseConnection.server;
            string server2 = databaseConnection.server2;
            //Check if database needs to be created
            ExecuteCreateTableOrDatabaseFile(server, filePathCreateDatabase);
            //Check if table needs to be created
            ExecuteCreateTableOrDatabaseFile(server2, filePathCreateTables);
            Logger.MonitoringLogger.Info("Connection to SQL database successful. If SchoolManagement database or " +
                "Teachers table or Students table did not exist, they were created successfully.");
        }

        private void ExecuteCreateTableOrDatabaseFile(string connectionString, string filePath)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string data = File.ReadAllText(filePath);

                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = data;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

    }
}

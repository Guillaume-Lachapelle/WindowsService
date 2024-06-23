using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace WindowsService.Helpers
{
    public class DatabaseConnection
    {

        private readonly string _serverString = ConfigurationManager.AppSettings["SQLServer"];

        // Connection string to server and SchoolManagement database
        public readonly string Server;
        public readonly string Server2;

        public DatabaseConnection()
        {
            Server = $"Data Source={_serverString};Integrated Security=True";
            Server2 = $"Data Source={_serverString};Database=SchoolManagement;Integrated Security=True";
        }

        public string Connect(string scriptFileName)
        {
            string filePathGetInfoTable;

            // Determine script file path based on environment
            if (Directory.Exists(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Scripts"))))
            {
                filePathGetInfoTable = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\..\Scripts\{scriptFileName}"));
            }
            else
            {
                filePathGetInfoTable = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\WindowsService\Scripts\{scriptFileName}"));
            }

            return filePathGetInfoTable;
        }

        public List<string> Connect(string databaseScriptFile, string tableScriptFile)
        {
            List<string> list = new List<string>();
            string filePathDatabase;
            string filePathTable;

            // Determine script file paths based on environment
            if (Directory.Exists(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Scripts"))))
            {
                filePathDatabase = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\..\Scripts\{databaseScriptFile}"));
                filePathTable = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\..\Scripts\{tableScriptFile}"));
            }
            else
            {
                filePathDatabase = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\WindowsService\Scripts\{databaseScriptFile}"));
                filePathTable = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\WindowsService\Scripts\{tableScriptFile}"));
            }

            list.Add(filePathDatabase);
            list.Add(filePathTable);

            return list;
        }

    }
}

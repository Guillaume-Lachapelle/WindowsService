using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService.Helpers
{
    public class DatabaseConnection
    {
        //string to connect to server and inside SchoolManagement database
        static readonly string serverString = ConfigurationManager.AppSettings["SQLServer"];
        public readonly string server = "Data Source=" + serverString + ";Integrated Security=True";
        public readonly string server2 = "Data Source=" + serverString + ";Database=SchoolManagement;Integrated Security=True";

        public string Connect(string scriptFileName)
        {
            string filePathGetInfoTable;
            // Check if the call to the function is made through the service or through the web app by checking if the Scripts folder is a child of the current directory
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
            // Check if the call to the function is made through the service or through the web app by checking if the Scripts folder is a child of the current directory
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

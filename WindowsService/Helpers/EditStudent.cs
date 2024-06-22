using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService.Models;

namespace WindowsService.Helpers
{
    public class EditStudent
    {
        private ScriptHandler scriptHandler = new ScriptHandler();

        public bool Edit(string ID, StudentDataModel studentDataModel)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePathGetInfoTable = databaseConnection.Connect("EditStudent.sql");

            try
            {
                scriptHandler.ExecuteEditStudentFile(databaseConnection.server2, filePathGetInfoTable, studentDataModel);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}

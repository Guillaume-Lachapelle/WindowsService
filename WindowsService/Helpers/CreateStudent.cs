using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService.Models;

namespace WindowsService.Helpers
{
    public class CreateStudent
    {
        private ScriptHandler scriptHandler = new ScriptHandler();

        public bool Create(StudentDataModel studentDataModel)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePathGetInfoTable = databaseConnection.Connect("CreateStudent.sql");

            try
            {
                scriptHandler.ExecuteCreateStudentFile(databaseConnection.server2, filePathGetInfoTable, studentDataModel);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}

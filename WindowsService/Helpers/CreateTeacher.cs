using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService.Models;

namespace WindowsService.Helpers
{
    public class CreateTeacher
    {
        private ScriptHandler scriptHandler = new ScriptHandler();

        public bool Create(TeacherDataModel teacherDataModel)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePathGetInfoTable = databaseConnection.Connect("CreateTeacher.sql");

            try
            {
                scriptHandler.ExecuteCreateTeacherFile(databaseConnection.server2, filePathGetInfoTable, teacherDataModel);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

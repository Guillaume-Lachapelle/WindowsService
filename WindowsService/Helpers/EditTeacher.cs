using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService.Models;

namespace WindowsService.Helpers
{
    public class EditTeacher
    {
        private ScriptHandler scriptHandler = new ScriptHandler();

        public bool Edit(string ID, TeacherDataModel teacherDataModel)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            string filePathGetInfoTable = databaseConnection.Connect("EditTeacher.sql");

            try
            {
                scriptHandler.ExecuteEditTeacherFile(databaseConnection.server2, filePathGetInfoTable, teacherDataModel);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

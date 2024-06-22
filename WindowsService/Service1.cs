using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Timers;
using WindowsService.Models;
using WindowsService.Helpers;

namespace WindowsService
{
    public partial class Service1 : ServiceBase
    {

        #region Fields

        //private int eventId = 1;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors

        public Service1()
        {
            InitializeComponent();
        }

        public Service1(string[] args)
        {
            InitializeComponent();
        }

        #endregion

        #region Event handlers

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {

            // SQL database handling
            DatabaseHandler handler = new DatabaseHandler();

            // SQL retrieve data from Teachers and Students table
/*            RetrieveTable retrieveTable = new RetrieveTable();
            List<TeacherDataModel> teachers = retrieveTable.RetrieveTableContents<TeacherDataModel>();
            List<StudentDataModel> students = retrieveTable.RetrieveTableContents<StudentDataModel>();*/

            /*foreach(var teacher in teachers)
            {
                Logger.MonitoringLogger.Info(teacher.ID + " " + teacher.FirstName + " " + teacher.LastName + " " + teacher.Department);
            }

            foreach(var student in students)
            {
                Logger.MonitoringLogger.Info(student.ID + " " + student.FirstName + " " + student.LastName);
            }*/
        }

        #endregion
    }
}

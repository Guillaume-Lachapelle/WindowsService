using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService.Helpers;
using WindowsService.Models;

namespace WindowsService.Checks
{
    public class CheckID
    {
        private FindByID findByID = new FindByID();

        public bool ValidFormat<T>(string ID)
        {
            bool isStudent = false;

            switch (typeof(T))
            {
                case Type studentType when studentType == typeof(StudentDataModel):
                    isStudent = true;
                    break;
                default:
                    break;
            }

            if (isStudent)
            {
                if (ID.Length != 8 || !int.TryParse(ID, out _))
                {
                    return false;
                }
            }
            else
            {
                if (ID.Length != 9 || !ID.StartsWith("T") || !int.TryParse(ID.Substring(1), out _))
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanCreate<T>(string ID)
        {
            bool isStudent = false;

            switch (typeof(T))
            {
                case Type studentType when studentType == typeof(StudentDataModel):
                    isStudent = true;
                    break;
                default:
                    break;
            }

            if (isStudent)
            {
                StudentDataModel student = findByID.Find<StudentDataModel>(ID);
                if (string.IsNullOrEmpty(student.ID))
                {
                    return true;
                }
            }
            else
            {
                TeacherDataModel teacher = findByID.Find<TeacherDataModel>(ID);
                if (string.IsNullOrEmpty(teacher.ID))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

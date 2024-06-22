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
            switch (typeof(T))
            {
                case Type studentType when studentType == typeof(StudentDataModel):
                    if (ID.Length != 8 || !int.TryParse(ID, out _))
                    {
                        return false;
                    }
                    break;
                case Type teacherType when teacherType == typeof(TeacherDataModel):
                    if (ID.Length != 9 || !ID.StartsWith("T") || !int.TryParse(ID.Substring(1), out _))
                    {
                        return false;
                    }
                    break;
                default:
                    break;
            }

            return true;
        }

        public bool CanCreate<T>(string ID)
        {
            switch (typeof(T))
            {
                case Type studentType when studentType == typeof(StudentDataModel):
                    StudentDataModel student = findByID.Find<StudentDataModel>(ID);
                    if (string.IsNullOrEmpty(student.ID))
                    {
                        return true;
                    }
                    break;
                case Type teacherType when teacherType == typeof(TeacherDataModel):
                    TeacherDataModel teacher = findByID.Find<TeacherDataModel>(ID);
                    if (string.IsNullOrEmpty(teacher.ID))
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }

            return false;
        }
    }
}

using System;
using WindowsService.Helpers;
using WindowsService.Models;

namespace WindowsService.Checks
{
    public class CheckID
    {
        private readonly FindByID _findByID = new FindByID();

        public bool ValidFormat<T>(string id) where T : class
        {
            switch (typeof(T))
            {
                case Type studentType when studentType == typeof(StudentDataModel):
                    if (id.Length != 8 || !int.TryParse(id, out _))
                    {
                        return false;
                    }
                    break;
                case Type teacherType when teacherType == typeof(TeacherDataModel):
                    if (id.Length != 9 || !id.StartsWith("T") || !int.TryParse(id.Substring(1), out _))
                    {
                        return false;
                    }
                    break;
                default:
                    return false; // Handle unexpected type
            }

            return true;
        }

        public bool CanCreate<T>(string id) where T : class
        {
            switch (typeof(T))
            {
                case Type studentType when studentType == typeof(StudentDataModel):
                    StudentDataModel student = _findByID.Find<StudentDataModel>(id);
                    return string.IsNullOrEmpty(student.ID);
                case Type teacherType when teacherType == typeof(TeacherDataModel):
                    TeacherDataModel teacher = _findByID.Find<TeacherDataModel>(id);
                    return string.IsNullOrEmpty(teacher.ID);
                default:
                    return false; // Handle unexpected type
            }
        }
    }
}

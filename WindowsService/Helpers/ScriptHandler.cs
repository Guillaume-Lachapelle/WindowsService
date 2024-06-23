using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using WindowsService.Models;

namespace WindowsService.Helpers
{
    public class ScriptHandler
    {

        private DataTable _dataTable = new DataTable();

        public void ExecuteSqlFromFile(string connectionString, string filePath, Action<SqlCommand> parameterSetter = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = File.ReadAllText(filePath);
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    parameterSetter?.Invoke(cmd);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable ExecuteGetRequestContentFile(string connectionString, string filePath, string id = "", string email = "")
        {
            _dataTable.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = File.ReadAllText(filePath);
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(_dataTable);
                    }
                }
            }
            return _dataTable;
        }

        public void ExecuteCreateOrUpdateStudentFile(string connectionString, string filePath, StudentDataModel studentDataModel, bool isEdit = false)
        {
            ExecuteSqlFromFile(connectionString, filePath, cmd =>
            {
                cmd.Parameters.AddWithValue("@ID", studentDataModel.ID);
                cmd.Parameters.AddWithValue("@FirstName", studentDataModel.FirstName);
                cmd.Parameters.AddWithValue("@LastName", studentDataModel.LastName);
                cmd.Parameters.AddWithValue("@Program", studentDataModel.Program);
                cmd.Parameters.AddWithValue("@SchoolEmail", studentDataModel.SchoolEmail);
                cmd.Parameters.AddWithValue("@YearOfAdmission", studentDataModel.YearOfAdmission);
                cmd.Parameters.AddWithValue("@Graduated", studentDataModel.Graduated);
                cmd.Parameters.AddWithValue("@Classes", string.IsNullOrEmpty(studentDataModel.Classes) ? SqlString.Null : studentDataModel.Classes);
            });
        }

        public void ExecuteCreateOrUpdateTeacherFile(string connectionString, string filePath, TeacherDataModel teacherDataModel, bool isEdit = false)
        {
            ExecuteSqlFromFile(connectionString, filePath, cmd =>
            {
                cmd.Parameters.AddWithValue("@ID", teacherDataModel.ID);
                cmd.Parameters.AddWithValue("@FirstName", teacherDataModel.FirstName);
                cmd.Parameters.AddWithValue("@LastName", teacherDataModel.LastName);
                cmd.Parameters.AddWithValue("@Salary", teacherDataModel.Salary);
                cmd.Parameters.AddWithValue("@HiringDate", teacherDataModel.HiringDate);
                cmd.Parameters.AddWithValue("@SchoolEmail", teacherDataModel.SchoolEmail);
                cmd.Parameters.AddWithValue("@Classes", string.IsNullOrEmpty(teacherDataModel.Classes) ? SqlString.Null : teacherDataModel.Classes);
                cmd.Parameters.AddWithValue("@Department", string.IsNullOrEmpty(teacherDataModel.Department) ? SqlString.Null : teacherDataModel.Department);
            });
        }

        public void ExecuteDelete(string connectionString, string filePath, string id)
        {
            ExecuteSqlFromFile(connectionString, filePath, cmd =>
            {
                cmd.Parameters.AddWithValue("@ID", id);
            });
        }
    }
}

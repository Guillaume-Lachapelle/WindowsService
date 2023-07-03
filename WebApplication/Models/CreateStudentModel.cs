using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class CreateStudentModel
    {
        public string Program { get; set; }
        public string SchoolEmail { get; set; }
        public string YearOfAdmission { get; set; }
        public string Classes { get; set; }
    }
}
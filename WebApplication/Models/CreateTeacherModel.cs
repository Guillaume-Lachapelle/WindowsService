﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class CreateTeacherModel
    {
        public string Department { get; set; }
        public string Salary { get; set; }
        public string HiringDate { get; set; }
        public string SchoolEmail { get; set; }
        public string Classes { get; set; }
    }
}
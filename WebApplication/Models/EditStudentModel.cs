namespace WebApplication.Models
{
    public class EditStudentModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Program { get; set; }
        public string SchoolEmail { get; set; }
        public string YearOfAdmission { get; set; }
        public string Classes { get; set; }
        public bool Graduated { get; set; }
    }
}
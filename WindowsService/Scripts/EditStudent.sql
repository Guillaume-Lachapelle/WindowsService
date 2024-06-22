UPDATE Students
SET FirstName = @FirstName, LastName = @LastName, Program = @Program, SchoolEmail = @SchoolEmail, YearOfAdmission = @YearOfAdmission, Classes = @Classes, Graduated = @Graduated
WHERE ID = @ID;
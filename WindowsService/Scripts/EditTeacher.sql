UPDATE Teachers
SET FirstName = @FirstName, LastName = @LastName, SchoolEmail = @SchoolEmail, Classes = @Classes,
    Salary = @Salary, HiringDate = @HiringDate, Department = @Department
WHERE ID = @ID;

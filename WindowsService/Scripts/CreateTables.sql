-- Create table Teachers if it does not exist
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Teachers' AND xtype='U') CREATE TABLE Teachers(ID nchar(9) not null,FirstName nvarchar(max) not null, LastName nvarchar(max) not null,
							Department nchar(4) null,Salary int not null, HiringDate datetime not null, SchoolEmail nvarchar(max) not null, Classes nvarchar(max) null);

-- Create table Students if it does not exist
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Students' AND xtype='U') CREATE TABLE Students(ID nchar(8) not null,FirstName nvarchar(max) not null, LastName nvarchar(max) not null,
							Program nchar(4) not null, SchoolEmail nvarchar(max) not null, YearOfAdmission date not null, Classes nvarchar(max) null, Graduated bit not null);
IF NOT EXISTS 
   (SELECT name FROM master.dbo.sysdatabases WHERE name = 'SchoolManagement') CREATE DATABASE [SchoolManagement];
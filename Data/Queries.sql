-- Create Database
CREATE DATABASE ToDoList;

-- Use Database
USE ToDoList;
GO

-- Do Migrations Using Commands:
-- Add-Migration InitialCreate
-- Update-Database

-- Update Table Tasks
ALTER TABLE Tasks
ALTER COLUMN Description VARCHAR(max) NULL
ALTER COLUMN StimatedDate DATETIME2 NULL
ALTER COLUMN StartingDate DATETIME2 NULL
ALTER COLUMN CompletionDate DATETIME2 NULL;
GO

ALTER TABLE Tasks
ADD CONSTRAINT DF_Tasks_CreationDate DEFAULT GETDATE() FOR CreationDate;

-- Insert One User
INSERT INTO Users VALUES(
    'jairoalperez', 
    'Jairo', 
    'Perez', 
    '2001-08-14', 
    'asd', 
    'jairo@email.com', 
    '1234567890');

-- Insert Two Tasks
INSERT INTO Tasks VALUES(
	1, 
    'Learn C#', 
    'I have to learn this as this will open to much more opportunities on development as it is one of the most used languages as for backend dev, software dev and game dev', 
    GETDATE(),
	'2024-12-31', 
    '2024-11-18', 
    null,
	0,
    1,
    1);
GO

INSERT INTO Tasks VALUES(
    1, 
    'Learn ASP.Net Core', 
    'ASP.Net Core is the C# Framework most used for backend development, this will open me a lot of opportunities', 
	GETDATE(),
    '2025-01-31', 
    '2024-11-18', 
	null,
    0,
    1,
    2);

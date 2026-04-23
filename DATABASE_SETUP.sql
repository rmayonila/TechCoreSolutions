-- Create Database
CREATE DATABASE TechCoreSolutionsDB;
GO

USE TechCoreSolutionsDB;
GO

-- Create Employees Table
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Position NVARCHAR(100) NOT NULL,
    Department NVARCHAR(100) NOT NULL,
    DailyRate DECIMAL(18,2) NOT NULL
);

-- Create Payrolls Table
CREATE TABLE Payrolls (
    PayrollID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT NOT NULL,
    Date DATETIME2 NOT NULL,
    DaysWorked INT NOT NULL,
    Deduction DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_Payrolls_Employees FOREIGN KEY (EmployeeID) 
        REFERENCES Employees(EmployeeID) ON DELETE CASCADE
);

-- Create Index for Foreign Key
CREATE INDEX IX_Payrolls_EmployeeID ON Payrolls(EmployeeID);


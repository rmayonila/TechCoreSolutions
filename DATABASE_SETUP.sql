-- TechCore Solutions Database Setup Script
-- Create this database for BOTH SQL Server and MySQL (with syntax adjustments)

-- ============================================
-- SQL SERVER / SQL SERVER EXPRESS / LOCALDB
-- ============================================

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

-- ============================================
-- MYSQL / XAMPP (Use this syntax for MySQL)
-- ============================================

-- Create Database
-- CREATE DATABASE IF NOT EXISTS TechCoreSolutionsDB;
-- USE TechCoreSolutionsDB;
--
-- -- Create Employees Table
-- CREATE TABLE Employees (
--     EmployeeID INT PRIMARY KEY AUTO_INCREMENT,
--     FirstName VARCHAR(50) NOT NULL,
--     LastName VARCHAR(50) NOT NULL,
--     Position VARCHAR(100) NOT NULL,
--     Department VARCHAR(100) NOT NULL,
--     DailyRate DECIMAL(18,2) NOT NULL
-- );
--
-- -- Create Payrolls Table
-- CREATE TABLE Payrolls (
--     PayrollID INT PRIMARY KEY AUTO_INCREMENT,
--     EmployeeID INT NOT NULL,
--     Date DATETIME NOT NULL,
--     DaysWorked INT NOT NULL,
--     Deduction DECIMAL(18,2) NOT NULL,
--     CONSTRAINT FK_Payrolls_Employees FOREIGN KEY (EmployeeID) 
--         REFERENCES Employees(EmployeeID) ON DELETE CASCADE
-- );
--
-- -- Create Index for Foreign Key
-- CREATE INDEX IX_Payrolls_EmployeeID ON Payrolls(EmployeeID);

-- ============================================
-- Sample Data (Optional - for testing)
-- ============================================

-- INSERT INTO Employees (FirstName, LastName, Position, Department, DailyRate)
-- VALUES 
--     ('John', 'Doe', 'Senior Developer', 'IT', 1500.00),
--     ('Jane', 'Smith', 'Project Manager', 'Management', 1800.00),
--     ('Robert', 'Johnson', 'Junior Developer', 'IT', 1000.00),
--     ('Maria', 'Garcia', 'HR Officer', 'Human Resources', 1200.00),
--     ('David', 'Williams', 'System Administrator', 'IT', 1400.00);
--
-- INSERT INTO Payrolls (EmployeeID, Date, DaysWorked, Deduction)
-- VALUES 
--     (1, '2026-04-15', 20, 2000.00),
--     (2, '2026-04-15', 22, 2500.00),
--     (1, '2026-04-30', 21, 2100.00),
--     (3, '2026-04-15', 18, 1500.00),
--     (4, '2026-04-15', 20, 1800.00);

-- ============================================
-- Verification Queries
-- ============================================

-- Check Employees
-- SELECT * FROM Employees;
--
-- -- Check Payrolls
-- SELECT * FROM Payrolls;
--
-- -- View with computed values
-- SELECT 
--     p.PayrollID,
--     e.FirstName + ' ' + e.LastName AS EmployeeName,
--     p.Date,
--     p.DaysWorked,
--     (p.DaysWorked * e.DailyRate) AS GrossPay,
--     p.Deduction,
--     (p.DaysWorked * e.DailyRate - p.Deduction) AS NetPay
-- FROM Payrolls p
-- INNER JOIN Employees e ON p.EmployeeID = e.EmployeeID
-- ORDER BY p.Date DESC;

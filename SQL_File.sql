-- Create the database
CREATE DATABASE BloodBankDB;
GO

-- Use the database
USE BloodBankDB;
GO

-- Create Users table
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(50) NOT NULL
);
GO

-- Create Donor table
CREATE TABLE Donor (
    DonorID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100),
    Age INT,
    Gender VARCHAR(10),
    PhoneNo VARCHAR(15),
    Address VARCHAR(200),
    BloodGroup VARCHAR(10)
);
GO

-- Insert sample data into Users
INSERT INTO Users (Username, Password) VALUES
('admin', 'admin123'),
('doctor', 'doc2025'),
('nurse', 'nursepass');
GO

-- Insert sample data into Donor
INSERT INTO Donor (Name, Age, Gender, PhoneNo, Address, BloodGroup) VALUES
('Nimal Perera', 29, 'Male', '0771234567', 'Colombo', 'A+'),
('Kavindu Silva', 35, 'Male', '0712345678', 'Galle', 'B+'),
('Sanduni Fernando', 27, 'Female', '0759876543', 'Kandy', 'O-');
GO

-- Show all Users
SELECT * FROM Users;
GO

-- Show all Donors
SELECT * FROM Donor;
GO

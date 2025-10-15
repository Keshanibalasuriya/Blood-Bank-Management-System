-- 🩸 Create the Blood Bank Database
CREATE DATABASE BloodBankDB;
GO

-- 🩸 Use the newly created database
USE BloodBankDB;
GO

-- 🧑‍💻 Create Users table
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(50) NOT NULL
);
GO

-- 🩸 Create Donor table
CREATE TABLE Donor (
    DonorID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Age INT CHECK (Age > 0),
    Gender VARCHAR(10) CHECK (Gender IN ('Male', 'Female', 'Other')),
    PhoneNo VARCHAR(15),
    Address VARCHAR(200),
    BloodGroup VARCHAR(10)
);
GO

-- 🧑‍⚕️ Create Patient table
CREATE TABLE Patient (
    PatientID INT IDENTITY(1,1) PRIMARY KEY,
    Pname VARCHAR(100) NOT NULL,
    Page INT CHECK (Page > 0),
    Pphone VARCHAR(15),
    Pgender VARCHAR(10) CHECK (Pgender IN ('Male', 'Female', 'Other')),
    PBloodGroup VARCHAR(5),
    Paddress VARCHAR(200)
);
GO

-- 👤 Insert sample data into Users
INSERT INTO Users (Username, Password) VALUES
('admin', 'admin123'),
('doctor', 'doc2025'),
('nurse', 'nursepass');
GO

-- 🩸 Insert sample data into Donor
INSERT INTO Donor (Name, Age, Gender, PhoneNo, Address, BloodGroup) VALUES
('Nimal Perera', 29, 'Male', '0771234567', 'Colombo', 'A+'),
('Kavindu Silva', 35, 'Male', '0712345678', 'Galle', 'B+'),
('Sanduni Fernando', 27, 'Female', '0759876543', 'Kandy', 'O-');
GO

-- 🏥 Insert sample data into Patient
INSERT INTO Patient (Pname, Page, Pphone, Pgender, PBloodGroup, Paddress)
VALUES
('Kasun Perera', 28, '0773214567', 'Male', 'A+', 'Galle'),
('Nimali Silva', 35, '0719876543', 'Female', 'O-', 'Matara'),
('Sajith Fernando', 42, '0754567890', 'Male', 'B+', 'Colombo'),
('Kamalini Jayasuriya', 30, '0761237894', 'Female', 'AB+', 'Kandy'),
('Ruwan Hettiarachchi', 25, '0706543210', 'Male', 'O+', 'Kurunegala');
GO

-- ✅ Show all tables data
SELECT * FROM Users;
SELECT * FROM Donor;
SELECT * FROM Patient;
GO

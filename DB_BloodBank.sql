-- ========================================================
-- DATABASE: Blood Bank Management System
-- ========================================================

-- Create the database
CREATE DATABASE DB_BloodBank;
GO

-- Use the new database
USE DB_BloodBank;
GO

-- ========================================================
-- USERS TABLE
-- ========================================================
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(50) NOT NULL
);
GO

-- ========================================================
-- DONOR TABLE
-- ========================================================
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

-- ========================================================
-- PATIENT TABLE
-- ========================================================
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

-- ========================================================
-- BLOOD STOCK TABLE
-- ========================================================
CREATE TABLE BloodStock (
    BloodGroup VARCHAR(5) NOT NULL PRIMARY KEY,
    BStock INT NOT NULL CHECK (BStock >= 0)
);
GO

-- ========================================================
-- TRANSFERS TABLE
-- ========================================================
CREATE TABLE Transfers (
    TransferID INT IDENTITY(1,1) PRIMARY KEY,
    PatientID INT NOT NULL,
    PatientName NVARCHAR(100),
    BloodGroup NVARCHAR(10),
    TransferDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (PatientID) REFERENCES Patient(PatientID)
);
GO

-- ========================================================
-- DONATIONS TABLE
-- ========================================================
CREATE TABLE Donations (
    DonationID INT IDENTITY(1,1) PRIMARY KEY,
    DonorID INT NOT NULL,
    DonorName NVARCHAR(100),
    BloodGroup NVARCHAR(10),
    DonatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (DonorID) REFERENCES Donor(DonorID)
);
GO


-- ========================================================
-- INSERT SAMPLE DATA
-- ========================================================

-- USERS
INSERT INTO Users (Username, Password) VALUES
('admin', 'admin123'),
('doctor1', 'docpass'),
('staff1', 'staffpass');
GO

-- DONORS
INSERT INTO Donor (Name, Age, Gender, PhoneNo, Address, BloodGroup) VALUES
('Nimal Perera', 29, 'Male', '0771234567', 'Colombo', 'O+'),
('Kavindi Silva', 25, 'Female', '0719876543', 'Galle', 'A+'),
('Sahan Fernando', 34, 'Male', '0755554321', 'Kandy', 'B+'),
('Anusha Wijesinghe', 28, 'Female', '0768765432', 'Matara', 'AB+'),
('Ruwan Jayasuriya', 40, 'Male', '0701239876', 'Kurunegala', 'O-');
GO

-- PATIENTS
INSERT INTO Patient (Pname, Page, Pphone, Pgender, PBloodGroup, Paddress) VALUES
('Kasun Bandara', 45, '0771112223', 'Male', 'O+', 'Colombo'),
('Dinithi Jayawardena', 32, '0783334445', 'Female', 'A+', 'Galle'),
('Pradeep Kumara', 50, '0715556667', 'Male', 'B+', 'Kandy'),
('Iresha Madushani', 27, '0757778889', 'Female', 'AB+', 'Matara'),
('Tharindu Senanayake', 38, '0752358889', 'Male', 'O-', 'Negombo');
GO

-- BLOOD STOCK (based on real-world distribution)
INSERT INTO BloodStock (BloodGroup, BStock) VALUES
('O+', 90),
('A+', 85),
('B+', 50),
('AB+', 25),
('O-', 20),
('A-', 15),
('B-', 10),
('AB-', 5);
GO

-- TRANSFERS (most recent at top when sorted DESC)
INSERT INTO Transfers (PatientID, PatientName, BloodGroup, TransferDate) VALUES
(3, 'Pradeep Kumara', 'B+', GETDATE()),
(2, 'Dinithi Jayawardena', 'A+', GETDATE()),
(1, 'Kasun Bandara', 'O+', GETDATE());
GO

-- DONATIONS
INSERT INTO Donations (DonorID, DonorName, BloodGroup, DonatedDate) VALUES
(1, 'Nimal Perera', 'O+', GETDATE()),
(2, 'Kavindi Silva', 'A+', GETDATE()),
(3, 'Sahan Fernando', 'B+', GETDATE()),
(4, 'Anusha Wijesinghe', 'AB+', GETDATE()),
(5, 'Ruwan Jayasuriya', 'O-', GETDATE());
GO


-- ========================================================
-- VIEW TABLE DATA
-- ========================================================
USE DB_BloodBank;

SELECT * FROM Users;
SELECT * FROM Donor;
SELECT * FROM Patient;
SELECT * FROM BloodStock;
SELECT * FROM Transfers ORDER BY TransferID DESC; -- show most recent transfers first
SELECT * FROM Donations;
GO

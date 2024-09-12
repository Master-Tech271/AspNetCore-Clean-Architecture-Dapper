


select * from Products


CREATE table Products
(
	Id int Identity(1, 1) not null primary key,
	Name VARCHAR(255),
	Price decimal(10, 2),
	Stock int,
	CreatedAt DateTime,
	UpdatedAt DateTime
);

/*
CREATE TABLE Logs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    LogDate DATETIME NOT NULL,
    Level NVARCHAR(50) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    Exception NVARCHAR(MAX) NULL
);

select * from Logs
*/


CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL UNIQUE,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE UserRoles (
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE CASCADE
);



-- Insert roles
INSERT INTO Roles (RoleName) VALUES ('User');
INSERT INTO Roles (RoleName) VALUES ('SystemAdmin');

-- Insert a user (make sure to hash the password properly in your application)
INSERT INTO Users (Username, PasswordHash, Email) VALUES ('testuser', '$2a$11$DYvlbJeXLpJbq1Eht7N.7Oo0bwYEN2CyqgDMWRQdAvCF/qx0NGU1i', 'test@example.com');
INSERT INTO Users (Username, PasswordHash, Email) VALUES ('adminuser', '$2a$11$DYvlbJeXLpJbq1Eht7N.7Oo0bwYEN2CyqgDMWRQdAvCF/qx0NGU1i', 'admin@example.com');

-- Assign roles to users
INSERT INTO UserRoles (UserId, RoleId) VALUES (1, 1); -- Assign User role to testuser
INSERT INTO UserRoles (UserId, RoleId) VALUES (2, 2); -- Assign SystemAdmin role to adminuser





SELECT Username, PasswordHash, Email, Id, CreatedAt, UpdatedAt FROM Users 
CREATE DATABASE TaskManagerDb;
GO
USE TaskManagerDb;
CREATE TABLE [TodoItem] (
    Id INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    DueDate DATETIME,
    IsCompleted BIT,
    UserId INT
);
CREATE TABLE [User] (
    Id INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(100) NOT NULL,
    Password NVARCHAR(255) NOT NULL
);
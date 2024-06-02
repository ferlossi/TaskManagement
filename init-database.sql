CREATE DATABASE TaskManagerDb;
GO
USE TaskManagerDb;
CREATE TABLE Task (
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

--volumes:
--- ./init-database.sql:/init-database.sql
--command: /bin/bash -c "(/opt/mssql/bin/sqlservr &) && sleep 40s && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'SaLsita8214' -d master -i /init-database.sql"
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Assets] (
    [AssetId] int NOT NULL IDENTITY,
    [AssetName] nvarchar(100) NOT NULL,
    [AssetType] nvarchar(max) NOT NULL,
    [MakeModel] nvarchar(100) NOT NULL,
    [SerialNumber] nvarchar(50) NOT NULL,
    [PurchaseDate] datetime2 NOT NULL,
    [WarrantyExpiryDate] datetime2 NOT NULL,
    [Condition] nvarchar(max) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [IsSpare] bit NOT NULL,
    [Specifications] nvarchar(500) NOT NULL,
    CONSTRAINT [PK_Assets] PRIMARY KEY ([AssetId])
);

CREATE TABLE [Employees] (
    [EmployeeId] int NOT NULL IDENTITY,
    [FullName] nvarchar(100) NOT NULL,
    [Department] nvarchar(50) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    [Designation] nvarchar(50) NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([EmployeeId])
);

CREATE TABLE [AssetAssignmentHistories] (
    [Id] int NOT NULL IDENTITY,
    [AssetId] int NOT NULL,
    [EmployeeId] int NOT NULL,
    [AssignedDate] datetime2 NOT NULL,
    [ReturnedDate] datetime2 NULL,
    [Notes] nvarchar(250) NOT NULL,
    CONSTRAINT [PK_AssetAssignmentHistories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AssetAssignmentHistories_Assets_AssetId] FOREIGN KEY ([AssetId]) REFERENCES [Assets] ([AssetId]) ON DELETE CASCADE,
    CONSTRAINT [FK_AssetAssignmentHistories_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([EmployeeId]) ON DELETE CASCADE
);

CREATE INDEX [IX_AssetAssignmentHistories_AssetId] ON [AssetAssignmentHistories] ([AssetId]);

CREATE INDEX [IX_AssetAssignmentHistories_EmployeeId] ON [AssetAssignmentHistories] ([EmployeeId]);

CREATE UNIQUE INDEX [IX_Assets_SerialNumber] ON [Assets] ([SerialNumber]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251008084532_InitialCreate', N'9.0.9');

COMMIT;
GO


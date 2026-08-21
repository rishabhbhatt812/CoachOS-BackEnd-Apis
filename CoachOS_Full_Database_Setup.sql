-- ====================================================================================================
-- CoachOS Complete Database Recreation Script
-- DBMS: Microsoft SQL Server (MSSQL)
-- Database Name: CoachOSDb
-- Description: Creates the complete schema for CoachOS including all tables, foreign keys,
--              unique constraints, indexes, EF migrations history, and default seed data.
-- ====================================================================================================

-- 1. Create Database if it does not exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CoachOSDb')
BEGIN
    CREATE DATABASE [CoachOSDb];
END
GO

USE [CoachOSDb];
GO

-- 2. Entity Framework Migrations History Table
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END
GO

-- ====================================================================================================
-- 3. CORE & TENANCY TABLES
-- ====================================================================================================

-- Organizations (Institutes) Table
IF OBJECT_ID(N'[Organizations]') IS NULL
BEGIN
    CREATE TABLE [Organizations] (
        [OrganizationId] uniqueidentifier NOT NULL,
        [InstituteCode] nvarchar(50) NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [ShortName] nvarchar(50) NULL,
        [LogoPath] nvarchar(500) NULL,
        [Description] nvarchar(1000) NULL,
        [ContactPersonName] nvarchar(150) NOT NULL,
        [MobileNumber] nvarchar(20) NOT NULL,
        [AlternateMobileNumber] nvarchar(20) NULL,
        [EmailAddress] nvarchar(150) NOT NULL,
        [WebsiteUrl] nvarchar(250) NULL,
        [AddressLine1] nvarchar(250) NOT NULL,
        [AddressLine2] nvarchar(250) NULL,
        [City] nvarchar(100) NOT NULL,
        [State] nvarchar(100) NOT NULL,
        [Country] nvarchar(100) NOT NULL,
        [Pincode] nvarchar(20) NOT NULL,
        [InstituteType] nvarchar(100) NOT NULL,
        [EstablishedYear] int NULL,
        [AcademicSessionStartMonth] nvarchar(50) NOT NULL,
        [AcademicSessionEndMonth] nvarchar(50) NOT NULL,
        [OwnerName] nvarchar(150) NOT NULL,
        [OwnerMobile] nvarchar(20) NOT NULL,
        [OwnerEmail] nvarchar(150) NULL,
        [AadhaarNumber] nvarchar(50) NULL,
        [PANNumber] nvarchar(50) NULL,
        [GSTNumber] nvarchar(50) NULL,
        [PlanName] nvarchar(100) NOT NULL,
        [MaxStudentsAllowed] int NOT NULL,
        [MaxTeachersAllowed] int NOT NULL,
        [ExpiryDate] datetime2 NOT NULL,
        [IsTrial] bit NOT NULL,
        [SMSEnabled] bit NOT NULL,
        [EmailEnabled] bit NOT NULL,
        [WhatsAppEnabled] bit NOT NULL,
        [Currency] nvarchar(10) NOT NULL DEFAULT (N'INR'),
        [ReceiptPrefix] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_Organizations] PRIMARY KEY ([OrganizationId])
    );
END
GO

-- Institutes Compatibility View
IF OBJECT_ID(N'[Institutes]', 'V') IS NOT NULL DROP VIEW [Institutes];
GO
CREATE VIEW [Institutes] AS 
SELECT 
    [OrganizationId] AS [Id], 
    [OrganizationId], 
    [InstituteCode], 
    [Name] AS [InstituteName], 
    [Name], 
    [ShortName], 
    [LogoPath], 
    [Description], 
    [ContactPersonName], 
    [MobileNumber], 
    [AlternateMobileNumber], 
    [EmailAddress], 
    [WebsiteUrl], 
    [AddressLine1], 
    [AddressLine2], 
    [City], 
    [State], 
    [Country], 
    [Pincode], 
    [InstituteType], 
    [EstablishedYear], 
    [AcademicSessionStartMonth], 
    [AcademicSessionEndMonth], 
    [OwnerName], 
    [OwnerMobile], 
    [OwnerEmail], 
    [AadhaarNumber], 
    [PANNumber], 
    [GSTNumber], 
    [PlanName], 
    [MaxStudentsAllowed], 
    [MaxTeachersAllowed], 
    [ExpiryDate], 
    [IsTrial], 
    [SMSEnabled], 
    [EmailEnabled], 
    [WhatsAppEnabled], 
    [Currency], 
    [ReceiptPrefix], 
    [IsActive], 
    [CreatedAt], 
    [CreatedBy], 
    [UpdatedAt], 
    [UpdatedBy], 
    [IsDeleted], 
    [DeletedAt], 
    [DeletedBy] 
FROM [Organizations];
GO

-- Branches Table
IF OBJECT_ID(N'[Branches]') IS NULL
BEGIN
    CREATE TABLE [Branches] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [Code] nvarchar(50) NOT NULL,
        [Address] nvarchar(500) NULL,
        [ContactNumber] nvarchar(50) NULL,
        [Email] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Branches] PRIMARY KEY ([Id])
    );
    CREATE INDEX [IX_Branches_InstituteId] ON [Branches] ([InstituteId]);
END
GO

-- Roles Table
IF OBJECT_ID(N'[Roles]') IS NULL
BEGIN
    CREATE TABLE [Roles] (
        [Id] uniqueidentifier NOT NULL,
        [RoleName] nvarchar(100) NOT NULL,
        [Code] nvarchar(50) NOT NULL,
        [Description] nvarchar(500) NULL,
        [IsSystemRole] bit NOT NULL CONSTRAINT [DF_Roles_IsSystemRole] DEFAULT (1),
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
    CREATE UNIQUE INDEX [IX_Roles_Code] ON [Roles] ([Code]);
END
GO

-- Permissions Table
IF OBJECT_ID(N'[Permissions]') IS NULL
BEGIN
    CREATE TABLE [Permissions] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [Code] nvarchar(50) NOT NULL,
        [Description] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_Permissions] PRIMARY KEY ([Id])
    );
    CREATE UNIQUE INDEX [IX_Permissions_Code] ON [Permissions] ([Code]);
END
GO

-- RolePermissions Mapping Table
IF OBJECT_ID(N'[RolePermissions]') IS NULL
BEGIN
    CREATE TABLE [RolePermissions] (
        [Id] uniqueidentifier NOT NULL,
        [RoleId] uniqueidentifier NOT NULL,
        [PermissionId] uniqueidentifier NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RolePermissions_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RolePermissions_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_RolePermissions_PermissionId] ON [RolePermissions] ([PermissionId]);
    CREATE INDEX [IX_RolePermissions_RoleId] ON [RolePermissions] ([RoleId]);
END
GO

-- Modules Table
IF OBJECT_ID(N'[Modules]') IS NULL
BEGIN
    CREATE TABLE [Modules] (
        [Id] uniqueidentifier NOT NULL,
        [ModuleName] nvarchar(100) NOT NULL,
        [ModuleCode] nvarchar(50) NOT NULL,
        [Description] nvarchar(max) NULL,
        [DisplayOrder] int NOT NULL DEFAULT 0,
        [Icon] nvarchar(max) NULL,
        [IsDefaultEnabled] bit NOT NULL DEFAULT 0,
        [IsMenuItem] bit NOT NULL DEFAULT 0,
        [ParentModuleId] uniqueidentifier NULL,
        [RoutePath] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_Modules] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Modules_Modules_ParentModuleId] FOREIGN KEY ([ParentModuleId]) REFERENCES [Modules] ([Id]) ON DELETE NO ACTION
    );
    CREATE UNIQUE INDEX [IX_Modules_ModuleCode] ON [Modules] ([ModuleCode]);
    CREATE INDEX [IX_Modules_ParentModuleId] ON [Modules] ([ParentModuleId]);
END
GO

-- OrganizationModules Mapping Table
IF OBJECT_ID(N'[OrganizationModules]') IS NULL
BEGIN
    CREATE TABLE [OrganizationModules] (
        [Id] uniqueidentifier NOT NULL,
        [OrganizationId] uniqueidentifier NOT NULL,
        [ModuleId] uniqueidentifier NOT NULL,
        [IsEnabled] bit NOT NULL,
        [EnabledBy] uniqueidentifier NULL,
        [EnabledOn] datetime2 NULL,
        [DisabledBy] uniqueidentifier NULL,
        [DisabledOn] datetime2 NULL,
        [Remarks] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_OrganizationModules] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrganizationModules_Modules_ModuleId] FOREIGN KEY ([ModuleId]) REFERENCES [Modules] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_OrganizationModules_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([OrganizationId]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_OrganizationModules_ModuleId] ON [OrganizationModules] ([ModuleId]);
    CREATE UNIQUE INDEX [IX_OrganizationModules_OrganizationId_ModuleId] ON [OrganizationModules] ([OrganizationId], [ModuleId]);
END
GO

-- Users Table
IF OBJECT_ID(N'[Users]') IS NULL
BEGIN
    CREATE TABLE [Users] (
        [Id] uniqueidentifier NOT NULL,
        [RoleId] uniqueidentifier NOT NULL,
        [FullName] nvarchar(150) NOT NULL,
        [Email] nvarchar(150) NOT NULL,
        [MobileNumber] nvarchar(20) NULL,
        [Username] nvarchar(150) NOT NULL,
        [PasswordHash] nvarchar(max) NOT NULL,
        [PasswordSalt] nvarchar(500) NULL,
        [BranchId] uniqueidentifier NULL,
        [ProfilePictureUrl] nvarchar(max) NULL,
        [LastLoginOn] datetime2 NULL,
        [IsPasswordChanged] bit NOT NULL CONSTRAINT [DF_Users_IsPasswordChanged] DEFAULT (0),
        [IsActive] bit NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedOn] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Users_Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [Branches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_Users_BranchId] ON [Users] ([BranchId]);
    CREATE INDEX [IX_Users_InstituteId] ON [Users] ([InstituteId]);
    CREATE INDEX [IX_Users_InstituteId_Email] ON [Users] ([InstituteId], [Email]);
    CREATE INDEX [IX_Users_InstituteId_MobileNumber] ON [Users] ([InstituteId], [MobileNumber]);
    CREATE INDEX [IX_Users_InstituteId_Username] ON [Users] ([InstituteId], [Username]);
    CREATE INDEX [IX_Users_RoleId] ON [Users] ([RoleId]);
END
GO

-- UserPermissions Mapping Table
IF OBJECT_ID(N'[UserPermissions]') IS NULL
BEGIN
    CREATE TABLE [UserPermissions] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [PermissionId] uniqueidentifier NOT NULL,
        [IsGranted] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_UserPermissions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserPermissions_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_UserPermissions_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_UserPermissions_PermissionId] ON [UserPermissions] ([PermissionId]);
    CREATE UNIQUE INDEX [IX_UserPermissions_UserId_PermissionId] ON [UserPermissions] ([UserId], [PermissionId]);
END
GO

-- StaffProfiles Table
IF OBJECT_ID(N'[StaffProfiles]') IS NULL
BEGIN
    CREATE TABLE [StaffProfiles] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [StaffCode] nvarchar(50) NOT NULL,
        [StaffType] nvarchar(50) NOT NULL,
        [Designation] nvarchar(100) NULL,
        [Department] nvarchar(100) NULL,
        [Qualification] nvarchar(200) NULL,
        [JoiningDate] datetime2 NOT NULL CONSTRAINT [DF_StaffProfiles_JoiningDate] DEFAULT (SYSUTCDATETIME()),
        [ExperienceYears] int NOT NULL CONSTRAINT [DF_StaffProfiles_ExperienceYears] DEFAULT (0),
        [Address] nvarchar(500) NULL,
        [ProfilePhoto] nvarchar(500) NULL,
        [EmergencyContactName] nvarchar(150) NULL,
        [EmergencyContactNumber] nvarchar(20) NULL,
        [IsActive] bit NOT NULL CONSTRAINT [DF_StaffProfiles_IsActive] DEFAULT (1),
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedOn] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_StaffProfiles] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_StaffProfiles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
    CREATE UNIQUE INDEX [IX_StaffProfiles_InstituteId_StaffCode] ON [StaffProfiles] ([InstituteId], [StaffCode]);
    CREATE UNIQUE INDEX [IX_StaffProfiles_UserId] ON [StaffProfiles] ([UserId]);
END
GO

-- TeacherProfiles Table
IF OBJECT_ID(N'[TeacherProfiles]') IS NULL
BEGIN
    CREATE TABLE [TeacherProfiles] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [SubjectExpertise] nvarchar(250) NULL,
        [TeachingExperienceYears] int NOT NULL CONSTRAINT [DF_TeacherProfiles_TeachingExperienceYears] DEFAULT (0),
        [Bio] nvarchar(1000) NULL,
        [TeacherType] nvarchar(50) NULL,
        [IsActive] bit NOT NULL CONSTRAINT [DF_TeacherProfiles_IsActive] DEFAULT (1),
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedOn] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_TeacherProfiles] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TeacherProfiles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
    CREATE UNIQUE INDEX [IX_TeacherProfiles_UserId] ON [TeacherProfiles] ([UserId]);
END
GO

-- TeacherQualifications Table
IF OBJECT_ID(N'[TeacherQualifications]') IS NULL
BEGIN
    CREATE TABLE [TeacherQualifications] (
        [Id] uniqueidentifier NOT NULL,
        [TeacherProfileId] uniqueidentifier NOT NULL,
        [Qualification] nvarchar(max) NOT NULL,
        [Specialization] nvarchar(max) NULL,
        [University] nvarchar(max) NOT NULL,
        [PassingYear] int NOT NULL,
        [PercentageOrCGPA] nvarchar(max) NULL,
        [CertificateFilePath] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_TeacherQualifications] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TeacherQualifications_TeacherProfiles_TeacherProfileId] FOREIGN KEY ([TeacherProfileId]) REFERENCES [TeacherProfiles] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_TeacherQualifications_TeacherProfileId] ON [TeacherQualifications] ([TeacherProfileId]);
END
GO

-- TeacherDocuments Table
IF OBJECT_ID(N'[TeacherDocuments]') IS NULL
BEGIN
    CREATE TABLE [TeacherDocuments] (
        [Id] uniqueidentifier NOT NULL,
        [TeacherProfileId] uniqueidentifier NOT NULL,
        [DocumentType] nvarchar(max) NOT NULL,
        [DocumentNumber] nvarchar(max) NULL,
        [FilePath] nvarchar(max) NOT NULL,
        [OriginalFileName] nvarchar(max) NOT NULL,
        [StoredFileName] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_TeacherDocuments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TeacherDocuments_TeacherProfiles_TeacherProfileId] FOREIGN KEY ([TeacherProfileId]) REFERENCES [TeacherProfiles] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_TeacherDocuments_TeacherProfileId] ON [TeacherDocuments] ([TeacherProfileId]);
END
GO

-- TeacherSalaries Table
IF OBJECT_ID(N'[TeacherSalaries]') IS NULL
BEGIN
    CREATE TABLE [TeacherSalaries] (
        [Id] uniqueidentifier NOT NULL,
        [TeacherProfileId] uniqueidentifier NOT NULL,
        [SalaryType] nvarchar(max) NOT NULL,
        [SalaryAmount] decimal(18,2) NOT NULL,
        [AccountNumber] nvarchar(max) NULL,
        [BankName] nvarchar(max) NULL,
        [IFSCCode] nvarchar(max) NULL,
        [PanNumber] nvarchar(max) NULL,
        [EffectiveFrom] date NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_TeacherSalaries] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TeacherSalaries_TeacherProfiles_TeacherProfileId] FOREIGN KEY ([TeacherProfileId]) REFERENCES [TeacherProfiles] ([Id]) ON DELETE NO ACTION
    );
    CREATE UNIQUE INDEX [IX_TeacherSalaries_TeacherProfileId] ON [TeacherSalaries] ([TeacherProfileId]);
END
GO

-- EmergencyContacts Table
IF OBJECT_ID(N'[EmergencyContacts]') IS NULL
BEGIN
    CREATE TABLE [EmergencyContacts] (
        [Id] uniqueidentifier NOT NULL,
        [TeacherProfileId] uniqueidentifier NOT NULL,
        [ContactPersonName] nvarchar(max) NOT NULL,
        [Relation] nvarchar(max) NOT NULL,
        [PhoneNumber] nvarchar(max) NOT NULL,
        [AlternateNumber] nvarchar(max) NULL,
        [Address] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_EmergencyContacts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_EmergencyContacts_TeacherProfiles_TeacherProfileId] FOREIGN KEY ([TeacherProfileId]) REFERENCES [TeacherProfiles] ([Id]) ON DELETE NO ACTION
    );
    CREATE UNIQUE INDEX [IX_EmergencyContacts_TeacherProfileId] ON [EmergencyContacts] ([TeacherProfileId]);
END
GO

-- ====================================================================================================
-- 4. ACADEMICS & COURSES
-- ====================================================================================================

-- AcademicSessions Table
IF OBJECT_ID(N'[AcademicSessions]') IS NULL
BEGIN
    CREATE TABLE [AcademicSessions] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(50) NOT NULL,
        [StartDate] date NOT NULL,
        [EndDate] date NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AcademicSessions] PRIMARY KEY ([Id])
    );
END
GO

-- Courses Table
IF OBJECT_ID(N'[Courses]') IS NULL
BEGIN
    CREATE TABLE [Courses] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [CourseCode] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [CourseCategory] nvarchar(max) NOT NULL,
        [CourseType] nvarchar(max) NOT NULL,
        [DurationValue] int NOT NULL,
        [DurationType] nvarchar(max) NOT NULL,
        [TotalFees] decimal(18,2) NOT NULL,
        [RegistrationFees] decimal(18,2) NULL,
        [MinimumStudents] int NULL,
        [MaxStudents] int NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Courses] PRIMARY KEY ([Id])
    );
    CREATE INDEX [IX_Courses_InstituteId] ON [Courses] ([InstituteId]);
END
GO

-- CourseFeeStructures Table
IF OBJECT_ID(N'[CourseFeeStructures]') IS NULL
BEGIN
    CREATE TABLE [CourseFeeStructures] (
        [Id] uniqueidentifier NOT NULL,
        [CourseId] uniqueidentifier NOT NULL,
        [AdmissionFee] decimal(18,2) NULL,
        [MonthlyFee] decimal(18,2) NULL,
        [TotalFee] decimal(18,2) NOT NULL,
        [InstallmentCount] int NULL,
        [DiscountAllowed] bit NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_CourseFeeStructures] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CourseFeeStructures_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_CourseFeeStructures_CourseId] ON [CourseFeeStructures] ([CourseId]);
END
GO

-- Subjects Table
IF OBJECT_ID(N'[Subjects]') IS NULL
BEGIN
    CREATE TABLE [Subjects] (
        [Id] uniqueidentifier NOT NULL,
        [CourseId] uniqueidentifier NOT NULL,
        [CourseId1] uniqueidentifier NULL,
        [Name] nvarchar(150) NOT NULL,
        [Description] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Subjects] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Subjects_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Subjects_Courses_CourseId1] FOREIGN KEY ([CourseId1]) REFERENCES [Courses] ([Id])
    );
    CREATE INDEX [IX_Subjects_CourseId] ON [Subjects] ([CourseId]);
    CREATE INDEX [IX_Subjects_CourseId1] ON [Subjects] ([CourseId1]);
END
GO

-- TeacherSubjects Mapping Table
IF OBJECT_ID(N'[TeacherSubjects]') IS NULL
BEGIN
    CREATE TABLE [TeacherSubjects] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_TeacherSubjects] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TeacherSubjects_Subjects_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TeacherSubjects_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_TeacherSubjects_SubjectId] ON [TeacherSubjects] ([SubjectId]);
    CREATE UNIQUE INDEX [IX_TeacherSubjects_UserId_SubjectId] ON [TeacherSubjects] ([UserId], [SubjectId]);
END
GO

-- Batches Table
IF OBJECT_ID(N'[Batches]') IS NULL
BEGIN
    CREATE TABLE [Batches] (
        [Id] uniqueidentifier NOT NULL,
        [CourseId] uniqueidentifier NOT NULL,
        [BranchId] uniqueidentifier NULL,
        [Name] nvarchar(150) NOT NULL,
        [BatchCode] nvarchar(max) NOT NULL,
        [BatchStatus] nvarchar(max) NOT NULL,
        [Capacity] int NOT NULL,
        [RoomNumber] nvarchar(max) NULL,
        [Notes] nvarchar(max) NULL,
        [StartDate] date NULL,
        [EndDate] date NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Batches] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Batches_Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [Branches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Batches_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_Batches_BranchId] ON [Batches] ([BranchId]);
    CREATE INDEX [IX_Batches_CourseId] ON [Batches] ([CourseId]);
    CREATE INDEX [IX_Batches_InstituteId] ON [Batches] ([InstituteId]);
END
GO

-- BatchSchedules Table
IF OBJECT_ID(N'[BatchSchedules]') IS NULL
BEGIN
    CREATE TABLE [BatchSchedules] (
        [Id] uniqueidentifier NOT NULL,
        [BatchId] uniqueidentifier NOT NULL,
        [TeacherProfileId] uniqueidentifier NULL,
        [DayOfWeek] nvarchar(max) NOT NULL,
        [StartTime] time NOT NULL,
        [EndTime] time NOT NULL,
        [RoomNumber] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_BatchSchedules] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BatchSchedules_Batches_BatchId] FOREIGN KEY ([BatchId]) REFERENCES [Batches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_BatchSchedules_TeacherProfiles_TeacherProfileId] FOREIGN KEY ([TeacherProfileId]) REFERENCES [TeacherProfiles] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_BatchSchedules_BatchId] ON [BatchSchedules] ([BatchId]);
    CREATE INDEX [IX_BatchSchedules_TeacherProfileId] ON [BatchSchedules] ([TeacherProfileId]);
END
GO

-- TeacherBatches Mapping Table
IF OBJECT_ID(N'[TeacherBatches]') IS NULL
BEGIN
    CREATE TABLE [TeacherBatches] (
        [Id] uniqueidentifier NOT NULL,
        [TeacherProfileId] uniqueidentifier NULL,
        [BatchId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NOT NULL,
        [AssignedOn] datetime2 NOT NULL CONSTRAINT [DF_TeacherBatches_AssignedOn] DEFAULT (SYSUTCDATETIME()),
        [IsActive] bit NOT NULL CONSTRAINT [DF_TeacherBatches_IsActive] DEFAULT (1),
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_TeacherBatches] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TeacherBatches_Batches_BatchId] FOREIGN KEY ([BatchId]) REFERENCES [Batches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TeacherBatches_Subjects_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TeacherBatches_TeacherProfiles_TeacherProfileId] FOREIGN KEY ([TeacherProfileId]) REFERENCES [TeacherProfiles] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_TeacherBatches_BatchId] ON [TeacherBatches] ([BatchId]);
    CREATE INDEX [IX_TeacherBatches_SubjectId] ON [TeacherBatches] ([SubjectId]);
    CREATE INDEX [IX_TeacherBatches_TeacherProfileId] ON [TeacherBatches] ([TeacherProfileId]);
END
GO

-- ====================================================================================================
-- 5. STUDENTS & PARENTS
-- ====================================================================================================

-- Students Table
IF OBJECT_ID(N'[Students]') IS NULL
BEGIN
    CREATE TABLE [Students] (
        [Id] uniqueidentifier NOT NULL,
        [StudentCode] nvarchar(50) NOT NULL,
        [FullName] nvarchar(150) NOT NULL,
        [Mobile] nvarchar(20) NULL,
        [ParentName] nvarchar(max) NULL,
        [ParentMobile] nvarchar(max) NULL,
        [Email] nvarchar(150) NULL,
        [DateOfBirth] date NULL,
        [Gender] nvarchar(max) NULL,
        [Address] nvarchar(max) NULL,
        [Qualification] nvarchar(max) NULL,
        [ProfileImagePath] nvarchar(max) NULL,
        [Status] nvarchar(max) NOT NULL,
        [AdmissionDate] date NOT NULL,
        [RollNumber] nvarchar(max) NULL,
        [FatherName] nvarchar(max) NULL,
        [MotherName] nvarchar(max) NULL,
        [ParentEmail] nvarchar(max) NULL,
        [BloodGroup] nvarchar(max) NULL,
        [SchoolOrCollegeName] nvarchar(max) NULL,
        [TargetExam] nvarchar(max) NULL,
        [TargetYear] int NULL,
        [Category] nvarchar(max) NULL,
        [AadhaarNumber] nvarchar(max) NULL,
        [EmergencyContactName] nvarchar(max) NULL,
        [EmergencyContactNumber] nvarchar(max) NULL,
        [AddressLine1] nvarchar(max) NULL,
        [AddressLine2] nvarchar(max) NULL,
        [City] nvarchar(max) NULL,
        [State] nvarchar(max) NULL,
        [Pincode] nvarchar(max) NULL,
        [PreviousMarksPercentage] decimal(5,2) NULL,
        [MedicalHistory] nvarchar(max) NULL,
        [DiscountReason] nvarchar(max) NULL,
        [DiscountPercentage] decimal(5,2) NULL,
        [ScholarshipName] nvarchar(max) NULL,
        [ScholarshipAmount] decimal(18,2) NULL,
        [LeadSource] nvarchar(max) NULL,
        [CounselledBy] nvarchar(max) NULL,
        [AdmissionNotes] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Students] PRIMARY KEY ([Id])
    );
    CREATE INDEX [IX_Students_InstituteId] ON [Students] ([InstituteId]);
    CREATE INDEX [IX_Students_Mobile] ON [Students] ([Mobile]);
    CREATE INDEX [IX_Students_StudentCode] ON [Students] ([StudentCode]);
END
GO

-- StudentBatches Mapping Table
IF OBJECT_ID(N'[StudentBatches]') IS NULL
BEGIN
    CREATE TABLE [StudentBatches] (
        [Id] uniqueidentifier NOT NULL,
        [StudentId] uniqueidentifier NOT NULL,
        [BatchId] uniqueidentifier NOT NULL,
        [JoinedDate] date NOT NULL,
        [LeftDate] date NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_StudentBatches] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_StudentBatches_Batches_BatchId] FOREIGN KEY ([BatchId]) REFERENCES [Batches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_StudentBatches_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_StudentBatches_BatchId] ON [StudentBatches] ([BatchId]);
    CREATE INDEX [IX_StudentBatches_StudentId] ON [StudentBatches] ([StudentId]);
END
GO

-- StudentDocuments Table
IF OBJECT_ID(N'[StudentDocuments]') IS NULL
BEGIN
    CREATE TABLE [StudentDocuments] (
        [Id] uniqueidentifier NOT NULL,
        [StudentId] uniqueidentifier NOT NULL,
        [DocumentType] nvarchar(100) NOT NULL,
        [OriginalFileName] nvarchar(255) NOT NULL,
        [StoredFileName] nvarchar(255) NOT NULL,
        [FilePath] nvarchar(500) NOT NULL,
        [FileSize] bigint NOT NULL,
        [UploadedAt] datetime2 NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_StudentDocuments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_StudentDocuments_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_StudentDocuments_StudentId] ON [StudentDocuments] ([StudentId]);
END
GO

-- Parents Table
IF OBJECT_ID(N'[Parents]') IS NULL
BEGIN
    CREATE TABLE [Parents] (
        [Id] uniqueidentifier NOT NULL,
        [FullName] nvarchar(150) NOT NULL,
        [Mobile] nvarchar(20) NOT NULL,
        [Email] nvarchar(150) NULL,
        [Occupation] nvarchar(100) NULL,
        [Address] nvarchar(max) NULL,
        [City] nvarchar(max) NULL,
        [State] nvarchar(max) NULL,
        [Pincode] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_Parents] PRIMARY KEY ([Id])
    );
END
GO

-- StudentParents Mapping Table
IF OBJECT_ID(N'[StudentParents]') IS NULL
BEGIN
    CREATE TABLE [StudentParents] (
        [Id] uniqueidentifier NOT NULL,
        [StudentId] uniqueidentifier NOT NULL,
        [ParentId] uniqueidentifier NOT NULL,
        [RelationshipType] nvarchar(50) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_StudentParents] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_StudentParents_Parents_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Parents] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_StudentParents_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_StudentParents_ParentId] ON [StudentParents] ([ParentId]);
    CREATE UNIQUE INDEX [IX_StudentParents_StudentId_ParentId] ON [StudentParents] ([StudentId], [ParentId]);
END
GO

-- ====================================================================================================
-- 6. CRM & LEADS
-- ====================================================================================================

-- Enquiries Table
IF OBJECT_ID(N'[Enquiries]') IS NULL
BEGIN
    CREATE TABLE [Enquiries] (
        [Id] uniqueidentifier NOT NULL,
        [FullName] nvarchar(150) NOT NULL,
        [Mobile] nvarchar(20) NOT NULL,
        [Email] nvarchar(150) NULL,
        [InterestedCourseId] uniqueidentifier NULL,
        [Source] nvarchar(max) NULL,
        [Status] nvarchar(450) NOT NULL,
        [NextFollowUpDate] date NULL,
        [Remark] nvarchar(max) NULL,
        [AssignedToUserId] uniqueidentifier NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Enquiries] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Enquiries_Courses_InterestedCourseId] FOREIGN KEY ([InterestedCourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Enquiries_Users_AssignedToUserId] FOREIGN KEY ([AssignedToUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_Enquiries_AssignedToUserId] ON [Enquiries] ([AssignedToUserId]);
    CREATE INDEX [IX_Enquiries_InstituteId] ON [Enquiries] ([InstituteId]);
    CREATE INDEX [IX_Enquiries_InterestedCourseId] ON [Enquiries] ([InterestedCourseId]);
    CREATE INDEX [IX_Enquiries_Mobile] ON [Enquiries] ([Mobile]);
    CREATE INDEX [IX_Enquiries_Status] ON [Enquiries] ([Status]);
END
GO

-- FollowUps Table
IF OBJECT_ID(N'[FollowUps]') IS NULL
BEGIN
    CREATE TABLE [FollowUps] (
        [Id] uniqueidentifier NOT NULL,
        [EnquiryId] uniqueidentifier NOT NULL,
        [FollowUpDate] datetime2 NOT NULL,
        [NextFollowUpDate] date NULL,
        [Remark] nvarchar(1000) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_FollowUps] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_FollowUps_Enquiries_EnquiryId] FOREIGN KEY ([EnquiryId]) REFERENCES [Enquiries] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_FollowUps_EnquiryId] ON [FollowUps] ([EnquiryId]);
END
GO

-- DemoClasses Table
IF OBJECT_ID(N'[DemoClasses]') IS NULL
BEGIN
    CREATE TABLE [DemoClasses] (
        [Id] uniqueidentifier NOT NULL,
        [EnquiryId] uniqueidentifier NOT NULL,
        [BatchId] uniqueidentifier NULL,
        [DemoDate] datetime2 NOT NULL,
        [Status] nvarchar(30) NOT NULL,
        [Remark] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_DemoClasses] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DemoClasses_Batches_BatchId] FOREIGN KEY ([BatchId]) REFERENCES [Batches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_DemoClasses_Enquiries_EnquiryId] FOREIGN KEY ([EnquiryId]) REFERENCES [Enquiries] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_DemoClasses_BatchId] ON [DemoClasses] ([BatchId]);
    CREATE INDEX [IX_DemoClasses_EnquiryId] ON [DemoClasses] ([EnquiryId]);
END
GO

-- ====================================================================================================
-- 7. FINANCE & FEES
-- ====================================================================================================

-- FeePlans Table
IF OBJECT_ID(N'[FeePlans]') IS NULL
BEGIN
    CREATE TABLE [FeePlans] (
        [Id] uniqueidentifier NOT NULL,
        [StudentId] uniqueidentifier NOT NULL,
        [CourseId] uniqueidentifier NOT NULL,
        [BatchId] uniqueidentifier NULL,
        [TotalFee] decimal(18,2) NOT NULL,
        [DiscountAmount] decimal(18,2) NOT NULL,
        [FinalFee] decimal(18,2) NOT NULL,
        [PlanType] nvarchar(30) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_FeePlans] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_FeePlans_Batches_BatchId] FOREIGN KEY ([BatchId]) REFERENCES [Batches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_FeePlans_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_FeePlans_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_FeePlans_BatchId] ON [FeePlans] ([BatchId]);
    CREATE INDEX [IX_FeePlans_CourseId] ON [FeePlans] ([CourseId]);
    CREATE INDEX [IX_FeePlans_StudentId] ON [FeePlans] ([StudentId]);
END
GO

-- Installments Table
IF OBJECT_ID(N'[Installments]') IS NULL
BEGIN
    CREATE TABLE [Installments] (
        [Id] uniqueidentifier NOT NULL,
        [FeePlanId] uniqueidentifier NOT NULL,
        [InstallmentNo] int NOT NULL,
        [DueDate] date NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [Status] nvarchar(30) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Installments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Installments_FeePlans_FeePlanId] FOREIGN KEY ([FeePlanId]) REFERENCES [FeePlans] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_Installments_DueDate] ON [Installments] ([DueDate]);
    CREATE INDEX [IX_Installments_FeePlanId] ON [Installments] ([FeePlanId]);
    CREATE INDEX [IX_Installments_Status] ON [Installments] ([Status]);
END
GO

-- Payments Table
IF OBJECT_ID(N'[Payments]') IS NULL
BEGIN
    CREATE TABLE [Payments] (
        [Id] uniqueidentifier NOT NULL,
        [StudentId] uniqueidentifier NOT NULL,
        [FeePlanId] uniqueidentifier NOT NULL,
        [InstallmentId] uniqueidentifier NULL,
        [ReceiptNo] nvarchar(50) NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [PaymentMode] nvarchar(30) NOT NULL,
        [TransactionNo] nvarchar(max) NULL,
        [PaymentDate] datetime2 NOT NULL,
        [Remark] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Payments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Payments_FeePlans_FeePlanId] FOREIGN KEY ([FeePlanId]) REFERENCES [FeePlans] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Payments_Installments_InstallmentId] FOREIGN KEY ([InstallmentId]) REFERENCES [Installments] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Payments_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_Payments_FeePlanId] ON [Payments] ([FeePlanId]);
    CREATE INDEX [IX_Payments_InstallmentId] ON [Payments] ([InstallmentId]);
    CREATE INDEX [IX_Payments_PaymentDate] ON [Payments] ([PaymentDate]);
    CREATE INDEX [IX_Payments_StudentId] ON [Payments] ([StudentId]);
END
GO

-- PaymentTransactions Table
IF OBJECT_ID(N'[PaymentTransactions]') IS NULL
BEGIN
    CREATE TABLE [PaymentTransactions] (
        [Id] uniqueidentifier NOT NULL,
        [PaymentId] uniqueidentifier NOT NULL,
        [Gateway] nvarchar(100) NOT NULL,
        [TransactionRef] nvarchar(100) NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [Status] nvarchar(30) NOT NULL,
        [RawResponse] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_PaymentTransactions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PaymentTransactions_Payments_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [Payments] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_PaymentTransactions_PaymentId] ON [PaymentTransactions] ([PaymentId]);
END
GO

-- Expenses Table
IF OBJECT_ID(N'[Expenses]') IS NULL
BEGIN
    CREATE TABLE [Expenses] (
        [Id] uniqueidentifier NOT NULL,
        [Category] nvarchar(100) NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [ExpenseDate] date NOT NULL,
        [PaymentMode] nvarchar(max) NULL,
        [Remark] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Expenses] PRIMARY KEY ([Id])
    );
END
GO

-- ====================================================================================================
-- 8. LEARNING, ATTENDANCE & ACADEMICS
-- ====================================================================================================

-- AttendanceSessions Table
IF OBJECT_ID(N'[AttendanceSessions]') IS NULL
BEGIN
    CREATE TABLE [AttendanceSessions] (
        [Id] uniqueidentifier NOT NULL,
        [BatchId] uniqueidentifier NOT NULL,
        [AttendanceDate] date NOT NULL,
        [TakenByUserId] uniqueidentifier NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AttendanceSessions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AttendanceSessions_Batches_BatchId] FOREIGN KEY ([BatchId]) REFERENCES [Batches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AttendanceSessions_Users_TakenByUserId] FOREIGN KEY ([TakenByUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_AttendanceSessions_AttendanceDate] ON [AttendanceSessions] ([AttendanceDate]);
    CREATE INDEX [IX_AttendanceSessions_BatchId] ON [AttendanceSessions] ([BatchId]);
    CREATE INDEX [IX_AttendanceSessions_TakenByUserId] ON [AttendanceSessions] ([TakenByUserId]);
END
GO

-- AttendanceRecords Table
IF OBJECT_ID(N'[AttendanceRecords]') IS NULL
BEGIN
    CREATE TABLE [AttendanceRecords] (
        [Id] uniqueidentifier NOT NULL,
        [AttendanceSessionId] uniqueidentifier NOT NULL,
        [StudentId] uniqueidentifier NOT NULL,
        [Status] nvarchar(20) NOT NULL,
        [Remark] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AttendanceRecords] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AttendanceRecords_AttendanceSessions_AttendanceSessionId] FOREIGN KEY ([AttendanceSessionId]) REFERENCES [AttendanceSessions] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AttendanceRecords_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_AttendanceRecords_AttendanceSessionId] ON [AttendanceRecords] ([AttendanceSessionId]);
    CREATE INDEX [IX_AttendanceRecords_StudentId] ON [AttendanceRecords] ([StudentId]);
END
GO

-- Tests Table
IF OBJECT_ID(N'[Tests]') IS NULL
BEGIN
    CREATE TABLE [Tests] (
        [Id] uniqueidentifier NOT NULL,
        [CourseId] uniqueidentifier NOT NULL,
        [BatchId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NULL,
        [TestName] nvarchar(150) NOT NULL,
        [TestDate] date NOT NULL,
        [MaxMarks] decimal(10,2) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Tests] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Tests_Batches_BatchId] FOREIGN KEY ([BatchId]) REFERENCES [Batches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Tests_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Tests_Subjects_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_Tests_BatchId] ON [Tests] ([BatchId]);
    CREATE INDEX [IX_Tests_CourseId] ON [Tests] ([CourseId]);
    CREATE INDEX [IX_Tests_SubjectId] ON [Tests] ([SubjectId]);
END
GO

-- TestResults Table
IF OBJECT_ID(N'[TestResults]') IS NULL
BEGIN
    CREATE TABLE [TestResults] (
        [Id] uniqueidentifier NOT NULL,
        [TestId] uniqueidentifier NOT NULL,
        [StudentId] uniqueidentifier NOT NULL,
        [MarksObtained] decimal(10,2) NOT NULL,
        [Remark] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_TestResults] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TestResults_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TestResults_Tests_TestId] FOREIGN KEY ([TestId]) REFERENCES [Tests] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_TestResults_StudentId] ON [TestResults] ([StudentId]);
    CREATE INDEX [IX_TestResults_TestId] ON [TestResults] ([TestId]);
END
GO

-- Notes (Study Material) Table
IF OBJECT_ID(N'[Notes]') IS NULL
BEGIN
    CREATE TABLE [Notes] (
        [Id] uniqueidentifier NOT NULL,
        [CourseId] uniqueidentifier NULL,
        [BatchId] uniqueidentifier NULL,
        [SubjectId] uniqueidentifier NULL,
        [Title] nvarchar(200) NOT NULL,
        [Description] nvarchar(max) NULL,
        [OriginalFileName] nvarchar(255) NOT NULL,
        [StoredFileName] nvarchar(255) NOT NULL,
        [FilePath] nvarchar(500) NOT NULL,
        [FileType] nvarchar(150) NOT NULL,
        [FileSizeInBytes] bigint NOT NULL,
        [UploadedByUserId] uniqueidentifier NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Notes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Notes_Batches_BatchId] FOREIGN KEY ([BatchId]) REFERENCES [Batches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Notes_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Notes_Subjects_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Notes_Users_UploadedByUserId] FOREIGN KEY ([UploadedByUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_Notes_BatchId] ON [Notes] ([BatchId]);
    CREATE INDEX [IX_Notes_CourseId] ON [Notes] ([CourseId]);
    CREATE INDEX [IX_Notes_SubjectId] ON [Notes] ([SubjectId]);
    CREATE INDEX [IX_Notes_UploadedByUserId] ON [Notes] ([UploadedByUserId]);
END
GO

-- Assignments Table
IF OBJECT_ID(N'[Assignments]') IS NULL
BEGIN
    CREATE TABLE [Assignments] (
        [Id] uniqueidentifier NOT NULL,
        [CourseId] uniqueidentifier NOT NULL,
        [BatchId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NOT NULL,
        [CreatedByUserId] uniqueidentifier NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [Description] nvarchar(max) NULL,
        [DueDate] datetime2 NOT NULL,
        [AttachmentUrl] nvarchar(max) NULL,
        [AttachmentFileName] nvarchar(max) NULL,
        [MaxMarks] decimal(18,2) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Assignments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Assignments_Batches_BatchId] FOREIGN KEY ([BatchId]) REFERENCES [Batches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Assignments_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Assignments_Subjects_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Assignments_Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_Assignments_BatchId] ON [Assignments] ([BatchId]);
    CREATE INDEX [IX_Assignments_CourseId] ON [Assignments] ([CourseId]);
    CREATE INDEX [IX_Assignments_CreatedByUserId] ON [Assignments] ([CreatedByUserId]);
    CREATE INDEX [IX_Assignments_SubjectId] ON [Assignments] ([SubjectId]);
END
GO

-- AssignmentSubmissions Table
IF OBJECT_ID(N'[AssignmentSubmissions]') IS NULL
BEGIN
    CREATE TABLE [AssignmentSubmissions] (
        [Id] uniqueidentifier NOT NULL,
        [AssignmentId] uniqueidentifier NOT NULL,
        [StudentId] uniqueidentifier NOT NULL,
        [ReviewedByUserId] uniqueidentifier NULL,
        [SubmissionDate] datetime2 NOT NULL,
        [AttachmentUrl] nvarchar(max) NOT NULL,
        [AttachmentFileName] nvarchar(max) NOT NULL,
        [Comments] nvarchar(max) NULL,
        [MarksObtained] decimal(18,2) NULL,
        [Status] nvarchar(max) NOT NULL,
        [ReviewDate] datetime2 NULL,
        [Feedback] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AssignmentSubmissions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AssignmentSubmissions_Assignments_AssignmentId] FOREIGN KEY ([AssignmentId]) REFERENCES [Assignments] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AssignmentSubmissions_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AssignmentSubmissions_Users_ReviewedByUserId] FOREIGN KEY ([ReviewedByUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_AssignmentSubmissions_AssignmentId] ON [AssignmentSubmissions] ([AssignmentId]);
    CREATE INDEX [IX_AssignmentSubmissions_ReviewedByUserId] ON [AssignmentSubmissions] ([ReviewedByUserId]);
    CREATE INDEX [IX_AssignmentSubmissions_StudentId] ON [AssignmentSubmissions] ([StudentId]);
END
GO

-- Doubts Table
IF OBJECT_ID(N'[Doubts]') IS NULL
BEGIN
    CREATE TABLE [Doubts] (
        [Id] uniqueidentifier NOT NULL,
        [StudentId] uniqueidentifier NOT NULL,
        [CourseId] uniqueidentifier NOT NULL,
        [BatchId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [AttachmentUrl] nvarchar(max) NULL,
        [AttachmentFileName] nvarchar(max) NULL,
        [Status] nvarchar(50) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Doubts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Doubts_Batches_BatchId] FOREIGN KEY ([BatchId]) REFERENCES [Batches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Doubts_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Doubts_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Doubts_Subjects_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_Doubts_BatchId] ON [Doubts] ([BatchId]);
    CREATE INDEX [IX_Doubts_CourseId] ON [Doubts] ([CourseId]);
    CREATE INDEX [IX_Doubts_StudentId] ON [Doubts] ([StudentId]);
    CREATE INDEX [IX_Doubts_SubjectId] ON [Doubts] ([SubjectId]);
END
GO

-- DoubtReplies Table
IF OBJECT_ID(N'[DoubtReplies]') IS NULL
BEGIN
    CREATE TABLE [DoubtReplies] (
        [Id] uniqueidentifier NOT NULL,
        [DoubtId] uniqueidentifier NOT NULL,
        [RepliedByUserId] uniqueidentifier NOT NULL,
        [ReplyText] nvarchar(max) NOT NULL,
        [AttachmentUrl] nvarchar(max) NULL,
        [AttachmentFileName] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_DoubtReplies] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DoubtReplies_Doubts_DoubtId] FOREIGN KEY ([DoubtId]) REFERENCES [Doubts] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_DoubtReplies_Users_RepliedByUserId] FOREIGN KEY ([RepliedByUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_DoubtReplies_DoubtId] ON [DoubtReplies] ([DoubtId]);
    CREATE INDEX [IX_DoubtReplies_RepliedByUserId] ON [DoubtReplies] ([RepliedByUserId]);
END
GO

-- ====================================================================================================
-- 9. COMMUNICATION, VACANCIES & NOTIFICATIONS
-- ====================================================================================================

-- Notices Table
IF OBJECT_ID(N'[Notices]') IS NULL
BEGIN
    CREATE TABLE [Notices] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [Message] nvarchar(1000) NOT NULL,
        [TargetType] nvarchar(max) NOT NULL,
        [CourseId] uniqueidentifier NULL,
        [BatchId] uniqueidentifier NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Notices] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Notices_Batches_BatchId] FOREIGN KEY ([BatchId]) REFERENCES [Batches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Notices_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_Notices_BatchId] ON [Notices] ([BatchId]);
    CREATE INDEX [IX_Notices_CourseId] ON [Notices] ([CourseId]);
END
GO

-- Notifications Table
IF OBJECT_ID(N'[Notifications]') IS NULL
BEGIN
    CREATE TABLE [Notifications] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [Message] nvarchar(1000) NOT NULL,
        [NotificationType] nvarchar(50) NOT NULL,
        [LinkUrl] nvarchar(500) NULL,
        [IsRead] bit NOT NULL,
        [ReadAt] datetime2 NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id])
    );
    CREATE INDEX [IX_Notifications_IsRead] ON [Notifications] ([IsRead]);
    CREATE INDEX [IX_Notifications_UserId] ON [Notifications] ([UserId]);
END
GO

-- Vacancies Table
IF OBJECT_ID(N'[Vacancies]') IS NULL
BEGIN
    CREATE TABLE [Vacancies] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [Department] nvarchar(max) NULL,
        [ExamCategory] nvarchar(100) NOT NULL,
        [QualificationRequired] nvarchar(max) NULL,
        [AgeLimit] nvarchar(max) NULL,
        [StartDate] date NULL,
        [LastDate] date NOT NULL,
        [OfficialLink] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Vacancies] PRIMARY KEY ([Id])
    );
    CREATE INDEX [IX_Vacancies_LastDate] ON [Vacancies] ([LastDate]);
END
GO

-- VacancyCourseMappings Table
IF OBJECT_ID(N'[VacancyCourseMappings]') IS NULL
BEGIN
    CREATE TABLE [VacancyCourseMappings] (
        [Id] uniqueidentifier NOT NULL,
        [VacancyId] uniqueidentifier NOT NULL,
        [CourseId] uniqueidentifier NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_VacancyCourseMappings] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_VacancyCourseMappings_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_VacancyCourseMappings_Vacancies_VacancyId] FOREIGN KEY ([VacancyId]) REFERENCES [Vacancies] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_VacancyCourseMappings_CourseId] ON [VacancyCourseMappings] ([CourseId]);
    CREATE INDEX [IX_VacancyCourseMappings_VacancyId] ON [VacancyCourseMappings] ([VacancyId]);
END
GO

-- ====================================================================================================
-- 10. IMPORT/EXPORT & AUDIT LOGS
-- ====================================================================================================

-- ImportJobs Table
IF OBJECT_ID(N'[ImportJobs]') IS NULL
BEGIN
    CREATE TABLE [ImportJobs] (
        [Id] uniqueidentifier NOT NULL,
        [ImportType] nvarchar(100) NOT NULL,
        [FileName] nvarchar(255) NOT NULL,
        [TotalRows] int NOT NULL,
        [SuccessRows] int NOT NULL,
        [FailedRows] int NOT NULL,
        [Status] nvarchar(30) NOT NULL,
        [UploadedBy] uniqueidentifier NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_ImportJobs] PRIMARY KEY ([Id])
    );
END
GO

-- ImportJobErrors Table
IF OBJECT_ID(N'[ImportJobErrors]') IS NULL
BEGIN
    CREATE TABLE [ImportJobErrors] (
        [Id] uniqueidentifier NOT NULL,
        [ImportJobId] uniqueidentifier NOT NULL,
        [RowNumber] int NOT NULL,
        [ErrorMessage] nvarchar(1000) NOT NULL,
        [RawData] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_ImportJobErrors] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ImportJobErrors_ImportJobs_ImportJobId] FOREIGN KEY ([ImportJobId]) REFERENCES [ImportJobs] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_ImportJobErrors_ImportJobId] ON [ImportJobErrors] ([ImportJobId]);
END
GO

-- AuditLogs Table
IF OBJECT_ID(N'[AuditLogs]') IS NULL
BEGIN
    CREATE TABLE [AuditLogs] (
        [Id] uniqueidentifier NOT NULL,
        [EntityName] nvarchar(100) NOT NULL,
        [EntityId] nvarchar(50) NOT NULL,
        [ActionType] nvarchar(50) NOT NULL,
        [UserId] uniqueidentifier NULL,
        [OldValue] nvarchar(max) NULL,
        [NewValue] nvarchar(max) NULL,
        [IpAddress] nvarchar(50) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NULL,
        CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([Id])
    );
    CREATE INDEX [IX_AuditLogs_CreatedAt] ON [AuditLogs] ([CreatedAt]);
    CREATE INDEX [IX_AuditLogs_EntityName] ON [AuditLogs] ([EntityName]);
    CREATE INDEX [IX_AuditLogs_UserId] ON [AuditLogs] ([UserId]);
END
GO

-- ====================================================================================================
-- 11. REGISTER EF MIGRATION ENTRIES
-- ====================================================================================================
MERGE INTO [__EFMigrationsHistory] AS Target
USING (VALUES
    (N'20260611182501_InitialCreate', N'10.0.11'),
    (N'20260613080829_AdmissionModule', N'10.0.11'),
    (N'20260613113441_Phase5_TeacherBatches', N'10.0.11'),
    (N'20260613121108_Phase6_CoreSchemaUpdates', N'10.0.11'),
    (N'20260613122652_Phase7_InstituteUpdates', N'10.0.11'),
    (N'20260613165716_Phase8_BranchAndGstUpdates', N'10.0.11'),
    (N'20260613174112_Phase9_NullableTeacherProfileInTeacherBatch', N'10.0.11'),
    (N'20260614040642_AddStaffManagementModule', N'10.0.11'),
    (N'20260614051531_IncreaseFileTypeLength', N'10.0.11'),
    (N'20260614151553_AddCourseCategory', N'10.0.11'),
    (N'20260821171742_UpdateSchemaFinal', N'10.0.11')
) AS Source ([MigrationId], [ProductVersion])
ON Target.[MigrationId] = Source.[MigrationId]
WHEN NOT MATCHED THEN
    INSERT ([MigrationId], [ProductVersion])
    VALUES (Source.[MigrationId], Source.[ProductVersion]);
GO

-- ====================================================================================================
-- 12. INITIAL SEED DATA
-- ====================================================================================================

-- 1. Seed Roles
IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Code] = N'SUPER_ADMIN')
    INSERT INTO [Roles] ([Id], [RoleName], [Code], [Description], [IsActive], [CreatedAt], [IsDeleted])
    VALUES ('E1B00001-0000-0000-0000-000000000001', N'Super Admin', N'SUPER_ADMIN', N'System-wide Super Administrator', 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Code] = N'INSTITUTE_ADMIN')
    INSERT INTO [Roles] ([Id], [RoleName], [Code], [Description], [IsActive], [CreatedAt], [IsDeleted])
    VALUES ('E1B00001-0000-0000-0000-000000000002', N'Institute Admin', N'INSTITUTE_ADMIN', N'Admin of Coaching Institute', 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Code] = N'TEACHER')
    INSERT INTO [Roles] ([Id], [RoleName], [Code], [Description], [IsActive], [CreatedAt], [IsDeleted])
    VALUES ('E1B00001-0000-0000-0000-000000000003', N'Teacher', N'TEACHER', N'Faculty / Instructor', 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Code] = N'STUDENT')
    INSERT INTO [Roles] ([Id], [RoleName], [Code], [Description], [IsActive], [CreatedAt], [IsDeleted])
    VALUES ('E1B00001-0000-0000-0000-000000000004', N'Student', N'STUDENT', N'Enrolled Student', 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Code] = N'GLOBAL_ADMIN')
    INSERT INTO [Roles] ([Id], [RoleName], [Code], [Description], [IsActive], [CreatedAt], [IsDeleted])
    VALUES ('E1B00001-0000-0000-0000-000000000005', N'Global Admin', N'GLOBAL_ADMIN', N'Global Platform Administrator', 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Code] = N'ACCOUNTANT')
    INSERT INTO [Roles] ([Id], [RoleName], [Code], [Description], [IsActive], [CreatedAt], [IsDeleted])
    VALUES ('E1B00001-0000-0000-0000-000000000006', N'Accountant', N'ACCOUNTANT', N'Accounts & Finance Manager', 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Code] = N'RECEPTIONIST')
    INSERT INTO [Roles] ([Id], [RoleName], [Code], [Description], [IsActive], [CreatedAt], [IsDeleted])
    VALUES ('E1B00001-0000-0000-0000-000000000007', N'Receptionist', N'RECEPTIONIST', N'Front Desk / Reception', 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Code] = N'BRANCH_ADMIN')
    INSERT INTO [Roles] ([Id], [RoleName], [Code], [Description], [IsActive], [CreatedAt], [IsDeleted])
    VALUES ('E1B00001-0000-0000-0000-000000000008', N'Branch Admin', N'BRANCH_ADMIN', N'Branch Administrator', 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Code] = N'COUNSELLOR')
    INSERT INTO [Roles] ([Id], [RoleName], [Code], [Description], [IsActive], [CreatedAt], [IsDeleted])
    VALUES ('E1B00001-0000-0000-0000-000000000009', N'Counsellor', N'COUNSELLOR', N'Admission Counsellor', 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Code] = N'DATA_ENTRY_OPERATOR')
    INSERT INTO [Roles] ([Id], [RoleName], [Code], [Description], [IsActive], [CreatedAt], [IsDeleted])
    VALUES ('E1B00001-0000-0000-0000-000000000010', N'Data Entry Operator', N'DATA_ENTRY_OPERATOR', N'Data Entry Staff', 1, SYSUTCDATETIME(), 0);
GO

-- 2. Seed Default Institute / Organization
DECLARE @OrgId uniqueidentifier = 'A1A00000-0000-0000-0000-000000000001';
IF NOT EXISTS (SELECT 1 FROM [Organizations] WHERE [OrganizationId] = @OrgId)
BEGIN
    INSERT INTO [Organizations] (
        [OrganizationId], [InstituteCode], [Name], [ShortName], [Description], [ContactPersonName],
        [MobileNumber], [EmailAddress], [AddressLine1], [City], [State], [Country], [Pincode],
        [InstituteType], [EstablishedYear], [AcademicSessionStartMonth], [AcademicSessionEndMonth],
        [OwnerName], [OwnerMobile], [OwnerEmail], [PlanName], [MaxStudentsAllowed], [MaxTeachersAllowed],
        [ExpiryDate], [IsTrial], [SMSEnabled], [EmailEnabled], [WhatsAppEnabled], [Currency],
        [ReceiptPrefix], [IsActive], [CreatedAt], [IsDeleted]
    ) VALUES (
        @OrgId, N'INST001', N'Apex Coaching Academy', N'ACA', N'Apex Coaching Academy - Leading Institute for Prep',
        N'Rishabh Admin', N'9876543210', N'admin@apex.com', N'123 Education Hub', N'Delhi', N'Delhi', N'India', N'110001',
        N'Coaching Institute', 2020, N'January', N'December', N'Rishabh Owner', N'9876543211', N'owner@apex.com',
        N'Premium', 1000, 50, DATEADD(YEAR, 2, SYSUTCDATETIME()), 0, 1, 1, 0, N'INR', N'RCPT', 1, SYSUTCDATETIME(), 0
    );
END
GO

-- 3. Seed Modules and Submodules
DECLARE @M_CRM uniqueidentifier = 'B1B00000-0000-0000-0000-000000000001';
DECLARE @M_FEES uniqueidentifier = 'B1B00000-0000-0000-0000-000000000002';
DECLARE @M_ATTENDANCE uniqueidentifier = 'B1B00000-0000-0000-0000-000000000003';
DECLARE @M_LEARNING uniqueidentifier = 'B1B00000-0000-0000-0000-000000000004';
DECLARE @M_COMMUNICATION uniqueidentifier = 'B1B00000-0000-0000-0000-000000000005';
DECLARE @M_STUDENT_PORTAL uniqueidentifier = 'B1B00000-0000-0000-0000-000000000006';
DECLARE @M_TEACHER_PORTAL uniqueidentifier = 'B1B00000-0000-0000-0000-000000000007';
DECLARE @M_STAFF uniqueidentifier = 'B1B00000-0000-0000-0000-000000000008';
DECLARE @M_BRANCHES uniqueidentifier = 'B1B00000-0000-0000-0000-000000000009';

IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [ModuleCode] = N'CRM')
    INSERT INTO [Modules] ([Id], [ModuleCode], [ModuleName], [RoutePath], [Icon], [DisplayOrder], [IsDefaultEnabled], [IsMenuItem], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@M_CRM, N'CRM', N'CRM & Leads', N'/admin/crm', N'chat_bubble', 1, 1, 1, 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [ModuleCode] = N'FEES')
    INSERT INTO [Modules] ([Id], [ModuleCode], [ModuleName], [RoutePath], [Icon], [DisplayOrder], [IsDefaultEnabled], [IsMenuItem], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@M_FEES, N'FEES', N'Fee Management', N'/admin/fees', N'payments', 2, 1, 1, 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [ModuleCode] = N'ATTENDANCE')
    INSERT INTO [Modules] ([Id], [ModuleCode], [ModuleName], [RoutePath], [Icon], [DisplayOrder], [IsDefaultEnabled], [IsMenuItem], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@M_ATTENDANCE, N'ATTENDANCE', N'Attendance Tracking', N'/admin/attendance', N'how_to_reg', 3, 1, 1, 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [ModuleCode] = N'LEARNING')
    INSERT INTO [Modules] ([Id], [ModuleCode], [ModuleName], [RoutePath], [Icon], [DisplayOrder], [IsDefaultEnabled], [IsMenuItem], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@M_LEARNING, N'LEARNING', N'Learning & Academics', N'/admin/courses', N'school', 4, 1, 1, 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [ModuleCode] = N'COMMUNICATION')
    INSERT INTO [Modules] ([Id], [ModuleCode], [ModuleName], [RoutePath], [Icon], [DisplayOrder], [IsDefaultEnabled], [IsMenuItem], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@M_COMMUNICATION, N'COMMUNICATION', N'Communication', N'/admin/notices', N'campaign', 5, 1, 1, 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [ModuleCode] = N'STUDENT_PORTAL')
    INSERT INTO [Modules] ([Id], [ModuleCode], [ModuleName], [RoutePath], [Icon], [DisplayOrder], [IsDefaultEnabled], [IsMenuItem], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@M_STUDENT_PORTAL, N'STUDENT_PORTAL', N'Student Portal', N'/student/dashboard', N'person', 6, 1, 1, 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [ModuleCode] = N'TEACHER_PORTAL')
    INSERT INTO [Modules] ([Id], [ModuleCode], [ModuleName], [RoutePath], [Icon], [DisplayOrder], [IsDefaultEnabled], [IsMenuItem], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@M_TEACHER_PORTAL, N'TEACHER_PORTAL', N'Teacher Portal', N'/teacher/dashboard', N'co_present', 7, 1, 1, 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [ModuleCode] = N'STAFF')
    INSERT INTO [Modules] ([Id], [ModuleCode], [ModuleName], [RoutePath], [Icon], [DisplayOrder], [IsDefaultEnabled], [IsMenuItem], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@M_STAFF, N'STAFF', N'Staff Management', N'/admin/staff', N'people', 8, 1, 1, 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [ModuleCode] = N'BRANCHES')
    INSERT INTO [Modules] ([Id], [ModuleCode], [ModuleName], [RoutePath], [Icon], [DisplayOrder], [IsDefaultEnabled], [IsMenuItem], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@M_BRANCHES, N'BRANCHES', N'Branch Management', N'/admin/branches', N'lan', 9, 1, 1, 1, SYSUTCDATETIME(), 0);

-- Submodules
IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [ModuleCode] = N'COURSES')
    INSERT INTO [Modules] ([Id], [ParentModuleId], [ModuleCode], [ModuleName], [RoutePath], [Icon], [DisplayOrder], [IsDefaultEnabled], [IsMenuItem], [IsActive], [CreatedAt], [IsDeleted])
    VALUES ('B1B00000-0000-0000-0000-000000000010', @M_LEARNING, N'COURSES', N'Courses', N'/admin/courses', N'class', 1, 1, 1, 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [ModuleCode] = N'BATCHES')
    INSERT INTO [Modules] ([Id], [ParentModuleId], [ModuleCode], [ModuleName], [RoutePath], [Icon], [DisplayOrder], [IsDefaultEnabled], [IsMenuItem], [IsActive], [CreatedAt], [IsDeleted])
    VALUES ('B1B00000-0000-0000-0000-000000000011', @M_LEARNING, N'BATCHES', N'Batches', N'/admin/batches', N'group', 2, 1, 1, 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [ModuleCode] = N'SUBJECTS')
    INSERT INTO [Modules] ([Id], [ParentModuleId], [ModuleCode], [ModuleName], [RoutePath], [Icon], [DisplayOrder], [IsDefaultEnabled], [IsMenuItem], [IsActive], [CreatedAt], [IsDeleted])
    VALUES ('B1B00000-0000-0000-0000-000000000012', @M_LEARNING, N'SUBJECTS', N'Subjects', N'/admin/subjects', N'book', 3, 1, 0, 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [ModuleCode] = N'ASSIGNMENTS')
    INSERT INTO [Modules] ([Id], [ParentModuleId], [ModuleCode], [ModuleName], [RoutePath], [Icon], [DisplayOrder], [IsDefaultEnabled], [IsMenuItem], [IsActive], [CreatedAt], [IsDeleted])
    VALUES ('B1B00000-0000-0000-0000-000000000013', @M_LEARNING, N'ASSIGNMENTS', N'Assignments', N'/admin/assignments', N'assignment', 4, 1, 0, 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [ModuleCode] = N'TESTS')
    INSERT INTO [Modules] ([Id], [ParentModuleId], [ModuleCode], [ModuleName], [RoutePath], [Icon], [DisplayOrder], [IsDefaultEnabled], [IsMenuItem], [IsActive], [CreatedAt], [IsDeleted])
    VALUES ('B1B00000-0000-0000-0000-000000000014', @M_LEARNING, N'TESTS', N'Tests & Exams', N'/admin/tests', N'assessment', 5, 1, 0, 1, SYSUTCDATETIME(), 0);
GO

-- 4. Enable Modules for Institute
DECLARE @OrgId uniqueidentifier = 'A1A00000-0000-0000-0000-000000000001';
INSERT INTO [OrganizationModules] ([Id], [OrganizationId], [ModuleId], [IsEnabled], [CreatedAt], [IsDeleted])
SELECT NEWID(), @OrgId, m.Id, 1, SYSUTCDATETIME(), 0
FROM [Modules] m
WHERE NOT EXISTS (
    SELECT 1 FROM [OrganizationModules] om 
    WHERE om.[OrganizationId] = @OrgId AND om.[ModuleId] = m.Id
);
GO

-- 5. Seed Permissions
MERGE INTO [Permissions] AS Target
USING (VALUES
    ('CanViewStudent',     N'View Students',         N'Can view the student list'),
    ('CanCreateStudent',   N'Create Students',       N'Can add a new student'),
    ('CanEditStudent',     N'Edit Students',         N'Can edit student information'),
    ('CanDeleteStudent',   N'Delete Students',       N'Can delete a student record'),
    ('CanViewCourse',      N'View Courses',          N'Can view the course catalogue'),
    ('CanCreateCourse',    N'Create Courses',        N'Can add new courses'),
    ('CanEditCourse',      N'Edit Courses',          N'Can edit existing courses'),
    ('CanDeleteCourse',    N'Delete Courses',        N'Can delete courses'),
    ('CanViewBatch',       N'View Batches',          N'Can view batch list'),
    ('CanCreateBatch',     N'Create Batches',        N'Can create new batches'),
    ('CanEditBatch',       N'Edit Batches',          N'Can edit batch details'),
    ('CanDeleteBatch',     N'Delete Batches',        N'Can delete batches'),
    ('CanViewCRM',         N'View CRM',              N'Can view leads and enquiries'),
    ('CanCreateEnquiry',   N'Create Enquiries',      N'Can add new enquiries/leads'),
    ('CanEditEnquiry',     N'Edit Enquiries',        N'Can update enquiry status'),
    ('CanDeleteEnquiry',   N'Delete Enquiries',      N'Can delete enquiries'),
    ('CanCollectFee',      N'Collect Fees',          N'Can record student payments'),
    ('CanApproveDiscount', N'Approve Discounts',     N'Can approve fee discounts'),
    ('CanViewFees',        N'View Fees',             N'Can view fee plans'),
    ('CanCreateVacancy',   N'Create Vacancies',      N'Can post job vacancies'),
    ('CanUploadNotes',     N'Upload Notes',          N'Can upload study materials'),
    ('CanDeleteNotes',     N'Delete Notes',          N'Can delete study materials'),
    ('CanCreateTests',     N'Create Tests',          N'Can create test schedules'),
    ('CanDeleteTests',     N'Delete Tests',          N'Can delete tests'),
    ('CanPublishResults',  N'Publish Results',       N'Can enter test results'),
    ('CanExportData',      N'Export Data',           N'Can export data to Excel'),
    ('CanManageNotices',   N'Manage Notices',        N'Can create and delete notices'),
    ('CanViewAuditLog',    N'View Audit Logs',       N'Can view the audit trail'),
    ('CanManageModules',   N'Manage Modules',        N'Can enable/disable institute modules'),
    ('CanManageUsers',     N'Manage Users',          N'Can create and manage user accounts')
) AS Source ([Code], [Name], [Description])
ON Target.[Code] = Source.[Code]
WHEN NOT MATCHED THEN
    INSERT ([Id], [Code], [Name], [Description], [CreatedAt], [IsDeleted])
    VALUES (NEWID(), Source.[Code], Source.[Name], Source.[Description], SYSUTCDATETIME(), 0);
GO

-- 6. Seed Default Users (Password: Password@123)
DECLARE @OrgId uniqueidentifier = 'A1A00000-0000-0000-0000-000000000001';
DECLARE @SuperAdminRoleId uniqueidentifier = 'E1B00001-0000-0000-0000-000000000001';
DECLARE @AdminRoleId uniqueidentifier = 'E1B00001-0000-0000-0000-000000000002';
DECLARE @TeacherRoleId uniqueidentifier = 'E1B00001-0000-0000-0000-000000000003';
DECLARE @StudentRoleId uniqueidentifier = 'E1B00001-0000-0000-0000-000000000004';
DECLARE @GlobalAdminRoleId uniqueidentifier = 'E1B00001-0000-0000-0000-000000000005';
DECLARE @PasswordHash nvarchar(max) = N'mvoOD5rhMZapkxCY+cHb66EvgHwIa0j+XR9tOZv57o7YOqFMJbVPSMhdpAcqt1b8vfbAp/lcsqOgMbCkkJTjxw==';
DECLARE @PasswordSalt nvarchar(500) = N'scf470Oc8a3OaLP8fgTeyl/j0eQG/8168vbXvKh+8BCZFFO0tvDZnzJkBVUnbDSec7n6qIhLqjdm10qdmLHZ5y9ZpcbupsmE0oSWgxI8Fiu31f6WbcsBlDSh5sKE4l1tU81gUgNym9wAqRnps4rumDyDJsX/3Z187CygM3FE3qY=';

-- Super Admin
IF NOT EXISTS (SELECT 1 FROM [Users] WHERE [Email] = N'superadmin@apex.com')
    INSERT INTO [Users] ([Id], [InstituteId], [RoleId], [FullName], [Email], [MobileNumber], [Username], [PasswordHash], [PasswordSalt], [IsActive], [CreatedOn], [IsDeleted])
    VALUES ('C1C00000-0000-0000-0000-000000000001', @OrgId, @SuperAdminRoleId, N'Apex Super Admin', N'superadmin@apex.com', N'9876543210', N'superadmin@apex.com', @PasswordHash, @PasswordSalt, 1, SYSUTCDATETIME(), 0);

-- Global Admin
IF NOT EXISTS (SELECT 1 FROM [Users] WHERE [Email] = N'globaladmin@apex.com')
    INSERT INTO [Users] ([Id], [InstituteId], [RoleId], [FullName], [Email], [MobileNumber], [Username], [PasswordHash], [PasswordSalt], [IsActive], [CreatedOn], [IsDeleted])
    VALUES ('C1C00000-0000-0000-0000-000000000002', @OrgId, @GlobalAdminRoleId, N'Apex Global Admin', N'globaladmin@apex.com', N'9876543210', N'globaladmin@apex.com', @PasswordHash, @PasswordSalt, 1, SYSUTCDATETIME(), 0);

-- Institute Admin
IF NOT EXISTS (SELECT 1 FROM [Users] WHERE [Email] = N'admin@apex.com')
    INSERT INTO [Users] ([Id], [InstituteId], [RoleId], [FullName], [Email], [MobileNumber], [Username], [PasswordHash], [PasswordSalt], [IsActive], [CreatedOn], [IsDeleted])
    VALUES ('C1C00000-0000-0000-0000-000000000003', @OrgId, @AdminRoleId, N'Apex Admin', N'admin@apex.com', N'9876543210', N'admin@apex.com', @PasswordHash, @PasswordSalt, 1, SYSUTCDATETIME(), 0);

-- Teacher User
IF NOT EXISTS (SELECT 1 FROM [Users] WHERE [Email] = N'teacher@apex.com')
    INSERT INTO [Users] ([Id], [InstituteId], [RoleId], [FullName], [Email], [MobileNumber], [Username], [PasswordHash], [PasswordSalt], [IsActive], [CreatedOn], [IsDeleted])
    VALUES ('C1C00000-0000-0000-0000-000000000004', @OrgId, @TeacherRoleId, N'John Doe', N'teacher@apex.com', N'9876543210', N'teacher@apex.com', @PasswordHash, @PasswordSalt, 1, SYSUTCDATETIME(), 0);

-- Student User
IF NOT EXISTS (SELECT 1 FROM [Users] WHERE [Email] = N'student@apex.com')
    INSERT INTO [Users] ([Id], [InstituteId], [RoleId], [FullName], [Email], [MobileNumber], [Username], [PasswordHash], [PasswordSalt], [IsActive], [CreatedOn], [IsDeleted])
    VALUES ('C1C00000-0000-0000-0000-000000000005', @OrgId, @StudentRoleId, N'Jane Smith', N'student@apex.com', N'9876543212', N'student@apex.com', @PasswordHash, @PasswordSalt, 1, SYSUTCDATETIME(), 0);
GO

-- 7. Seed Student Profile
DECLARE @OrgId uniqueidentifier = 'A1A00000-0000-0000-0000-000000000001';
DECLARE @StudentId uniqueidentifier = 'C1C00000-0000-0000-0000-000000000005';
IF NOT EXISTS (SELECT 1 FROM [Students] WHERE [Id] = @StudentId)
BEGIN
    INSERT INTO [Students] (
        [Id], [InstituteId], [StudentCode], [FullName], [Email], [Mobile], [ParentName], [ParentMobile],
        [DateOfBirth], [Gender], [Address], [Status], [AdmissionDate], [CreatedAt], [IsDeleted]
    ) VALUES (
        @StudentId, @OrgId, N'STU001', N'Jane Smith', N'student@apex.com', N'9876543212', N'Mr. Smith', N'9876543200',
        '2008-05-15', N'Female', N'456 Park Avenue, Delhi, India', N'Active', '2026-01-01', SYSUTCDATETIME(), 0
    );
END
GO

-- 8. Seed Courses & Subjects
DECLARE @OrgId uniqueidentifier = 'A1A00000-0000-0000-0000-000000000001';
DECLARE @Course1Id uniqueidentifier = 'D1D00000-0000-0000-0000-000000000001';
DECLARE @Course2Id uniqueidentifier = 'D1D00000-0000-0000-0000-000000000002';

IF NOT EXISTS (SELECT 1 FROM [Courses] WHERE [Id] = @Course1Id)
    INSERT INTO [Courses] ([Id], [InstituteId], [Name], [CourseCode], [Description], [CourseCategory], [CourseType], [DurationValue], [DurationType], [TotalFees], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@Course1Id, @OrgId, N'IIT-JEE Prep', N'IIT-JEE-01', N'Comprehensive coaching for JEE Main & Advanced', N'Engineering', N'Offline', 1, N'Years', 120000, 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Courses] WHERE [Id] = @Course2Id)
    INSERT INTO [Courses] ([Id], [InstituteId], [Name], [CourseCode], [Description], [CourseCategory], [CourseType], [DurationValue], [DurationType], [TotalFees], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@Course2Id, @OrgId, N'NEET Prep', N'NEET-01', N'Medical entrance preparation course', N'Medical', N'Hybrid', 2, N'Years', 150000, 1, SYSUTCDATETIME(), 0);

-- Subjects
DECLARE @Sub1 uniqueidentifier = 'D1D00000-0000-0000-0000-000000000011';
DECLARE @Sub2 uniqueidentifier = 'D1D00000-0000-0000-0000-000000000012';
DECLARE @Sub3 uniqueidentifier = 'D1D00000-0000-0000-0000-000000000013';
DECLARE @Sub4 uniqueidentifier = 'D1D00000-0000-0000-0000-000000000014';

IF NOT EXISTS (SELECT 1 FROM [Subjects] WHERE [Id] = @Sub1)
    INSERT INTO [Subjects] ([Id], [InstituteId], [CourseId], [Name], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@Sub1, @OrgId, @Course1Id, N'Physics', 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Subjects] WHERE [Id] = @Sub2)
    INSERT INTO [Subjects] ([Id], [InstituteId], [CourseId], [Name], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@Sub2, @OrgId, @Course1Id, N'Chemistry', 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Subjects] WHERE [Id] = @Sub3)
    INSERT INTO [Subjects] ([Id], [InstituteId], [CourseId], [Name], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@Sub3, @OrgId, @Course1Id, N'Mathematics', 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Subjects] WHERE [Id] = @Sub4)
    INSERT INTO [Subjects] ([Id], [InstituteId], [CourseId], [Name], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@Sub4, @OrgId, @Course2Id, N'Biology', 1, SYSUTCDATETIME(), 0);
GO

-- 9. Seed Batches
DECLARE @OrgId uniqueidentifier = 'A1A00000-0000-0000-0000-000000000001';
DECLARE @Course1Id uniqueidentifier = 'D1D00000-0000-0000-0000-000000000001';
DECLARE @Course2Id uniqueidentifier = 'D1D00000-0000-0000-0000-000000000002';
DECLARE @Batch1Id uniqueidentifier = 'E1E00000-0000-0000-0000-000000000001';
DECLARE @Batch2Id uniqueidentifier = 'E1E00000-0000-0000-0000-000000000002';

IF NOT EXISTS (SELECT 1 FROM [Batches] WHERE [Id] = @Batch1Id)
    INSERT INTO [Batches] ([Id], [InstituteId], [CourseId], [Name], [BatchCode], [BatchStatus], [Capacity], [StartDate], [EndDate], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@Batch1Id, @OrgId, @Course1Id, N'JEE 2026 Batch A', N'JEE-2026-A', N'Active', 40, '2026-01-01', '2026-12-31', 1, SYSUTCDATETIME(), 0);

IF NOT EXISTS (SELECT 1 FROM [Batches] WHERE [Id] = @Batch2Id)
    INSERT INTO [Batches] ([Id], [InstituteId], [CourseId], [Name], [BatchCode], [BatchStatus], [Capacity], [StartDate], [EndDate], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@Batch2Id, @OrgId, @Course2Id, N'NEET 2026 Batch A', N'NEET-2026-A', N'Active', 40, '2026-01-01', '2026-12-31', 1, SYSUTCDATETIME(), 0);
GO

-- 10. Enroll Student in Batch 1
DECLARE @OrgId uniqueidentifier = 'A1A00000-0000-0000-0000-000000000001';
DECLARE @StudentId uniqueidentifier = 'C1C00000-0000-0000-0000-000000000005';
DECLARE @Batch1Id uniqueidentifier = 'E1E00000-0000-0000-0000-000000000001';

IF NOT EXISTS (SELECT 1 FROM [StudentBatches] WHERE [StudentId] = @StudentId AND [BatchId] = @Batch1Id)
BEGIN
    INSERT INTO [StudentBatches] ([Id], [InstituteId], [StudentId], [BatchId], [JoinedDate], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (NEWID(), @OrgId, @StudentId, @Batch1Id, '2026-01-01', 1, SYSUTCDATETIME(), 0);
END
GO

PRINT N'CoachOS Database, all 50+ tables, relationships, indexes, and seed data created successfully!';
GO

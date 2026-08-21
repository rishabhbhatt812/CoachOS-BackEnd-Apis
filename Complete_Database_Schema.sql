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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE TABLE [Courses] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [Description] nvarchar(max) NULL,
        [DurationInMonths] int NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE TABLE [Institutes] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [OwnerName] nvarchar(150) NOT NULL,
        [Email] nvarchar(150) NOT NULL,
        [Mobile] nvarchar(20) NOT NULL,
        [Address] nvarchar(max) NULL,
        [LogoPath] nvarchar(max) NULL,
        [ReceiptPrefix] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_Institutes] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE TABLE [Modules] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [Code] nvarchar(50) NOT NULL,
        [Description] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_Modules] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE TABLE [Roles] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [Code] nvarchar(50) NOT NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE TABLE [Subjects] (
        [Id] uniqueidentifier NOT NULL,
        [CourseId] uniqueidentifier NOT NULL,
        [Name] nvarchar(150) NOT NULL,
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
        CONSTRAINT [FK_Subjects_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE TABLE [InstituteModules] (
        [Id] uniqueidentifier NOT NULL,
        [ModuleId] uniqueidentifier NOT NULL,
        [IsEnabled] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_InstituteModules] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_InstituteModules_Institutes_InstituteId] FOREIGN KEY ([InstituteId]) REFERENCES [Institutes] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_InstituteModules_Modules_ModuleId] FOREIGN KEY ([ModuleId]) REFERENCES [Modules] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] uniqueidentifier NOT NULL,
        [RoleId] uniqueidentifier NOT NULL,
        [FullName] nvarchar(150) NOT NULL,
        [Email] nvarchar(150) NOT NULL,
        [Mobile] nvarchar(20) NULL,
        [PasswordHash] nvarchar(max) NOT NULL,
        [LastLoginAt] datetime2 NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE TABLE [Batches] (
        [Id] uniqueidentifier NOT NULL,
        [CourseId] uniqueidentifier NOT NULL,
        [TeacherUserId] uniqueidentifier NULL,
        [Name] nvarchar(150) NOT NULL,
        [StartTime] time NULL,
        [EndTime] time NULL,
        [StartDate] date NULL,
        [EndDate] date NULL,
        [DefaultFee] decimal(18,2) NULL,
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
        CONSTRAINT [PK_Batches] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Batches_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Batches_Users_TeacherUserId] FOREIGN KEY ([TeacherUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
        [FileType] nvarchar(50) NOT NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AttendanceRecords_AttendanceSessionId] ON [AttendanceRecords] ([AttendanceSessionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AttendanceRecords_StudentId] ON [AttendanceRecords] ([StudentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AttendanceSessions_AttendanceDate] ON [AttendanceSessions] ([AttendanceDate]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AttendanceSessions_BatchId] ON [AttendanceSessions] ([BatchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AttendanceSessions_TakenByUserId] ON [AttendanceSessions] ([TakenByUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Batches_CourseId] ON [Batches] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Batches_InstituteId] ON [Batches] ([InstituteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Batches_TeacherUserId] ON [Batches] ([TeacherUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Courses_InstituteId] ON [Courses] ([InstituteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DemoClasses_BatchId] ON [DemoClasses] ([BatchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DemoClasses_EnquiryId] ON [DemoClasses] ([EnquiryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Enquiries_AssignedToUserId] ON [Enquiries] ([AssignedToUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Enquiries_InstituteId] ON [Enquiries] ([InstituteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Enquiries_InterestedCourseId] ON [Enquiries] ([InterestedCourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Enquiries_Mobile] ON [Enquiries] ([Mobile]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Enquiries_Status] ON [Enquiries] ([Status]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_FeePlans_BatchId] ON [FeePlans] ([BatchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_FeePlans_CourseId] ON [FeePlans] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_FeePlans_StudentId] ON [FeePlans] ([StudentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_FollowUps_EnquiryId] ON [FollowUps] ([EnquiryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ImportJobErrors_ImportJobId] ON [ImportJobErrors] ([ImportJobId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Installments_DueDate] ON [Installments] ([DueDate]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Installments_FeePlanId] ON [Installments] ([FeePlanId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Installments_Status] ON [Installments] ([Status]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_InstituteModules_InstituteId] ON [InstituteModules] ([InstituteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_InstituteModules_ModuleId] ON [InstituteModules] ([ModuleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Modules_Code] ON [Modules] ([Code]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Notes_BatchId] ON [Notes] ([BatchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Notes_CourseId] ON [Notes] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Notes_SubjectId] ON [Notes] ([SubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Notes_UploadedByUserId] ON [Notes] ([UploadedByUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Notices_BatchId] ON [Notices] ([BatchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Notices_CourseId] ON [Notices] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Payments_FeePlanId] ON [Payments] ([FeePlanId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Payments_InstallmentId] ON [Payments] ([InstallmentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Payments_PaymentDate] ON [Payments] ([PaymentDate]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Payments_StudentId] ON [Payments] ([StudentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Permissions_Code] ON [Permissions] ([Code]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RolePermissions_PermissionId] ON [RolePermissions] ([PermissionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RolePermissions_RoleId] ON [RolePermissions] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Roles_Code] ON [Roles] ([Code]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_StudentBatches_BatchId] ON [StudentBatches] ([BatchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_StudentBatches_StudentId] ON [StudentBatches] ([StudentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Students_InstituteId] ON [Students] ([InstituteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Students_Mobile] ON [Students] ([Mobile]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Students_StudentCode] ON [Students] ([StudentCode]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Subjects_CourseId] ON [Subjects] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_TestResults_StudentId] ON [TestResults] ([StudentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_TestResults_TestId] ON [TestResults] ([TestId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Tests_BatchId] ON [Tests] ([BatchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Tests_CourseId] ON [Tests] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Tests_SubjectId] ON [Tests] ([SubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Users_Email] ON [Users] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Users_InstituteId] ON [Users] ([InstituteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Users_RoleId] ON [Users] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Vacancies_LastDate] ON [Vacancies] ([LastDate]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_VacancyCourseMappings_CourseId] ON [VacancyCourseMappings] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_VacancyCourseMappings_VacancyId] ON [VacancyCourseMappings] ([VacancyId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611182501_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260611182501_InitialCreate', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE TABLE [AuditLogs] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NULL,
        [EntityName] nvarchar(100) NOT NULL,
        [EntityId] nvarchar(50) NOT NULL,
        [ActionType] nvarchar(50) NOT NULL,
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
        CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE TABLE [Notifications] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [Message] nvarchar(1000) NOT NULL,
        [NotificationType] nvarchar(50) NOT NULL,
        [IsRead] bit NOT NULL,
        [ReadAt] datetime2 NULL,
        [LinkUrl] nvarchar(500) NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE TABLE [Parents] (
        [Id] uniqueidentifier NOT NULL,
        [FullName] nvarchar(150) NOT NULL,
        [Mobile] nvarchar(20) NOT NULL,
        [Email] nvarchar(150) NULL,
        [Occupation] nvarchar(100) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_Parents] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE TABLE [PaymentTransactions] (
        [Id] uniqueidentifier NOT NULL,
        [PaymentId] uniqueidentifier NOT NULL,
        [Gateway] nvarchar(100) NOT NULL,
        [TransactionRef] nvarchar(100) NOT NULL,
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
        CONSTRAINT [PK_PaymentTransactions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PaymentTransactions_Payments_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [Payments] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE TABLE [StudentDocuments] (
        [Id] uniqueidentifier NOT NULL,
        [StudentId] uniqueidentifier NOT NULL,
        [DocumentType] nvarchar(100) NOT NULL,
        [OriginalFileName] nvarchar(255) NOT NULL,
        [StoredFileName] nvarchar(255) NOT NULL,
        [FilePath] nvarchar(500) NOT NULL,
        [FileType] nvarchar(max) NULL,
        [FileSize] bigint NOT NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE INDEX [IX_AuditLogs_CreatedAt] ON [AuditLogs] ([CreatedAt]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE INDEX [IX_AuditLogs_EntityName] ON [AuditLogs] ([EntityName]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE INDEX [IX_AuditLogs_UserId] ON [AuditLogs] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE INDEX [IX_Notifications_IsRead] ON [Notifications] ([IsRead]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE INDEX [IX_Notifications_UserId] ON [Notifications] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE INDEX [IX_PaymentTransactions_PaymentId] ON [PaymentTransactions] ([PaymentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE INDEX [IX_StudentDocuments_StudentId] ON [StudentDocuments] ([StudentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE INDEX [IX_StudentParents_ParentId] ON [StudentParents] ([ParentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE UNIQUE INDEX [IX_StudentParents_StudentId_ParentId] ON [StudentParents] ([StudentId], [ParentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE INDEX [IX_UserPermissions_PermissionId] ON [UserPermissions] ([PermissionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    CREATE UNIQUE INDEX [IX_UserPermissions_UserId_PermissionId] ON [UserPermissions] ([UserId], [PermissionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613080829_AdmissionModule'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260613080829_AdmissionModule', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    ALTER TABLE [Batches] ADD [SubjectId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE TABLE [Assignments] (
        [Id] uniqueidentifier NOT NULL,
        [CourseId] uniqueidentifier NULL,
        [BatchId] uniqueidentifier NULL,
        [SubjectId] uniqueidentifier NULL,
        [Title] nvarchar(200) NOT NULL,
        [Description] nvarchar(max) NULL,
        [DueDate] datetime2 NOT NULL,
        [OriginalFileName] nvarchar(max) NULL,
        [StoredFileName] nvarchar(max) NULL,
        [FilePath] nvarchar(max) NULL,
        [FileType] nvarchar(max) NULL,
        [FileSizeInBytes] bigint NULL,
        [CreatedByUserId] uniqueidentifier NOT NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE TABLE [Doubts] (
        [Id] uniqueidentifier NOT NULL,
        [StudentId] uniqueidentifier NOT NULL,
        [CourseId] uniqueidentifier NULL,
        [BatchId] uniqueidentifier NULL,
        [SubjectId] uniqueidentifier NULL,
        [Title] nvarchar(200) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE TABLE [AssignmentSubmissions] (
        [Id] uniqueidentifier NOT NULL,
        [AssignmentId] uniqueidentifier NOT NULL,
        [StudentId] uniqueidentifier NOT NULL,
        [SubmittedAt] datetime2 NOT NULL,
        [OriginalFileName] nvarchar(max) NULL,
        [StoredFileName] nvarchar(max) NULL,
        [FilePath] nvarchar(max) NULL,
        [FileType] nvarchar(max) NULL,
        [FileSizeInBytes] bigint NULL,
        [StudentNotes] nvarchar(max) NULL,
        [MarksAwarded] int NULL,
        [TeacherRemarks] nvarchar(max) NULL,
        [ReviewedAt] datetime2 NULL,
        [ReviewedByUserId] uniqueidentifier NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE TABLE [DoubtReplies] (
        [Id] uniqueidentifier NOT NULL,
        [DoubtId] uniqueidentifier NOT NULL,
        [RepliedByUserId] uniqueidentifier NOT NULL,
        [ReplyMessage] nvarchar(max) NOT NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE INDEX [IX_Batches_SubjectId] ON [Batches] ([SubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE INDEX [IX_Assignments_BatchId] ON [Assignments] ([BatchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE INDEX [IX_Assignments_CourseId] ON [Assignments] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE INDEX [IX_Assignments_CreatedByUserId] ON [Assignments] ([CreatedByUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE INDEX [IX_Assignments_SubjectId] ON [Assignments] ([SubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE INDEX [IX_AssignmentSubmissions_AssignmentId] ON [AssignmentSubmissions] ([AssignmentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE INDEX [IX_AssignmentSubmissions_ReviewedByUserId] ON [AssignmentSubmissions] ([ReviewedByUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE INDEX [IX_AssignmentSubmissions_StudentId] ON [AssignmentSubmissions] ([StudentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE INDEX [IX_DoubtReplies_DoubtId] ON [DoubtReplies] ([DoubtId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE INDEX [IX_DoubtReplies_RepliedByUserId] ON [DoubtReplies] ([RepliedByUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE INDEX [IX_Doubts_BatchId] ON [Doubts] ([BatchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE INDEX [IX_Doubts_CourseId] ON [Doubts] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE INDEX [IX_Doubts_StudentId] ON [Doubts] ([StudentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE INDEX [IX_Doubts_SubjectId] ON [Doubts] ([SubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE INDEX [IX_TeacherSubjects_SubjectId] ON [TeacherSubjects] ([SubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    CREATE UNIQUE INDEX [IX_TeacherSubjects_UserId_SubjectId] ON [TeacherSubjects] ([UserId], [SubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    ALTER TABLE [Batches] ADD CONSTRAINT [FK_Batches_Subjects_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    ALTER TABLE [Notifications] ADD CONSTRAINT [FK_Notifications_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613113441_Phase5_TeacherBatches'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260613113441_Phase5_TeacherBatches', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Batches] DROP CONSTRAINT [FK_Batches_Subjects_SubjectId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Batches] DROP CONSTRAINT [FK_Batches_Users_TeacherUserId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    DROP INDEX [IX_Batches_SubjectId] ON [Batches];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    DROP INDEX [IX_Batches_TeacherUserId] ON [Batches];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Batches]') AND [c].[name] = N'DefaultFee');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Batches] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Batches] DROP COLUMN [DefaultFee];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    DECLARE @var1 nvarchar(max);
    SELECT @var1 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Batches]') AND [c].[name] = N'EndTime');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Batches] DROP CONSTRAINT ' + @var1 + ';');
    ALTER TABLE [Batches] DROP COLUMN [EndTime];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    DECLARE @var2 nvarchar(max);
    SELECT @var2 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Batches]') AND [c].[name] = N'MaxStudents');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Batches] DROP CONSTRAINT ' + @var2 + ';');
    ALTER TABLE [Batches] DROP COLUMN [MaxStudents];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    DECLARE @var3 nvarchar(max);
    SELECT @var3 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Batches]') AND [c].[name] = N'StartTime');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Batches] DROP CONSTRAINT ' + @var3 + ';');
    ALTER TABLE [Batches] DROP COLUMN [StartTime];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    DECLARE @var4 nvarchar(max);
    SELECT @var4 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Batches]') AND [c].[name] = N'SubjectId');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Batches] DROP CONSTRAINT ' + @var4 + ';');
    ALTER TABLE [Batches] DROP COLUMN [SubjectId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    EXEC sp_rename N'[Courses].[DurationInMonths]', N'MinimumStudents', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    EXEC sp_rename N'[Batches].[TeacherUserId]', N'BranchId', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Subjects] ADD [CourseId1] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Subjects] ADD [Description] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Courses] ADD [CourseCode] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Courses] ADD [CourseType] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Courses] ADD [DurationType] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Courses] ADD [DurationValue] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Courses] ADD [MaxStudents] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Courses] ADD [RegistrationFees] decimal(18,2) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Courses] ADD [TotalFees] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Batches] ADD [BatchCode] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Batches] ADD [BatchStatus] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Batches] ADD [Capacity] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Batches] ADD [Notes] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Batches] ADD [RoomNumber] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE TABLE [CourseFeeStructures] (
        [Id] uniqueidentifier NOT NULL,
        [CourseId] uniqueidentifier NOT NULL,
        [TotalFee] decimal(18,2) NOT NULL,
        [AdmissionFee] decimal(18,2) NULL,
        [MonthlyFee] decimal(18,2) NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE TABLE [TeacherProfiles] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [TeacherCode] nvarchar(max) NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [Gender] nvarchar(max) NOT NULL,
        [DateOfBirth] datetime2 NOT NULL,
        [AlternateMobile] nvarchar(max) NULL,
        [ProfilePhotoPath] nvarchar(max) NULL,
        [MaritalStatus] nvarchar(max) NULL,
        [CurrentAddress] nvarchar(max) NULL,
        [PermanentAddress] nvarchar(max) NULL,
        [City] nvarchar(max) NULL,
        [State] nvarchar(max) NULL,
        [Pincode] nvarchar(max) NULL,
        [Country] nvarchar(max) NULL,
        [EmployeeId] nvarchar(max) NOT NULL,
        [JoiningDate] datetime2 NOT NULL,
        [TeacherType] nvarchar(max) NOT NULL,
        [Department] nvarchar(max) NULL,
        [Designation] nvarchar(max) NULL,
        [SubjectExpertise] nvarchar(max) NOT NULL,
        [ExperienceYears] int NOT NULL,
        [PreviousInstituteName] nvarchar(max) NULL,
        [EmploymentStatus] nvarchar(max) NOT NULL,
        [Bio] nvarchar(max) NULL,
        [LanguagesKnown] nvarchar(max) NULL,
        [PreferredTeachingMode] nvarchar(max) NULL,
        [AvailableDays] nvarchar(max) NULL,
        [AvailableTimeSlots] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        [InstituteId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_TeacherProfiles] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TeacherProfiles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE TABLE [BatchSchedules] (
        [Id] uniqueidentifier NOT NULL,
        [BatchId] uniqueidentifier NOT NULL,
        [DayOfWeek] nvarchar(max) NOT NULL,
        [StartTime] time NOT NULL,
        [EndTime] time NOT NULL,
        [TeacherProfileId] uniqueidentifier NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE TABLE [EmergencyContacts] (
        [Id] uniqueidentifier NOT NULL,
        [TeacherProfileId] uniqueidentifier NOT NULL,
        [ContactPersonName] nvarchar(max) NOT NULL,
        [Relation] nvarchar(max) NOT NULL,
        [ContactNumber] nvarchar(max) NOT NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE TABLE [TeacherBatches] (
        [Id] uniqueidentifier NOT NULL,
        [TeacherProfileId] uniqueidentifier NOT NULL,
        [BatchId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NOT NULL,
        [AssignedOn] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE TABLE [TeacherDocuments] (
        [Id] uniqueidentifier NOT NULL,
        [TeacherProfileId] uniqueidentifier NOT NULL,
        [DocumentType] nvarchar(max) NOT NULL,
        [FileName] nvarchar(max) NOT NULL,
        [FilePath] nvarchar(max) NOT NULL,
        [UploadedOn] datetime2 NOT NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE TABLE [TeacherQualifications] (
        [Id] uniqueidentifier NOT NULL,
        [TeacherProfileId] uniqueidentifier NOT NULL,
        [Qualification] nvarchar(max) NOT NULL,
        [Specialization] nvarchar(max) NOT NULL,
        [University] nvarchar(max) NOT NULL,
        [PassingYear] int NOT NULL,
        [PercentageOrCGPA] nvarchar(max) NOT NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE TABLE [TeacherSalaries] (
        [Id] uniqueidentifier NOT NULL,
        [TeacherProfileId] uniqueidentifier NOT NULL,
        [SalaryType] nvarchar(max) NOT NULL,
        [SalaryAmount] decimal(18,2) NOT NULL,
        [BankName] nvarchar(max) NULL,
        [AccountNumber] nvarchar(max) NULL,
        [IfscCode] nvarchar(max) NULL,
        [PanNumber] nvarchar(max) NULL,
        [EffectiveFrom] datetime2 NOT NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE INDEX [IX_Subjects_CourseId1] ON [Subjects] ([CourseId1]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE INDEX [IX_BatchSchedules_BatchId] ON [BatchSchedules] ([BatchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE INDEX [IX_BatchSchedules_TeacherProfileId] ON [BatchSchedules] ([TeacherProfileId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE INDEX [IX_CourseFeeStructures_CourseId] ON [CourseFeeStructures] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE UNIQUE INDEX [IX_EmergencyContacts_TeacherProfileId] ON [EmergencyContacts] ([TeacherProfileId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE INDEX [IX_TeacherBatches_BatchId] ON [TeacherBatches] ([BatchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE INDEX [IX_TeacherBatches_SubjectId] ON [TeacherBatches] ([SubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE INDEX [IX_TeacherBatches_TeacherProfileId] ON [TeacherBatches] ([TeacherProfileId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE INDEX [IX_TeacherDocuments_TeacherProfileId] ON [TeacherDocuments] ([TeacherProfileId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE UNIQUE INDEX [IX_TeacherProfiles_UserId] ON [TeacherProfiles] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE INDEX [IX_TeacherQualifications_TeacherProfileId] ON [TeacherQualifications] ([TeacherProfileId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    CREATE UNIQUE INDEX [IX_TeacherSalaries_TeacherProfileId] ON [TeacherSalaries] ([TeacherProfileId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    ALTER TABLE [Subjects] ADD CONSTRAINT [FK_Subjects_Courses_CourseId1] FOREIGN KEY ([CourseId1]) REFERENCES [Courses] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613121108_Phase6_CoreSchemaUpdates'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260613121108_Phase6_CoreSchemaUpdates', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    DECLARE @var5 nvarchar(max);
    SELECT @var5 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Institutes]') AND [c].[name] = N'Address');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Institutes] DROP CONSTRAINT ' + @var5 + ';');
    ALTER TABLE [Institutes] DROP COLUMN [Address];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    EXEC sp_rename N'[Institutes].[Mobile]', N'Pincode', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    EXEC sp_rename N'[Institutes].[Email]', N'EmailAddress', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    DECLARE @var6 nvarchar(max);
    SELECT @var6 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Institutes]') AND [c].[name] = N'LogoPath');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Institutes] DROP CONSTRAINT ' + @var6 + ';');
    ALTER TABLE [Institutes] ALTER COLUMN [LogoPath] nvarchar(500) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [AadhaarNumber] nvarchar(50) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [AcademicSessionEndMonth] nvarchar(50) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [AcademicSessionStartMonth] nvarchar(50) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [AddressLine1] nvarchar(250) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [AddressLine2] nvarchar(250) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [AlternateMobileNumber] nvarchar(20) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [City] nvarchar(100) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [ContactPersonName] nvarchar(150) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [Country] nvarchar(100) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [Currency] nvarchar(10) NOT NULL DEFAULT N'INR';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [Description] nvarchar(1000) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [EmailEnabled] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [EstablishedYear] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [ExpiryDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [InstituteCode] nvarchar(50) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [InstituteType] nvarchar(100) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [IsTrial] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [MaxStudentsAllowed] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [MaxTeachersAllowed] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [MobileNumber] nvarchar(20) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [OwnerEmail] nvarchar(150) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [OwnerMobile] nvarchar(20) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [PANNumber] nvarchar(50) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [PlanName] nvarchar(100) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [SMSEnabled] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [ShortName] nvarchar(50) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [State] nvarchar(100) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [WebsiteUrl] nvarchar(250) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [WhatsAppEnabled] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613122652_Phase7_InstituteUpdates'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260613122652_Phase7_InstituteUpdates', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613165716_Phase8_BranchAndGstUpdates'
)
BEGIN
    ALTER TABLE [Institutes] ADD [GSTNumber] nvarchar(50) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613165716_Phase8_BranchAndGstUpdates'
)
BEGIN
    CREATE TABLE [Branches] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [Code] nvarchar(50) NOT NULL,
        [Address] nvarchar(500) NULL,
        [ContactNumber] nvarchar(50) NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613165716_Phase8_BranchAndGstUpdates'
)
BEGIN
    CREATE INDEX [IX_Batches_BranchId] ON [Batches] ([BranchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613165716_Phase8_BranchAndGstUpdates'
)
BEGIN
    CREATE INDEX [IX_Branches_InstituteId] ON [Branches] ([InstituteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613165716_Phase8_BranchAndGstUpdates'
)
BEGIN
    UPDATE Batches SET BranchId = NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613165716_Phase8_BranchAndGstUpdates'
)
BEGIN
    ALTER TABLE [Batches] ADD CONSTRAINT [FK_Batches_Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [Branches] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613165716_Phase8_BranchAndGstUpdates'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260613165716_Phase8_BranchAndGstUpdates', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613174112_Phase9_NullableTeacherProfileInTeacherBatch'
)
BEGIN
    DECLARE @var7 nvarchar(max);
    SELECT @var7 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherBatches]') AND [c].[name] = N'TeacherProfileId');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [TeacherBatches] DROP CONSTRAINT ' + @var7 + ';');
    ALTER TABLE [TeacherBatches] ALTER COLUMN [TeacherProfileId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613174112_Phase9_NullableTeacherProfileInTeacherBatch'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260613174112_Phase9_NullableTeacherProfileInTeacherBatch', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DROP INDEX [IX_Users_Email] ON [Users];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var8 nvarchar(max);
    SELECT @var8 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'AlternateMobile');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var8 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [AlternateMobile];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var9 nvarchar(max);
    SELECT @var9 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'AvailableDays');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var9 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [AvailableDays];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var10 nvarchar(max);
    SELECT @var10 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'AvailableTimeSlots');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var10 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [AvailableTimeSlots];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var11 nvarchar(max);
    SELECT @var11 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'City');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var11 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [City];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var12 nvarchar(max);
    SELECT @var12 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'Country');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var12 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [Country];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var13 nvarchar(max);
    SELECT @var13 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'CurrentAddress');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var13 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [CurrentAddress];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var14 nvarchar(max);
    SELECT @var14 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'DateOfBirth');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var14 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [DateOfBirth];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var15 nvarchar(max);
    SELECT @var15 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'Department');
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var15 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [Department];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var16 nvarchar(max);
    SELECT @var16 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'Designation');
    IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var16 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [Designation];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var17 nvarchar(max);
    SELECT @var17 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'EmployeeId');
    IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var17 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [EmployeeId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var18 nvarchar(max);
    SELECT @var18 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'EmploymentStatus');
    IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var18 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [EmploymentStatus];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var19 nvarchar(max);
    SELECT @var19 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'FirstName');
    IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var19 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [FirstName];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var20 nvarchar(max);
    SELECT @var20 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'Gender');
    IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var20 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [Gender];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var21 nvarchar(max);
    SELECT @var21 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'JoiningDate');
    IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var21 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [JoiningDate];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var22 nvarchar(max);
    SELECT @var22 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'LanguagesKnown');
    IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var22 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [LanguagesKnown];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var23 nvarchar(max);
    SELECT @var23 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'LastName');
    IF @var23 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var23 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [LastName];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var24 nvarchar(max);
    SELECT @var24 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'MaritalStatus');
    IF @var24 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var24 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [MaritalStatus];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var25 nvarchar(max);
    SELECT @var25 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'PermanentAddress');
    IF @var25 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var25 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [PermanentAddress];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var26 nvarchar(max);
    SELECT @var26 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'Pincode');
    IF @var26 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var26 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [Pincode];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var27 nvarchar(max);
    SELECT @var27 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'PreferredTeachingMode');
    IF @var27 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var27 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [PreferredTeachingMode];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var28 nvarchar(max);
    SELECT @var28 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'PreviousInstituteName');
    IF @var28 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var28 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [PreviousInstituteName];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var29 nvarchar(max);
    SELECT @var29 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'ProfilePhotoPath');
    IF @var29 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var29 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [ProfilePhotoPath];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var30 nvarchar(max);
    SELECT @var30 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'State');
    IF @var30 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var30 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [State];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var31 nvarchar(max);
    SELECT @var31 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'TeacherCode');
    IF @var31 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var31 + ';');
    ALTER TABLE [TeacherProfiles] DROP COLUMN [TeacherCode];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    EXEC sp_rename N'[Users].[UpdatedAt]', N'UpdatedOn', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    EXEC sp_rename N'[Users].[CreatedAt]', N'CreatedOn', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    EXEC sp_rename N'[Users].[Mobile]', N'MobileNumber', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    EXEC sp_rename N'[Users].[LastLoginAt]', N'LastLoginOn', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    EXEC sp_rename N'[TeacherProfiles].[UpdatedAt]', N'UpdatedOn', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    EXEC sp_rename N'[TeacherProfiles].[CreatedAt]', N'CreatedOn', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    EXEC sp_rename N'[TeacherProfiles].[ExperienceYears]', N'TeachingExperienceYears', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    EXEC sp_rename N'[Roles].[Name]', N'RoleName', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    ALTER TABLE [Users] ADD [BranchId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    ALTER TABLE [Users] ADD [IsPasswordChanged] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    ALTER TABLE [Users] ADD [PasswordSalt] nvarchar(500) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    ALTER TABLE [Users] ADD [Username] nvarchar(150) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var32 nvarchar(max);
    SELECT @var32 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'TeacherType');
    IF @var32 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var32 + ';');
    ALTER TABLE [TeacherProfiles] ALTER COLUMN [TeacherType] nvarchar(50) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var33 nvarchar(max);
    SELECT @var33 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'SubjectExpertise');
    IF @var33 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var33 + ';');
    ALTER TABLE [TeacherProfiles] ALTER COLUMN [SubjectExpertise] nvarchar(250) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    DECLARE @var34 nvarchar(max);
    SELECT @var34 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'Bio');
    IF @var34 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT ' + @var34 + ';');
    ALTER TABLE [TeacherProfiles] ALTER COLUMN [Bio] nvarchar(1000) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    ALTER TABLE [TeacherProfiles] ADD [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    ALTER TABLE [Roles] ADD [Description] nvarchar(500) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    ALTER TABLE [Roles] ADD [IsSystemRole] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    CREATE TABLE [StaffProfiles] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [StaffCode] nvarchar(50) NOT NULL,
        [StaffType] nvarchar(50) NOT NULL,
        [JoiningDate] datetime2 NOT NULL,
        [Designation] nvarchar(100) NULL,
        [Department] nvarchar(100) NULL,
        [Qualification] nvarchar(200) NULL,
        [ExperienceYears] int NOT NULL,
        [Address] nvarchar(500) NULL,
        [ProfilePhoto] nvarchar(500) NULL,
        [EmergencyContactName] nvarchar(150) NULL,
        [EmergencyContactNumber] nvarchar(20) NULL,
        [IsActive] bit NOT NULL,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    CREATE INDEX [IX_Users_BranchId] ON [Users] ([BranchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    CREATE INDEX [IX_Users_InstituteId_Email] ON [Users] ([InstituteId], [Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    CREATE INDEX [IX_Users_InstituteId_MobileNumber] ON [Users] ([InstituteId], [MobileNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    CREATE INDEX [IX_Users_InstituteId_Username] ON [Users] ([InstituteId], [Username]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    CREATE UNIQUE INDEX [IX_StaffProfiles_InstituteId_StaffCode] ON [StaffProfiles] ([InstituteId], [StaffCode]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    CREATE UNIQUE INDEX [IX_StaffProfiles_UserId] ON [StaffProfiles] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    ALTER TABLE [Users] ADD CONSTRAINT [FK_Users_Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [Branches] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614040642_AddStaffManagementModule'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260614040642_AddStaffManagementModule', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614051531_IncreaseFileTypeLength'
)
BEGIN
    DECLARE @var35 nvarchar(max);
    SELECT @var35 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Notes]') AND [c].[name] = N'FileType');
    IF @var35 IS NOT NULL EXEC(N'ALTER TABLE [Notes] DROP CONSTRAINT ' + @var35 + ';');
    ALTER TABLE [Notes] ALTER COLUMN [FileType] nvarchar(150) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614051531_IncreaseFileTypeLength'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260614051531_IncreaseFileTypeLength', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614151553_AddCourseCategory'
)
BEGIN
    ALTER TABLE [Courses] ADD [CourseCategory] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260614151553_AddCourseCategory'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260614151553_AddCourseCategory', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    DROP TABLE [InstituteModules];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    ALTER TABLE [Institutes] DROP CONSTRAINT [PK_Institutes];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    EXEC sp_rename N'[Institutes]', N'Organizations', 'OBJECT';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    EXEC sp_rename N'[Modules].[Name]', N'ModuleName', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    EXEC sp_rename N'[Modules].[Code]', N'ModuleCode', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    EXEC sp_rename N'[Modules].[IX_Modules_Code]', N'IX_Modules_ModuleCode', 'INDEX';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    EXEC sp_rename N'[Organizations].[Id]', N'OrganizationId', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    ALTER TABLE [Modules] ADD [DisplayOrder] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    ALTER TABLE [Modules] ADD [Icon] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    ALTER TABLE [Modules] ADD [IsDefaultEnabled] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    ALTER TABLE [Modules] ADD [IsMenuItem] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    ALTER TABLE [Modules] ADD [ParentModuleId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    ALTER TABLE [Modules] ADD [RoutePath] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    ALTER TABLE [Organizations] ADD CONSTRAINT [PK_Organizations] PRIMARY KEY ([OrganizationId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    CREATE TABLE [OrganizationModules] (
        [Id] uniqueidentifier NOT NULL,
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
        [OrganizationId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_OrganizationModules] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrganizationModules_Modules_ModuleId] FOREIGN KEY ([ModuleId]) REFERENCES [Modules] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_OrganizationModules_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([OrganizationId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    CREATE INDEX [IX_Modules_ParentModuleId] ON [Modules] ([ParentModuleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    CREATE INDEX [IX_OrganizationModules_ModuleId] ON [OrganizationModules] ([ModuleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    CREATE UNIQUE INDEX [IX_OrganizationModules_OrganizationId_ModuleId] ON [OrganizationModules] ([OrganizationId], [ModuleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    ALTER TABLE [Modules] ADD CONSTRAINT [FK_Modules_Modules_ParentModuleId] FOREIGN KEY ([ParentModuleId]) REFERENCES [Modules] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821171742_UpdateSchemaFinal'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260821171742_UpdateSchemaFinal', N'10.0.9');
END;

COMMIT;
GO


using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Academic;
using CoachOS.Domain.Audit;
using CoachOS.Domain.Common;
using CoachOS.Domain.Communication;
using CoachOS.Domain.CRM;
using CoachOS.Domain.Finance;
using CoachOS.Domain.Identity;
using CoachOS.Domain.ImportExport;
using CoachOS.Domain.Learning;
using CoachOS.Domain.Student;
using CoachOS.Domain.Tenancy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoachOS.Infrastructure.Data;

public class AppDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;

    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    // Tenancy & Core
    public DbSet<Institute> Institutes => Set<Institute>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
    public DbSet<TeacherSubject> TeacherSubjects => Set<TeacherSubject>();
    public DbSet<StaffProfile> StaffProfiles => Set<StaffProfile>();
    public DbSet<TeacherProfile> TeacherProfiles => Set<TeacherProfile>();
    public DbSet<TeacherQualification> TeacherQualifications => Set<TeacherQualification>();
    public DbSet<TeacherDocument> TeacherDocuments => Set<TeacherDocument>();
    public DbSet<TeacherSalary> TeacherSalaries => Set<TeacherSalary>();
    public DbSet<EmergencyContact> EmergencyContacts => Set<EmergencyContact>();
    public DbSet<TeacherBatch> TeacherBatches => Set<TeacherBatch>();
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<OrganizationModule> OrganizationModules => Set<OrganizationModule>();
    public DbSet<Branch> Branches => Set<Branch>();

    // Academic
    public DbSet<AcademicSession> AcademicSessions => Set<AcademicSession>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseFeeStructure> CourseFeeStructures => Set<CourseFeeStructure>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Batch> Batches => Set<Batch>();
    public DbSet<BatchSchedule> BatchSchedules => Set<BatchSchedule>();

    // Student
    public DbSet<Student> Students => Set<Student>();
    public DbSet<StudentBatch> StudentBatches => Set<StudentBatch>();
    public DbSet<StudentDocument> StudentDocuments => Set<StudentDocument>();

    // CRM
    public DbSet<Enquiry> Enquiries => Set<Enquiry>();
    public DbSet<FollowUp> FollowUps => Set<FollowUp>();
    public DbSet<DemoClass> DemoClasses => Set<DemoClass>();

    // Finance / Fees
    public DbSet<FeePlan> FeePlans => Set<FeePlan>();
    public DbSet<Installment> Installments => Set<Installment>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
    public DbSet<Expense> Expenses => Set<Expense>();

    // Learning / Attendance
    public DbSet<AttendanceSession> AttendanceSessions => Set<AttendanceSession>();
    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
    public DbSet<Test> Tests => Set<Test>();
    public DbSet<TestResult> TestResults => Set<TestResult>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<AssignmentSubmission> AssignmentSubmissions => Set<AssignmentSubmission>();
    public DbSet<Doubt> Doubts => Set<Doubt>();
    public DbSet<DoubtReply> DoubtReplies => Set<DoubtReply>();

    // Communication
    public DbSet<Notice> Notices => Set<Notice>();
    public DbSet<Vacancy> Vacancies => Set<Vacancy>();
    public DbSet<VacancyCourseMapping> VacancyCourseMappings => Set<VacancyCourseMapping>();

    // Import/Export
    public DbSet<ImportJob> ImportJobs => Set<ImportJob>();
    public DbSet<ImportJobError> ImportJobErrors => Set<ImportJobError>();

    // Audit
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    // Notifications
    public DbSet<Notification> Notifications => Set<Notification>();

    // Parents
    public DbSet<Parent> Parents => Set<Parent>();
    public DbSet<StudentParent> StudentParents => Set<StudentParent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Core/Tenancy Configurations ---
        modelBuilder.Entity<Institute>(entity =>
        {
            entity.ToTable("Organizations");
            entity.Property(x => x.Id).HasColumnName("OrganizationId");
            entity.Property(x => x.InstituteCode).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.ShortName).HasMaxLength(50);
            entity.Property(x => x.LogoPath); // Allows base64 data URLs & file URLs
            entity.Property(x => x.Description);
            entity.Property(x => x.WebsiteUrl);

            entity.Property(x => x.ContactPersonName).HasMaxLength(150);
            entity.Property(x => x.MobileNumber).HasMaxLength(20);
            entity.Property(x => x.AlternateMobileNumber).HasMaxLength(20);
            entity.Property(x => x.EmailAddress).HasMaxLength(150);

            entity.Property(x => x.AddressLine1).HasMaxLength(250).IsRequired();
            entity.Property(x => x.AddressLine2).HasMaxLength(250);
            entity.Property(x => x.City).HasMaxLength(100).IsRequired();
            entity.Property(x => x.State).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Country).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Pincode).HasMaxLength(20).IsRequired();

            entity.Property(x => x.InstituteType).HasMaxLength(100).IsRequired();
            entity.Property(x => x.AcademicSessionStartMonth).HasMaxLength(50).IsRequired();
            entity.Property(x => x.AcademicSessionEndMonth).HasMaxLength(50).IsRequired();

            entity.Property(x => x.OwnerName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.OwnerMobile).HasMaxLength(20).IsRequired();
            entity.Property(x => x.OwnerEmail).HasMaxLength(150);
            entity.Property(x => x.AadhaarNumber).HasMaxLength(50);
            entity.Property(x => x.PANNumber).HasMaxLength(50);
            entity.Property(x => x.GSTNumber).HasMaxLength(50);

            entity.Property(x => x.PlanName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Currency).HasMaxLength(10).HasDefaultValue("INR");

            entity.HasQueryFilter(e => !e.IsDeleted);
        });


        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(x => x.RoleName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Code).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.HasIndex(x => x.Code).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Code).HasMaxLength(50).IsRequired();
            entity.HasIndex(x => x.Code).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Permission)
                .WithMany()
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.Property(x => x.ModuleName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.ModuleCode).HasMaxLength(50).IsRequired();
            entity.HasIndex(x => x.ModuleCode).IsUnique();

            entity.HasOne(x => x.ParentModule)
                .WithMany(x => x.SubModules)
                .HasForeignKey(x => x.ParentModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<OrganizationModule>(entity =>
        {
            entity.ToTable("OrganizationModules");
            entity.Property(x => x.InstituteId).HasColumnName("OrganizationId");

            entity.HasIndex(x => new { x.InstituteId, x.ModuleId }).IsUnique();

            entity.HasOne(x => x.Institute)
                .WithMany()
                .HasForeignKey(x => x.InstituteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Module)
                .WithMany()
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(x => x.FullName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(150).IsRequired();
            entity.Property(x => x.MobileNumber).HasMaxLength(20);
            entity.Property(x => x.Username).HasMaxLength(150).IsRequired();
            entity.Property(x => x.PasswordHash).IsRequired();
            entity.Property(x => x.PasswordSalt).HasMaxLength(500);

            entity.HasIndex(x => x.InstituteId);
            entity.HasIndex(x => new { x.InstituteId, x.Email });
            entity.HasIndex(x => new { x.InstituteId, x.MobileNumber });
            entity.HasIndex(x => new { x.InstituteId, x.Username });

            entity.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Branch)
                .WithMany()
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(x => x.CreatedAt).HasColumnName("CreatedOn");
            entity.Property(x => x.UpdatedAt).HasColumnName("UpdatedOn");

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        // --- Academic Configurations ---
        modelBuilder.Entity<TeacherSubject>(entity =>
        {
            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Subject)
                .WithMany()
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => new { x.UserId, x.SubjectId }).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<StaffProfile>(entity =>
        {
            entity.Property(x => x.StaffCode).HasMaxLength(50).IsRequired();
            entity.Property(x => x.StaffType).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Designation).HasMaxLength(100);
            entity.Property(x => x.Department).HasMaxLength(100);
            entity.Property(x => x.Qualification).HasMaxLength(200);
            entity.Property(x => x.Address).HasMaxLength(500);
            entity.Property(x => x.ProfilePhoto).HasMaxLength(500);
            entity.Property(x => x.EmergencyContactName).HasMaxLength(150);
            entity.Property(x => x.EmergencyContactNumber).HasMaxLength(20);

            entity.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<StaffProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.UserId).IsUnique();
            entity.HasIndex(x => new { x.InstituteId, x.StaffCode }).IsUnique();

            entity.Property(x => x.CreatedAt).HasColumnName("CreatedOn");
            entity.Property(x => x.UpdatedAt).HasColumnName("UpdatedOn");

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<TeacherProfile>(entity =>
        {
            entity.Property(x => x.SubjectExpertise).HasMaxLength(250);
            entity.Property(x => x.TeacherType).HasMaxLength(50);
            entity.Property(x => x.Bio).HasMaxLength(1000);

            entity.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<TeacherProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.UserId).IsUnique();

            entity.Property(x => x.CreatedAt).HasColumnName("CreatedOn");
            entity.Property(x => x.UpdatedAt).HasColumnName("UpdatedOn");

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<TeacherQualification>(entity =>
        {
            entity.HasOne(x => x.TeacherProfile)
                .WithMany(x => x.Qualifications)
                .HasForeignKey(x => x.TeacherProfileId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<TeacherDocument>(entity =>
        {
            entity.HasOne(x => x.TeacherProfile)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.TeacherProfileId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<TeacherSalary>(entity =>
        {
            entity.HasOne(x => x.TeacherProfile)
                .WithOne(x => x.Salary)
                .HasForeignKey<TeacherSalary>(x => x.TeacherProfileId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<EmergencyContact>(entity =>
        {
            entity.HasOne(x => x.TeacherProfile)
                .WithOne(x => x.EmergencyContact)
                .HasForeignKey<EmergencyContact>(x => x.TeacherProfileId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<TeacherBatch>(entity =>
        {
            entity.HasOne(x => x.TeacherProfile)
                .WithMany()
                .HasForeignKey(x => x.TeacherProfileId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Batch)
                .WithMany(x => x.Teachers)
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Subject)
                .WithMany()
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<AcademicSession>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(50).IsRequired();
            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(150).IsRequired();
            entity.HasIndex(x => x.InstituteId);
            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<CourseFeeStructure>(entity =>
        {
            entity.HasOne(x => x.Course)
                .WithMany(x => x.FeeStructures)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(150).IsRequired();

            entity.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<Batch>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(150).IsRequired();

            entity.HasIndex(x => x.InstituteId);
            entity.HasIndex(x => x.CourseId);

            entity.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Branch>()
                .WithMany()
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Code).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Address).HasMaxLength(500);
            entity.Property(x => x.ContactNumber).HasMaxLength(50);
            entity.HasIndex(x => x.InstituteId);
            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<BatchSchedule>(entity =>
        {
            entity.HasOne(x => x.Batch)
                .WithMany(x => x.Schedules)
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.TeacherProfile)
                .WithMany()
                .HasForeignKey(x => x.TeacherProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        // --- Student Configurations ---
        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(x => x.StudentCode).HasMaxLength(50).IsRequired();
            entity.Property(x => x.FullName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Mobile).HasMaxLength(20);
            entity.Property(x => x.Email).HasMaxLength(150);

            entity.HasIndex(x => x.InstituteId);
            entity.HasIndex(x => x.Mobile);
            entity.HasIndex(x => x.StudentCode);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<StudentBatch>(entity =>
        {
            entity.HasIndex(x => x.StudentId);
            entity.HasIndex(x => x.BatchId);

            entity.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Batch)
                .WithMany()
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<StudentDocument>(entity =>
        {
            entity.Property(x => x.DocumentType).HasMaxLength(100).IsRequired();
            entity.Property(x => x.OriginalFileName).HasMaxLength(255).IsRequired();
            entity.Property(x => x.StoredFileName).HasMaxLength(255).IsRequired();
            entity.Property(x => x.FilePath).HasMaxLength(500).IsRequired();
            
            entity.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        // --- CRM Configurations ---
        modelBuilder.Entity<Enquiry>(entity =>
        {
            entity.Property(x => x.FullName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Mobile).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(150);

            entity.HasIndex(x => x.InstituteId);
            entity.HasIndex(x => x.Mobile);
            entity.HasIndex(x => x.Status);

            entity.HasOne(x => x.InterestedCourse)
                .WithMany()
                .HasForeignKey(x => x.InterestedCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.AssignedToUser)
                .WithMany()
                .HasForeignKey(x => x.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<FollowUp>(entity =>
        {
            entity.Property(x => x.Remark).HasMaxLength(1000).IsRequired();

            entity.HasOne(x => x.Enquiry)
                .WithMany()
                .HasForeignKey(x => x.EnquiryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<DemoClass>(entity =>
        {
            entity.Property(x => x.Status).HasMaxLength(30).IsRequired();

            entity.HasOne(x => x.Enquiry)
                .WithMany()
                .HasForeignKey(x => x.EnquiryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Batch)
                .WithMany()
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        // --- Fee Configurations ---
        modelBuilder.Entity<FeePlan>(entity =>
        {
            entity.Property(x => x.TotalFee).HasPrecision(18, 2);
            entity.Property(x => x.DiscountAmount).HasPrecision(18, 2);
            entity.Property(x => x.FinalFee).HasPrecision(18, 2);
            entity.Property(x => x.PlanType).HasMaxLength(30).IsRequired();

            entity.HasIndex(x => x.StudentId);

            entity.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Batch)
                .WithMany()
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<Installment>(entity =>
        {
            entity.Property(x => x.Amount).HasPrecision(18, 2);
            entity.Property(x => x.Status).HasMaxLength(30).IsRequired();

            entity.HasIndex(x => x.DueDate);
            entity.HasIndex(x => x.Status);

            entity.HasOne(x => x.FeePlan)
                .WithMany()
                .HasForeignKey(x => x.FeePlanId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(x => x.Amount).HasPrecision(18, 2);
            entity.Property(x => x.ReceiptNo).HasMaxLength(50).IsRequired();
            entity.Property(x => x.PaymentMode).HasMaxLength(30).IsRequired();

            entity.HasIndex(x => x.StudentId);
            entity.HasIndex(x => x.PaymentDate);

            entity.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.FeePlan)
                .WithMany()
                .HasForeignKey(x => x.FeePlanId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Installment)
                .WithMany()
                .HasForeignKey(x => x.InstallmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<PaymentTransaction>(entity =>
        {
            entity.Property(x => x.Amount).HasPrecision(18, 2);
            entity.Property(x => x.Gateway).HasMaxLength(100).IsRequired();
            entity.Property(x => x.TransactionRef).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(30).IsRequired();

            entity.HasOne(x => x.Payment)
                .WithMany()
                .HasForeignKey(x => x.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.Property(x => x.Amount).HasPrecision(18, 2);
            entity.Property(x => x.Category).HasMaxLength(100).IsRequired();

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        // --- Learning Configurations ---
        modelBuilder.Entity<AttendanceSession>(entity =>
        {
            entity.HasIndex(x => x.BatchId);
            entity.HasIndex(x => x.AttendanceDate);

            entity.HasOne(x => x.Batch)
                .WithMany()
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.TakenByUser)
                .WithMany()
                .HasForeignKey(x => x.TakenByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<AttendanceRecord>(entity =>
        {
            entity.Property(x => x.Status).HasMaxLength(20).IsRequired();

            entity.HasOne(x => x.AttendanceSession)
                .WithMany()
                .HasForeignKey(x => x.AttendanceSessionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.Property(x => x.TestName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.MaxMarks).HasPrecision(10, 2);

            entity.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Batch)
                .WithMany()
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Subject)
                .WithMany()
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.Property(x => x.MarksObtained).HasPrecision(10, 2);

            entity.HasOne(x => x.Test)
                .WithMany()
                .HasForeignKey(x => x.TestId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.OriginalFileName).HasMaxLength(255).IsRequired();
            entity.Property(x => x.StoredFileName).HasMaxLength(255).IsRequired();
            entity.Property(x => x.FilePath).HasMaxLength(500).IsRequired();
            entity.Property(x => x.FileType).HasMaxLength(150).IsRequired();

            entity.HasIndex(x => x.BatchId);

            entity.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Batch)
                .WithMany()
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Subject)
                .WithMany()
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.UploadedByUser)
                .WithMany()
                .HasForeignKey(x => x.UploadedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            
            entity.HasOne(x => x.Course).WithMany().HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Batch).WithMany().HasForeignKey(x => x.BatchId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Subject).WithMany().HasForeignKey(x => x.SubjectId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
            
            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<AssignmentSubmission>(entity =>
        {
            entity.HasOne(x => x.Assignment).WithMany(a => a.Submissions).HasForeignKey(x => x.AssignmentId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.Student).WithMany().HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.ReviewedByUser).WithMany().HasForeignKey(x => x.ReviewedByUserId).OnDelete(DeleteBehavior.Restrict);
            
            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<Doubt>(entity =>
        {
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(50).IsRequired();

            entity.HasOne(x => x.Student).WithMany().HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Course).WithMany().HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Batch).WithMany().HasForeignKey(x => x.BatchId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Subject).WithMany().HasForeignKey(x => x.SubjectId).OnDelete(DeleteBehavior.Restrict);
            
            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<DoubtReply>(entity =>
        {
            entity.HasOne(x => x.Doubt).WithMany(d => d.Replies).HasForeignKey(x => x.DoubtId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.RepliedByUser).WithMany().HasForeignKey(x => x.RepliedByUserId).OnDelete(DeleteBehavior.Restrict);
            
            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        // --- Communication Configurations ---
        modelBuilder.Entity<Notice>(entity =>
        {
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Message).HasMaxLength(1000).IsRequired();

            entity.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Batch)
                .WithMany()
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<Vacancy>(entity =>
        {
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.ExamCategory).HasMaxLength(100).IsRequired();

            entity.HasIndex(x => x.LastDate);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<VacancyCourseMapping>(entity =>
        {
            entity.HasOne(x => x.Vacancy)
                .WithMany()
                .HasForeignKey(x => x.VacancyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        // --- Import/Export Configurations ---
        modelBuilder.Entity<ImportJob>(entity =>
        {
            entity.Property(x => x.ImportType).HasMaxLength(100).IsRequired();
            entity.Property(x => x.FileName).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(30).IsRequired();

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        modelBuilder.Entity<ImportJobError>(entity =>
        {
            entity.Property(x => x.ErrorMessage).HasMaxLength(1000).IsRequired();

            entity.HasOne(x => x.ImportJob)
                .WithMany()
                .HasForeignKey(x => x.ImportJobId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        // --- UserPermission Configuration ---
        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Permission)
                .WithMany()
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => new { x.UserId, x.PermissionId }).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // --- Audit Log Configuration ---
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.Property(x => x.EntityName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.EntityId).HasMaxLength(50).IsRequired();
            entity.Property(x => x.ActionType).HasMaxLength(50).IsRequired();
            entity.Property(x => x.IpAddress).HasMaxLength(50);
            entity.HasIndex(x => x.EntityName);
            entity.HasIndex(x => x.UserId);
            entity.HasIndex(x => x.CreatedAt);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // --- Notification Configuration ---
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Message).HasMaxLength(1000).IsRequired();
            entity.Property(x => x.NotificationType).HasMaxLength(50).IsRequired();
            entity.Property(x => x.LinkUrl).HasMaxLength(500);
            entity.HasIndex(x => x.UserId);
            entity.HasIndex(x => x.IsRead);
            entity.HasQueryFilter(e => !e.IsDeleted && e.InstituteId == _currentUserService.InstituteId);
        });

        // --- Parent / StudentParent Configuration ---
        modelBuilder.Entity<Parent>(entity =>
        {
            entity.Property(x => x.FullName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Mobile).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(150);
            entity.Property(x => x.Occupation).HasMaxLength(100);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<StudentParent>(entity =>
        {
            entity.Property(x => x.RelationshipType).HasMaxLength(50).IsRequired();

            entity.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Parent)
                .WithMany()
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => new { x.StudentId, x.ParentId }).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = _currentUserService.UserId;
        var currentInstituteId = _currentUserService.InstituteId;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = currentUserId;
                    entry.Entity.IsDeleted = false;

                    if (entry.Entity is TenantBaseEntity tenantEntity)
                    {
                        if (tenantEntity.InstituteId == Guid.Empty && currentInstituteId.HasValue)
                        {
                            tenantEntity.InstituteId = currentInstituteId.Value;
                        }
                    }
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = currentUserId;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    entry.Entity.DeletedBy = currentUserId;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
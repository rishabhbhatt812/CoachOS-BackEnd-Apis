using CoachOS.Api.Middlewares;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Infrastructure.Data;
using CoachOS.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Text;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using CoachOS.Application.Validators;
using CoachOS.Application.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Often needed if using cookies or specific headers
    });
});

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
    options.Filters.Add(new CoachOS.Api.Filters.ValidationFilter());
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = Microsoft.OpenApi.ParameterLocation.Header,
        Type = Microsoft.OpenApi.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    c.AddSecurityRequirement(doc => new Microsoft.OpenApi.OpenApiSecurityRequirement
    {
        { new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer"), new System.Collections.Generic.List<string>() }
    });
});
// DbContext configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

// ADO.NET Connection Factory
builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

// Services DI
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.Configure<CoachOS.Infrastructure.Configurations.CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IModuleAccessService, ModuleAccessService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<CoachOS.Application.Interfaces.Services.IJwtTokenService, CoachOS.Infrastructure.Services.JwtTokenService>();
builder.Services.AddScoped(typeof(CoachOS.Application.Interfaces.Repositories.IRepository<>), typeof(CoachOS.Infrastructure.Repositories.Repository<>));
builder.Services.AddScoped<CoachOS.Application.Interfaces.Repositories.IUnitOfWork, CoachOS.Infrastructure.Repositories.UnitOfWork>();

// Feature Services DI
builder.Services.AddScoped<IAuthService, CoachOS.Application.Features.Auth.AuthService>();
builder.Services.AddScoped<IAcademicsService, CoachOS.Application.Features.Academics.AcademicsService>();
builder.Services.AddScoped<IStudentService, CoachOS.Application.Features.Students.StudentService>();
builder.Services.AddScoped<ICrmService, CoachOS.Application.Features.Crm.CrmService>();
builder.Services.AddScoped<IFeeService, CoachOS.Application.Features.Finance.FeeService>();
builder.Services.AddScoped<ICommunicationService, CoachOS.Application.Features.Communication.CommunicationService>();
builder.Services.AddScoped<IAttendanceService, CoachOS.Application.Features.Learning.AttendanceService>();
builder.Services.AddScoped<ILearningService, CoachOS.Application.Features.Learning.LearningService>();
builder.Services.AddScoped<IStudentPortalService, CoachOS.Application.Features.StudentPortal.StudentPortalService>();
builder.Services.AddScoped<CoachOS.Application.Interfaces.Services.ITeacherRegistrationService, CoachOS.Application.Features.Teachers.TeacherRegistrationService>();
builder.Services.AddScoped<IAdmissionsService, CoachOS.Infrastructure.Services.AdmissionsService>();
builder.Services.AddScoped<CoachOS.Application.Interfaces.Repositories.IUserRepository, CoachOS.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<CoachOS.Application.Interfaces.Repositories.IStaffProfileRepository, CoachOS.Infrastructure.Repositories.StaffProfileRepository>();
builder.Services.AddScoped<CoachOS.Application.Interfaces.Repositories.ITeacherProfileRepository, CoachOS.Infrastructure.Repositories.TeacherProfileRepository>();
builder.Services.AddScoped<IStaffService, CoachOS.Application.Features.Staff.StaffService>();

// Advanced Feature Services
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

// Mapster
builder.Services.AddMapster();

// Validation
builder.Services.AddValidators();

// Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"] ?? "ThisIsADummySecretKeyForDevelopmentOnlyPleaseChangeIt!";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "CoachOS",
        ValidAudience = jwtSettings["Audience"] ?? "CoachOSUsers",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoachOS API v1"));
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        try
        {
            context.Database.Migrate();
        }
        catch (Exception migEx)
        {
            logger.LogWarning(migEx, "Migration warning, continuing with ensure created / seeding...");
        }

        try
        {
            context.Database.ExecuteSqlRaw(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Vacancies]') AND name = 'ApplicationFee')
                BEGIN
                    ALTER TABLE [Vacancies] ADD [ApplicationFee] nvarchar(max) NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Vacancies]') AND name = 'EligibilityDetails')
                BEGIN
                    ALTER TABLE [Vacancies] ADD [EligibilityDetails] nvarchar(max) NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Vacancies]') AND name = 'NotificationPdfUrl')
                BEGIN
                    ALTER TABLE [Vacancies] ADD [NotificationPdfUrl] nvarchar(max) NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Vacancies]') AND name = 'NotificationSent')
                BEGIN
                    ALTER TABLE [Vacancies] ADD [NotificationSent] bit NOT NULL DEFAULT 0;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Vacancies]') AND name = 'SalaryRange')
                BEGIN
                    ALTER TABLE [Vacancies] ADD [SalaryRange] nvarchar(max) NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Vacancies]') AND name = 'TotalPosts')
                BEGIN
                    ALTER TABLE [Vacancies] ADD [TotalPosts] nvarchar(max) NULL;
                END;

                -- Ensure Assignments columns & nullability
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Assignments]') AND name = 'OriginalFileName')
                BEGIN
                    ALTER TABLE [Assignments] ADD [OriginalFileName] nvarchar(max) NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Assignments]') AND name = 'StoredFileName')
                BEGIN
                    ALTER TABLE [Assignments] ADD [StoredFileName] nvarchar(max) NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Assignments]') AND name = 'FilePath')
                BEGIN
                    ALTER TABLE [Assignments] ADD [FilePath] nvarchar(max) NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Assignments]') AND name = 'FileType')
                BEGIN
                    ALTER TABLE [Assignments] ADD [FileType] nvarchar(max) NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Assignments]') AND name = 'FileSizeInBytes')
                BEGIN
                    ALTER TABLE [Assignments] ADD [FileSizeInBytes] bigint NULL;
                END;

                ALTER TABLE [Assignments] ALTER COLUMN [SubjectId] uniqueidentifier NULL;
                ALTER TABLE [Assignments] ALTER COLUMN [CourseId] uniqueidentifier NULL;
                ALTER TABLE [Assignments] ALTER COLUMN [BatchId] uniqueidentifier NULL;
                ALTER TABLE [Assignments] ALTER COLUMN [MaxMarks] decimal(18,2) NULL;

                -- Ensure AssignmentSubmissions columns & nullability
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[AssignmentSubmissions]') AND name = 'OriginalFileName')
                BEGIN
                    ALTER TABLE [AssignmentSubmissions] ADD [OriginalFileName] nvarchar(max) NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[AssignmentSubmissions]') AND name = 'StoredFileName')
                BEGIN
                    ALTER TABLE [AssignmentSubmissions] ADD [StoredFileName] nvarchar(max) NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[AssignmentSubmissions]') AND name = 'FilePath')
                BEGIN
                    ALTER TABLE [AssignmentSubmissions] ADD [FilePath] nvarchar(max) NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[AssignmentSubmissions]') AND name = 'FileType')
                BEGIN
                    ALTER TABLE [AssignmentSubmissions] ADD [FileType] nvarchar(max) NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[AssignmentSubmissions]') AND name = 'FileSizeInBytes')
                BEGIN
                    ALTER TABLE [AssignmentSubmissions] ADD [FileSizeInBytes] bigint NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[AssignmentSubmissions]') AND name = 'SubmittedAt')
                BEGIN
                    ALTER TABLE [AssignmentSubmissions] ADD [SubmittedAt] datetime2 NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[AssignmentSubmissions]') AND name = 'StudentNotes')
                BEGIN
                    ALTER TABLE [AssignmentSubmissions] ADD [StudentNotes] nvarchar(max) NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[AssignmentSubmissions]') AND name = 'MarksAwarded')
                BEGIN
                    ALTER TABLE [AssignmentSubmissions] ADD [MarksAwarded] int NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[AssignmentSubmissions]') AND name = 'TeacherRemarks')
                BEGIN
                    ALTER TABLE [AssignmentSubmissions] ADD [TeacherRemarks] nvarchar(max) NULL;
                END;
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[AssignmentSubmissions]') AND name = 'ReviewedAt')
                BEGIN
                    ALTER TABLE [AssignmentSubmissions] ADD [ReviewedAt] datetime2 NULL;
                END;

                ALTER TABLE [AssignmentSubmissions] ALTER COLUMN [SubmissionDate] datetime2 NULL;
                ALTER TABLE [AssignmentSubmissions] ALTER COLUMN [AttachmentUrl] nvarchar(max) NULL;
                ALTER TABLE [AssignmentSubmissions] ALTER COLUMN [AttachmentFileName] nvarchar(max) NULL;
                ALTER TABLE [AssignmentSubmissions] ALTER COLUMN [Status] nvarchar(max) NULL;
            ");
        }
        catch (Exception schemaEx)
        {
            logger.LogWarning(schemaEx, "Schema update notice.");
        }

        CoachOS.Infrastructure.Data.DbSeeder.Seed(context);

        // Seed permissions
        var permService = services.GetRequiredService<IPermissionService>();
        permService.SeedPermissionsAsync().GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

app.Run();

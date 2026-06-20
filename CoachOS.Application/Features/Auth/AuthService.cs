using CoachOS.Application.Features.Auth.Dtos;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using CoachOS.Application.Helpers;
using CoachOS.Domain.Identity;
using CoachOS.Domain.Tenancy;
using CoachOS.Domain.Academic;
using CoachOS.Shared.Constants;
using CoachOS.Shared.Responses;
using Microsoft.AspNetCore.Identity;

namespace CoachOS.Application.Features.Auth;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public AuthService(
        IUnitOfWork unitOfWork,
        IJwtTokenService jwtTokenService,
        ICurrentUserService currentUserService,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
        _currentUserService = currentUserService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse<RegisterInstituteResponse>> RegisterInstituteAsync(RegisterInstituteRequest request)
    {
        var emailExists = await _unitOfWork.Repository<User>()
            .AnyAsync(x => x.Email == request.Email, ignoreQueryFilters: true);

        if (emailExists)
            return ApiResponse<RegisterInstituteResponse>.Fail("Email already registered.");

        var adminRole = await _unitOfWork.Repository<Role>()
            .FirstOrDefaultAsync(x => x.Code == RoleCodes.InstituteAdmin);

        if (adminRole == null)
        {
            adminRole = new Role
            {
                RoleName = "Institute Admin",
                Code = RoleCodes.InstituteAdmin
            };

            await _unitOfWork.Repository<Role>().AddAsync(adminRole);
            await _unitOfWork.SaveChangesAsync();
        }

        var institute = new Institute
        {
            InstituteCode = request.InstituteCode,
            Name = request.InstituteName,
            ShortName = request.ShortName,
            LogoPath = request.Logo,
            Description = request.Description,
            ContactPersonName = request.ContactPersonName,
            MobileNumber = request.MobileNumber,
            AlternateMobileNumber = request.AlternateMobileNumber,
            EmailAddress = request.EmailAddress,
            WebsiteUrl = request.WebsiteUrl,
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            City = request.City,
            State = request.State,
            Country = request.Country,
            Pincode = request.Pincode,
            InstituteType = request.InstituteType,
            EstablishedYear = request.EstablishedYear,
            AcademicSessionStartMonth = request.AcademicSessionStartMonth,
            AcademicSessionEndMonth = request.AcademicSessionEndMonth,
            OwnerName = request.OwnerName,
            OwnerMobile = request.OwnerMobile,
            OwnerEmail = request.OwnerEmail,
            AadhaarNumber = request.AadhaarNumber,
            PANNumber = request.PANNumber,
            GSTNumber = request.GSTNumber,
            PlanName = request.PlanName,
            MaxStudentsAllowed = request.MaxStudentsAllowed,
            MaxTeachersAllowed = request.MaxTeachersAllowed,
            ExpiryDate = request.ExpiryDate,
            IsTrial = request.IsTrial,
            SMSEnabled = request.SMSEnabled,
            EmailEnabled = request.EmailEnabled,
            WhatsAppEnabled = request.WhatsAppEnabled,
            Currency = request.Currency,
            ReceiptPrefix = "RCPT",
            IsActive = true
        };

        if (request.LogoFile != null && request.LogoFile.Length > 0)
        {
            if (request.LogoFile.Length > 2 * 1024 * 1024)
            {
                return ApiResponse<RegisterInstituteResponse>.Fail("Logo file size cannot exceed 2 MB.");
            }

            var ext = System.IO.Path.GetExtension(request.LogoFile.FileName).ToLower();
            var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!System.Linq.Enumerable.Contains(allowedExts, ext))
            {
                return ApiResponse<RegisterInstituteResponse>.Fail("Invalid logo file format. Only JPG, JPEG, PNG, and WEBP are allowed.");
            }

            var uploadDir = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", "uploads", "institutes", institute.Id.ToString(), "logo");
            if (!System.IO.Directory.Exists(uploadDir))
            {
                System.IO.Directory.CreateDirectory(uploadDir);
            }

            var cleanName = System.IO.Path.GetFileNameWithoutExtension(request.LogoFile.FileName)
                .Replace(" ", "_");
            var uniqueFileName = $"{cleanName}_{Guid.NewGuid()}{ext}";
            var fullPath = System.IO.Path.Combine(uploadDir, uniqueFileName);

            using (var fileStream = new System.IO.FileStream(fullPath, System.IO.FileMode.Create))
            {
                await request.LogoFile.CopyToAsync(fileStream);
            }

            institute.LogoPath = $"uploads/institutes/{institute.Id}/logo/{uniqueFileName}".Replace("\\", "/");
        }

        await _unitOfWork.Repository<Institute>().AddAsync(institute);

        var user = new User
        {
            InstituteId = institute.Id,
            RoleId = adminRole.Id,
            FullName = request.OwnerName,
            Email = request.Email,
            MobileNumber = request.Mobile,
            Username = request.Email
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _unitOfWork.Repository<User>().AddAsync(user);

        await _unitOfWork.SaveChangesAsync();

        // Seed all default-enabled modules for this new organization
        var allModules = await _unitOfWork.Repository<Module>().GetAllAsync();
        foreach (var module in allModules.Where(m => !m.IsDeleted && m.IsDefaultEnabled))
        {
            await _unitOfWork.Repository<OrganizationModule>().AddAsync(new OrganizationModule
            {
                InstituteId = institute.Id,
                ModuleId = module.Id,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            });
        }
        await _unitOfWork.SaveChangesAsync();

        // Create Default Branch
        var defaultBranch = new Branch
        {
            InstituteId = institute.Id,
            Code = "MAIN",
            Name = "Main Branch",
            Address = request.AddressLine1 + (string.IsNullOrWhiteSpace(request.AddressLine2) ? "" : ", " + request.AddressLine2) + $", {request.City}, {request.State}, {request.Country} - {request.Pincode}",
            ContactNumber = request.MobileNumber,
            IsActive = true
        };
        await _unitOfWork.Repository<Branch>().AddAsync(defaultBranch);
        await _unitOfWork.SaveChangesAsync();

        // Create Default Course
        var defaultCourse = new Course
        {
            InstituteId = institute.Id,
            CourseCode = "CORE01",
            Name = "General Core Program",
            Description = "General Core Course program containing initial subjects",
            CourseType = "Offline",
            DurationValue = 12,
            DurationType = "Months",
            TotalFees = 0,
            IsActive = true
        };
        await _unitOfWork.Repository<Course>().AddAsync(defaultCourse);
        await _unitOfWork.SaveChangesAsync();

        // Create Default Subjects linked to default Course
        if (request.SubjectNames != null && request.SubjectNames.Any())
        {
            foreach (var subName in request.SubjectNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var subject = new Subject
                {
                    InstituteId = institute.Id,
                    CourseId = defaultCourse.Id,
                    Name = subName,
                    IsActive = true
                };
                await _unitOfWork.Repository<Subject>().AddAsync(subject);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        var response = new RegisterInstituteResponse
        {
            InstituteId = institute.Id,
            AdminUserId = user.Id,
            InstituteName = institute.Name,
            AdminEmail = user.Email
        };

        return ApiResponse<RegisterInstituteResponse>.Ok(response, "Institute registered successfully.");
    }

    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _unitOfWork.Repository<User>()
            .FirstOrDefaultAsync(x => x.Email == request.Email && x.IsActive, ignoreQueryFilters: true);

        if (user == null)
            return ApiResponse<LoginResponse>.Fail("Invalid email or password.");

        bool isPasswordValid = false;
        if (!string.IsNullOrEmpty(user.PasswordSalt))
        {
            isPasswordValid = PasswordHelper.VerifyHash(request.Password, user.PasswordHash, user.PasswordSalt);
        }
        else
        {
            var verifyResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password
            );
            isPasswordValid = verifyResult != PasswordVerificationResult.Failed;
        }

        if (!isPasswordValid)
            return ApiResponse<LoginResponse>.Fail("Invalid email or password.");

        var role = await _unitOfWork.Repository<Role>()
            .GetByIdAsync(user.RoleId);

        if (role == null)
            return ApiResponse<LoginResponse>.Fail("User role not found.");

        user.LastLoginOn = DateTime.UtcNow;
        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync();

        var token = _jwtTokenService.GenerateToken(user, role.Code);

        string? instituteName = null;
        string? logoPath = null;
        if (user.InstituteId != Guid.Empty)
        {
            var institute = await _unitOfWork.Repository<Institute>().FirstOrDefaultAsync(i => i.Id == user.InstituteId, ignoreQueryFilters: true);
            if (institute != null)
            {
                instituteName = institute.Name;
                logoPath = institute.LogoPath;
            }
        }

        string? branchName = null;
        if (user.BranchId.HasValue)
        {
            var branch = await _unitOfWork.Repository<Branch>().FirstOrDefaultAsync(b => b.Id == user.BranchId.Value, ignoreQueryFilters: true);
            branchName = branch?.Name;
        }

        var response = new LoginResponse
        {
            UserId = user.Id,
            InstituteId = user.InstituteId,
            FullName = user.FullName,
            Email = user.Email,
            RoleCode = role.Code,
            AccessToken = token,
            ExpiresInMinutes = 60,
            IsPasswordChanged = user.IsPasswordChanged,
            InstituteName = instituteName,
            InstituteLogoUrl = GetAbsoluteUrl(logoPath),
            BranchId = user.BranchId,
            BranchName = branchName
        };

        return ApiResponse<LoginResponse>.Ok(response, "Login successful.");
    }

    private string GetAbsoluteUrl(string? relativePath)
    {
        if (string.IsNullOrEmpty(relativePath)) return "";
        if (relativePath.StartsWith("http://") || relativePath.StartsWith("https://"))
            return relativePath;

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return relativePath;

        var request = httpContext.Request;
        var baseUri = $"{request.Scheme}://{request.Host}{request.PathBase}";
        return $"{baseUri}/{relativePath.TrimStart('/')}";
    }

    public async Task<ApiResponse<CreateUserResponse>> CreateUserAsync(CreateUserRequest request)
    {
        var instituteId = _currentUserService.InstituteId;
        if (instituteId == null)
            return ApiResponse<CreateUserResponse>.Fail("Unauthorized. Institute ID not found.");

        var emailExists = await _unitOfWork.Repository<User>()
            .AnyAsync(x => x.Email == request.Email, ignoreQueryFilters: true);

        if (emailExists)
            return ApiResponse<CreateUserResponse>.Fail("Email already registered.");

        var role = await _unitOfWork.Repository<Role>()
            .FirstOrDefaultAsync(x => x.Code == request.RoleCode);

        if (role == null)
        {
            role = new Role
            {
                RoleName = request.RoleCode, // Quick fallback for name
                Code = request.RoleCode
            };
            await _unitOfWork.Repository<Role>().AddAsync(role);
            await _unitOfWork.SaveChangesAsync();
        }

        var user = new User
        {
            InstituteId = instituteId.Value,
            RoleId = role.Id,
            FullName = request.FullName,
            Email = request.Email,
            MobileNumber = request.Mobile,
            Username = request.Email
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _unitOfWork.Repository<User>().AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var response = new CreateUserResponse
        {
            UserId = user.Id,
            Email = user.Email,
            RoleCode = role.Code
        };

        return ApiResponse<CreateUserResponse>.Ok(response, "User created successfully.");
    }
}
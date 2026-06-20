using CoachOS.Application.Features.Auth;
using CoachOS.Application.Features.Auth.Dtos;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Domain.Identity;
using CoachOS.Domain.Tenancy;
using CoachOS.Infrastructure.Data;
using CoachOS.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoachOS.Api.Controllers.Common;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("register-institute")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> RegisterInstitute([FromForm] RegisterInstituteRequest request)
    {
        var response = await _authService.RegisterInstituteAsync(request);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUserProfile(
        [FromServices] ICurrentUserService currentUserService,
        [FromServices] IUnitOfWork unitOfWork)
    {
        var userId = currentUserService.UserId;
        if (userId == null)
            return Unauthorized(ApiResponse<object>.Fail("User context not found."));

        var user = await unitOfWork.Repository<User>().FirstOrDefaultAsync(u => u.Id == userId.Value, ignoreQueryFilters: true);
        if (user == null)
            return NotFound(ApiResponse<object>.Fail("User not found."));

        var role = await unitOfWork.Repository<Role>().FirstOrDefaultAsync(r => r.Id == user.RoleId, ignoreQueryFilters: true);
        var branchName = "";
        if (user.BranchId.HasValue)
        {
            var branch = await unitOfWork.Repository<Branch>().FirstOrDefaultAsync(b => b.Id == user.BranchId.Value, ignoreQueryFilters: true);
            branchName = branch?.Name ?? "";
        }

        string? instituteName = null;
        string? logoPath = null;
        if (user.InstituteId != Guid.Empty)
        {
            var institute = await unitOfWork.Repository<Institute>().FirstOrDefaultAsync(i => i.Id == user.InstituteId, ignoreQueryFilters: true);
            if (institute != null)
            {
                instituteName = institute.Name;
                logoPath = institute.LogoPath;
            }
        }

        string? profilePhotoUrl = null;
        if (role?.Code == "STUDENT")
        {
            var student = await unitOfWork.Repository<CoachOS.Domain.Student.Student>().FirstOrDefaultAsync(s => s.Id == userId.Value, ignoreQueryFilters: true);
            if (student != null)
            {
                profilePhotoUrl = GetAbsoluteUrl(student.ProfileImagePath);
            }
        }

        var response = new
        {
            userId = user.Id,
            fullName = user.FullName,
            email = user.Email,
            mobileNumber = user.MobileNumber,
            role = role?.RoleName ?? role?.Code ?? "",
            roleCode = role?.Code ?? "",
            instituteId = user.InstituteId,
            instituteName = instituteName ?? "CoachOS",
            instituteLogoUrl = GetAbsoluteUrl(logoPath),
            branchId = user.BranchId,
            branchName = branchName,
            profilePhotoUrl = profilePhotoUrl
        };

        return Ok(ApiResponse<object>.Ok(response));
    }

    private string GetAbsoluteUrl(string? relativePath)
    {
        if (string.IsNullOrEmpty(relativePath)) return "";
        if (relativePath.StartsWith("http://") || relativePath.StartsWith("https://"))
            return relativePath;

        var request = HttpContext.Request;
        var baseUri = $"{request.Scheme}://{request.Host}{request.PathBase}";
        return $"{baseUri}/{relativePath.TrimStart('/')}";
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);

        if (!response.Success)
            return Unauthorized(response);

        return Ok(response);
    }

    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        var response = await _authService.CreateUserAsync(request);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    [Authorize]
    [HttpGet("my-enabled-modules")]
    public async Task<IActionResult> GetMyEnabledModules(
        [FromServices] ICurrentUserService currentUserService,
        [FromServices] AppDbContext context)
    {
        if (currentUserService.InstituteId == null)
        {
            return Ok(ApiResponse<List<object>>.Ok(new List<object>()));
        }

        var enabledModules = await context.OrganizationModules
            .IgnoreQueryFilters()
            .Where(om => om.InstituteId == currentUserService.InstituteId && om.IsEnabled && !om.IsDeleted && !om.Module.IsDeleted)
            .Select(om => new
            {
                om.Module.Id,
                ModuleName = om.Module.ModuleName,
                ModuleCode = om.Module.ModuleCode,
                Icon = om.Module.Icon,
                RoutePath = om.Module.RoutePath,
                DisplayOrder = om.Module.DisplayOrder,
                ParentModuleId = om.Module.ParentModuleId,
                IsMenuItem = om.Module.IsMenuItem,
                IsDefaultEnabled = om.Module.IsDefaultEnabled
            })
            .OrderBy(m => m.DisplayOrder)
            .ToListAsync();

        return Ok(ApiResponse<object>.Ok(enabledModules));
    }
}
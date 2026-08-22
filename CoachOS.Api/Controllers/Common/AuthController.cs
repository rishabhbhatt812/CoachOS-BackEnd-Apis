using CoachOS.Application.Features.Auth;
using CoachOS.Application.Features.Auth.Dtos;
using CoachOS.Application.Interfaces.Services;
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
    public async Task<IActionResult> RegisterInstitute(RegisterInstituteRequest request)
    {
        var response = await _authService.RegisterInstituteAsync(request);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
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
            .Where(om => om.InstituteId == currentUserService.InstituteId && om.IsEnabled && !om.IsDeleted && om.Module != null && !om.Module.IsDeleted)
            .Select(om => new
            {
                om.Module!.Id,
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

    [Authorize]
    [HttpGet("my-institute")]
    public async Task<IActionResult> GetMyInstitute(
        [FromServices] ICurrentUserService currentUserService,
        [FromServices] AppDbContext context)
    {
        var instituteId = currentUserService.InstituteId;
        if (instituteId == null || instituteId == Guid.Empty)
        {
            return Ok(ApiResponse<object>.Ok(new
            {
                Name = "EduNex Global Platform",
                InstituteCode = "EDUNEX",
                Logo = "/logo.png",
                MobileNumber = "+91 98765 43210",
                EmailAddress = "admissions@edunex.in",
                Address = "Global Education Center"
            }));
        }

        var inst = await context.Institutes
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(i => i.Id == instituteId.Value && !i.IsDeleted);

        if (inst == null)
            return NotFound(ApiResponse<object>.Fail("Institute not found."));

        var fullAddress = string.Join(", ", new[] { inst.AddressLine1, inst.AddressLine2, inst.City, inst.State, inst.Pincode }
            .Where(s => !string.IsNullOrWhiteSpace(s)));

        return Ok(ApiResponse<object>.Ok(new
        {
            inst.Id,
            inst.InstituteCode,
            inst.Name,
            inst.ShortName,
            Logo = inst.LogoPath ?? "/logo.png",
            inst.ContactPersonName,
            inst.MobileNumber,
            inst.EmailAddress,
            inst.WebsiteUrl,
            Address = !string.IsNullOrWhiteSpace(fullAddress) ? fullAddress : "Main Campus",
            inst.City,
            inst.State,
            inst.Pincode,
            inst.PlanName
        }));
    }
}
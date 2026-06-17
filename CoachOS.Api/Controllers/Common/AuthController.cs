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
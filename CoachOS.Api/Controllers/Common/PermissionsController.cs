using CoachOS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CoachOS.Infrastructure.Data;

namespace CoachOS.Api.Controllers.Common
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary>Returns all permission codes for the currently logged-in user</summary>
        [HttpGet("my-permissions")]
        public async Task<IActionResult> GetMyPermissions()
        {
            var perms = await _permissionService.GetUserPermissionsAsync();
            return Ok(perms);
        }

        /// <summary>Returns all available permissions (for admin config screens)</summary>
        [HttpGet("all")]
        [Authorize(Roles = "GLOBAL_ADMIN,INSTITUTE_ADMIN,SUPER_ADMIN")]
        public IActionResult GetAllPermissions()
        {
            var all = CoachOS.Infrastructure.Services.PermissionService.SystemPermissions
                .Select(p => new { p.Code, p.Name, p.Description });
            return Ok(all);
        }

        /// <summary>Get permissions for a specific role</summary>
        [HttpGet("role/{roleId}")]
        [Authorize(Roles = "GLOBAL_ADMIN,INSTITUTE_ADMIN,SUPER_ADMIN")]
        public async Task<IActionResult> GetRolePermissions(Guid roleId)
        {
            var perms = await _permissionService.GetRolePermissionsAsync(roleId);
            return Ok(perms);
        }

        /// <summary>Assign a permission to a role</summary>
        [HttpPost("role/{roleId}/assign/{permissionId}")]
        [Authorize(Roles = "GLOBAL_ADMIN,INSTITUTE_ADMIN,SUPER_ADMIN")]
        public async Task<IActionResult> AssignToRole(Guid roleId, Guid permissionId)
        {
            await _permissionService.AssignPermissionToRoleAsync(roleId, permissionId);
            return Ok(new { success = true });
        }

        /// <summary>Override permission for a specific user</summary>
        [HttpPost("user/{userId}/assign/{permissionId}")]
        [Authorize(Roles = "GLOBAL_ADMIN,INSTITUTE_ADMIN,SUPER_ADMIN")]
        public async Task<IActionResult> AssignToUser(Guid userId, Guid permissionId, [FromQuery] bool isGranted = true)
        {
            await _permissionService.AssignPermissionToUserAsync(userId, permissionId, isGranted);
            return Ok(new { success = true });
        }

        /// <summary>Returns all active module codes for the current tenant/institute</summary>
        [HttpGet("my-modules")]
        public async Task<IActionResult> GetMyModules()
        {
            var currentUserService = HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();
            var context = HttpContext.RequestServices.GetRequiredService<AppDbContext>();

            if (currentUserService.InstituteId == null)
            {
                return Ok(new string[0]);
            }

            var enabledModules = await context.OrganizationModules
                .IgnoreQueryFilters()
                .Where(im => im.InstituteId == currentUserService.InstituteId && im.IsEnabled && !im.IsDeleted && im.Module != null && !im.Module.IsDeleted)
                .Select(im => im.Module!.ModuleCode)
                .ToListAsync();

            return Ok(enabledModules);
        }
    }
}

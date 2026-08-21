using CoachOS.Infrastructure.Data;
using CoachOS.Shared.Responses;
using CoachOS.Domain.Tenancy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "GLOBAL_ADMIN,SUPER_ADMIN")]
    public class GlobalAdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GlobalAdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("institutes")]
        public async Task<IActionResult> GetInstitutes()
        {
            var institutes = await _context.Institutes.IgnoreQueryFilters().ToListAsync();
            return Ok(ApiResponse<object>.Ok(institutes));
        }

        [HttpGet("institutes/{id:guid}/modules")]
        public async Task<IActionResult> GetInstituteModules(Guid id)
        {
            var allModules = await _context.Modules
                .IgnoreQueryFilters()
                .Where(m => !m.IsDeleted)
                .ToListAsync();

            var enabledModuleIds = await _context.OrganizationModules
                .IgnoreQueryFilters()
                .Where(im => im.InstituteId == id && im.IsEnabled && !im.IsDeleted)
                .Select(im => im.ModuleId)
                .ToListAsync();

            var response = allModules.Select(m => new
            {
                ModuleId = m.Id,
                Code = m.ModuleCode,
                Name = m.ModuleName,
                m.Description,
                IsEnabled = enabledModuleIds.Contains(m.Id)
            }).ToList();

            return Ok(ApiResponse<object>.Ok(response));
        }

        [HttpPost("institutes/{id:guid}/modules")]
        public async Task<IActionResult> UpdateInstituteModules(Guid id, [FromBody] System.Collections.Generic.List<Guid> enabledModuleIds)
        {
            var instituteExists = await _context.Institutes
                .IgnoreQueryFilters()
                .AnyAsync(i => i.Id == id && !i.IsDeleted);

            if (!instituteExists)
                return NotFound(ApiResponse<object>.Fail("Institute not found."));

            var allModules = await _context.Modules
                .IgnoreQueryFilters()
                .Where(m => !m.IsDeleted)
                .ToListAsync();

            var existingRelations = await _context.OrganizationModules
                .IgnoreQueryFilters()
                .Where(im => im.InstituteId == id)
                .ToListAsync();

            foreach (var module in allModules)
            {
                var isEnabled = enabledModuleIds.Contains(module.Id);
                var relation = existingRelations.FirstOrDefault(im => im.ModuleId == module.Id);

                if (relation != null)
                {
                    relation.IsEnabled = isEnabled;
                    relation.IsDeleted = false;
                    relation.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    _context.OrganizationModules.Add(new OrganizationModule
                    {
                        InstituteId = id,
                        ModuleId = module.Id,
                        IsEnabled = isEnabled,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok(ApiResponse<bool>.Ok(true, "Institute modules updated successfully."));
        }
    }
}

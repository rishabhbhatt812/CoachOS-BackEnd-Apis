using CoachOS.Infrastructure.Data;
using CoachOS.Shared.Responses;
using CoachOS.Domain.Tenancy;
using CoachOS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CoachOS.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/organizations")]
    [Authorize(Roles = "GLOBAL_ADMIN,SUPER_ADMIN")]
    public class OrganizationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public OrganizationsController(AppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        // GET: api/organizations/{id}/modules
        [HttpGet("{id:guid}/modules")]
        public async Task<IActionResult> GetOrganizationModules(Guid id)
        {
            var organizationExists = await _context.Institutes
                .IgnoreQueryFilters()
                .AnyAsync(i => i.Id == id && !i.IsDeleted);

            if (!organizationExists)
            {
                return NotFound(ApiResponse<object>.Fail("Organization not found."));
            }

            var allModules = await _context.Modules
                .IgnoreQueryFilters()
                .Where(m => !m.IsDeleted)
                .OrderBy(m => m.DisplayOrder)
                .ToListAsync();

            var enabledModuleIds = await _context.OrganizationModules
                .IgnoreQueryFilters()
                .Where(om => om.InstituteId == id && om.IsEnabled && !om.IsDeleted)
                .Select(om => om.ModuleId)
                .ToListAsync();

            var response = allModules.Select(m => new
            {
                ModuleId = m.Id,
                Code = m.ModuleCode,
                Name = m.ModuleName,
                m.Description,
                m.ParentModuleId,
                m.DisplayOrder,
                m.Icon,
                m.RoutePath,
                m.IsMenuItem,
                m.IsDefaultEnabled,
                IsEnabled = enabledModuleIds.Contains(m.Id)
            }).ToList();

            return Ok(ApiResponse<object>.Ok(response));
        }

        // PUT: api/organizations/{id}/modules
        [HttpPut("{id:guid}/modules")]
        public async Task<IActionResult> UpdateOrganizationModules(Guid id, [FromBody] UpdateOrganizationModulesRequest request)
        {
            var organizationExists = await _context.Institutes
                .IgnoreQueryFilters()
                .AnyAsync(i => i.Id == id && !i.IsDeleted);

            if (!organizationExists)
            {
                return NotFound(ApiResponse<object>.Fail("Organization not found."));
            }

            var allModules = await _context.Modules
                .IgnoreQueryFilters()
                .Where(m => !m.IsDeleted)
                .ToListAsync();

            var existingRelations = await _context.OrganizationModules
                .IgnoreQueryFilters()
                .Where(om => om.InstituteId == id)
                .ToListAsync();

            var currentUserId = _currentUserService.UserId ?? Guid.Empty;

            foreach (var module in allModules)
            {
                var isEnabled = request.EnabledModuleIds.Contains(module.Id);
                var relation = existingRelations.FirstOrDefault(om => om.ModuleId == module.Id);

                if (relation != null)
                {
                    if (relation.IsEnabled != isEnabled)
                    {
                        if (isEnabled)
                        {
                            relation.EnabledBy = currentUserId;
                            relation.EnabledOn = DateTime.UtcNow;
                        }
                        else
                        {
                            relation.DisabledBy = currentUserId;
                            relation.DisabledOn = DateTime.UtcNow;
                        }
                        relation.Remarks = request.Remarks;
                    }
                    relation.IsEnabled = isEnabled;
                    relation.IsDeleted = false;
                    relation.UpdatedAt = DateTime.UtcNow;
                    relation.UpdatedBy = currentUserId;
                }
                else
                {
                    _context.OrganizationModules.Add(new OrganizationModule
                    {
                        InstituteId = id,
                        ModuleId = module.Id,
                        IsEnabled = isEnabled,
                        EnabledBy = isEnabled ? currentUserId : null,
                        EnabledOn = isEnabled ? DateTime.UtcNow : null,
                        DisabledBy = !isEnabled ? currentUserId : null,
                        DisabledOn = !isEnabled ? DateTime.UtcNow : null,
                        Remarks = request.Remarks,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = currentUserId,
                        IsDeleted = false
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok(ApiResponse<bool>.Ok(true, "Organization modules updated successfully."));
        }
    }

    public class UpdateOrganizationModulesRequest
    {
        public List<Guid> EnabledModuleIds { get; set; } = new();
        public string? Remarks { get; set; }
    }
}

using CoachOS.Infrastructure.Data;
using CoachOS.Shared.Responses;
using CoachOS.Domain.Tenancy;
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
    [Route("api/modules")]
    [Authorize(Roles = "GLOBAL_ADMIN,SUPER_ADMIN")]
    public class ModulesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ModulesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/modules
        [HttpGet]
        public async Task<IActionResult> GetModules()
        {
            var modules = await _context.Modules
                .IgnoreQueryFilters()
                .Where(m => !m.IsDeleted)
                .OrderBy(m => m.DisplayOrder)
                .Select(m => new
                {
                    m.Id,
                    Code = m.ModuleCode,
                    Name = m.ModuleName,
                    m.Description,
                    m.Icon,
                    m.RoutePath,
                    m.DisplayOrder,
                    m.ParentModuleId,
                    m.IsMenuItem,
                    m.IsDefaultEnabled,
                    m.IsActive
                })
                .ToListAsync();

            return Ok(ApiResponse<object>.Ok(modules));
        }

        // POST: api/modules
        [HttpPost]
        public async Task<IActionResult> CreateModule([FromBody] CreateModuleRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code) || string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(ApiResponse<object>.Fail("Code and Name are required."));
            }

            var exists = await _context.Modules
                .IgnoreQueryFilters()
                .AnyAsync(m => m.ModuleCode == request.Code && !m.IsDeleted);

            if (exists)
            {
                return BadRequest(ApiResponse<object>.Fail("Module code already exists."));
            }

            var module = new Module
            {
                ModuleCode = request.Code,
                ModuleName = request.Name,
                Description = request.Description,
                Icon = request.Icon,
                RoutePath = request.RoutePath,
                DisplayOrder = request.DisplayOrder,
                ParentModuleId = request.ParentModuleId,
                IsMenuItem = request.IsMenuItem,
                IsDefaultEnabled = request.IsDefaultEnabled,
                IsActive = true
            };

            _context.Modules.Add(module);
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<object>.Ok(module, "Module created successfully."));
        }

        // PUT: api/modules
        [HttpPut]
        public async Task<IActionResult> UpdateModule([FromBody] UpdateModuleRequest request)
        {
            var module = await _context.Modules
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(m => m.Id == request.Id && !m.IsDeleted);

            if (module == null)
            {
                return NotFound(ApiResponse<object>.Fail("Module not found."));
            }

            if (module.ModuleCode != request.Code)
            {
                var exists = await _context.Modules
                    .IgnoreQueryFilters()
                    .AnyAsync(m => m.ModuleCode == request.Code && !m.IsDeleted);

                if (exists)
                {
                    return BadRequest(ApiResponse<object>.Fail("Module code already exists."));
                }
            }

            module.ModuleCode = request.Code;
            module.ModuleName = request.Name;
            module.Description = request.Description;
            module.Icon = request.Icon;
            module.RoutePath = request.RoutePath;
            module.DisplayOrder = request.DisplayOrder;
            module.ParentModuleId = request.ParentModuleId;
            module.IsMenuItem = request.IsMenuItem;
            module.IsDefaultEnabled = request.IsDefaultEnabled;
            module.IsActive = request.IsActive;

            await _context.SaveChangesAsync();

            return Ok(ApiResponse<object>.Ok(module, "Module updated successfully."));
        }
    }

    public class CreateModuleRequest
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? RoutePath { get; set; }
        public int DisplayOrder { get; set; }
        public Guid? ParentModuleId { get; set; }
        public bool IsMenuItem { get; set; }
        public bool IsDefaultEnabled { get; set; }
    }

    public class UpdateModuleRequest
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? RoutePath { get; set; }
        public int DisplayOrder { get; set; }
        public Guid? ParentModuleId { get; set; }
        public bool IsMenuItem { get; set; }
        public bool IsDefaultEnabled { get; set; }
        public bool IsActive { get; set; }
    }
}

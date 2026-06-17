using CoachOS.Application.Interfaces.Services;
using CoachOS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CoachOS.Infrastructure.Services
{
    public class ModuleAccessService : IModuleAccessService
    {
        private readonly AppDbContext _context;

        public ModuleAccessService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsModuleEnabledAsync(Guid instituteId, string moduleCode)
        {
            var isEnabled = await _context.OrganizationModules
                .IgnoreQueryFilters()
                .AnyAsync(im => im.InstituteId == instituteId 
                                && im.Module!.ModuleCode == moduleCode 
                                && im.IsEnabled 
                                && !im.IsDeleted 
                                && !im.Module.IsDeleted);

            return isEnabled;
        }
    }
}

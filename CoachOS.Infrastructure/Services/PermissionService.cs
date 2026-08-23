using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Identity;
using CoachOS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _db;
        private readonly ICurrentUserService _currentUser;

        // All system permissions — seeded on startup
        public static readonly List<(string Code, string Name, string Description)> SystemPermissions = new()
        {
            ("CanViewStudent",     "View Students",         "Can view the student list"),
            ("CanCreateStudent",   "Create Students",       "Can add a new student"),
            ("CanEditStudent",     "Edit Students",         "Can edit student information"),
            ("CanDeleteStudent",   "Delete Students",       "Can delete a student record"),
            ("CanViewCourse",      "View Courses",          "Can view the course catalogue"),
            ("CanCreateCourse",    "Create Courses",        "Can add new courses"),
            ("CanEditCourse",      "Edit Courses",          "Can edit existing courses"),
            ("CanDeleteCourse",    "Delete Courses",        "Can delete courses"),
            ("CanViewBatch",       "View Batches",          "Can view batch list"),
            ("CanCreateBatch",     "Create Batches",        "Can create new batches"),
            ("CanEditBatch",       "Edit Batches",          "Can edit batch details"),
            ("CanDeleteBatch",     "Delete Batches",        "Can delete batches"),
            ("CanViewCRM",         "View CRM",              "Can view leads and enquiries"),
            ("CanCreateEnquiry",   "Create Enquiries",      "Can add new enquiries/leads"),
            ("CanEditEnquiry",     "Edit Enquiries",        "Can update enquiry status"),
            ("CanDeleteEnquiry",   "Delete Enquiries",      "Can delete enquiries"),
            ("CanCollectFee",      "Collect Fees",          "Can record student payments"),
            ("CanApproveDiscount", "Approve Discounts",     "Can approve fee discounts"),
            ("CanViewFees",        "View Fees",             "Can view fee plans"),
            ("CanCreateVacancy",   "Create Vacancies",      "Can post job vacancies"),
            ("CanUploadNotes",     "Upload Notes",          "Can upload study materials"),
            ("CanDeleteNotes",     "Delete Notes",          "Can delete study materials"),
            ("CanCreateTests",     "Create Tests",          "Can create test schedules"),
            ("CanDeleteTests",     "Delete Tests",          "Can delete tests"),
            ("CanPublishResults",  "Publish Results",       "Can enter test results"),
            ("CanExportData",      "Export Data",           "Can export data to Excel"),
            ("CanManageNotices",   "Manage Notices",        "Can create and delete notices"),
            ("CanViewAuditLog",    "View Audit Logs",       "Can view the audit trail"),
            ("CanManageModules",   "Manage Modules",        "Can enable/disable institute modules"),
            ("CanManageUsers",     "Manage Users",          "Can create and manage user accounts"),
        };

        public PermissionService(AppDbContext db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<List<string>> GetUserPermissionsAsync()
        {
            if (!_currentUser.UserId.HasValue) return new List<string>();

            var user = await _db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == _currentUser.UserId.Value);

            if (user == null) return new List<string>();

            // Global Admin and Super Admin automatically have all system permissions
            if (user.Role?.Code == "GLOBAL_ADMIN" || user.Role?.Code == "SUPER_ADMIN" || _currentUser.RoleCode == "GLOBAL_ADMIN" || _currentUser.RoleCode == "SUPER_ADMIN")
            {
                return SystemPermissions.Select(p => p.Code).ToList();
            }

            // Role-level permissions
            var rolePerms = await _db.RolePermissions
                .Where(rp => rp.RoleId == user.RoleId)
                .Include(rp => rp.Permission)
                .Select(rp => new { rp.Permission!.Code, Granted = true })
                .ToListAsync();

            // User-level overrides
            var userPerms = await _db.UserPermissions
                .Where(up => up.UserId == user.Id)
                .Include(up => up.Permission)
                .Select(up => new { up.Permission!.Code, up.IsGranted })
                .ToListAsync();

            // Merge: user overrides take priority
            var permMap = rolePerms.ToDictionary(p => p.Code, p => p.Granted);
            foreach (var up in userPerms)
                permMap[up.Code] = up.IsGranted;

            return permMap.Where(kv => kv.Value).Select(kv => kv.Key).ToList();
        }

        public async Task<bool> HasPermissionAsync(string permissionCode)
        {
            var perms = await GetUserPermissionsAsync();
            return perms.Contains(permissionCode);
        }

        public async Task SeedPermissionsAsync()
        {
            foreach (var (code, name, desc) in SystemPermissions)
            {
                var exists = await _db.Permissions.AnyAsync(p => p.Code == code);
                if (!exists)
                {
                    _db.Permissions.Add(new Permission { Code = code, Name = name, Description = desc });
                }
            }
            await _db.SaveChangesAsync();
        }

        public async Task<List<string>> GetRolePermissionsAsync(Guid roleId)
        {
            return await _db.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission!.Code)
                .ToListAsync();
        }

        public async Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId)
        {
            var exists = await _db.RolePermissions.AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
            if (!exists)
            {
                _db.RolePermissions.Add(new RolePermission { RoleId = roleId, PermissionId = permissionId });
                await _db.SaveChangesAsync();
            }
        }

        public async Task AssignPermissionToUserAsync(Guid userId, Guid permissionId, bool isGranted)
        {
            var existing = await _db.UserPermissions
                .FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permissionId);

            if (existing != null)
            {
                existing.IsGranted = isGranted;
            }
            else
            {
                _db.UserPermissions.Add(new UserPermission { UserId = userId, PermissionId = permissionId, IsGranted = isGranted });
            }
            await _db.SaveChangesAsync();
        }
    }
}

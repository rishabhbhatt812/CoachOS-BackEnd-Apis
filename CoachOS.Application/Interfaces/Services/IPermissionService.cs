using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IPermissionService
    {
        /// <summary>Returns all permission codes for the current user (role + user-level overrides)</summary>
        Task<List<string>> GetUserPermissionsAsync();

        /// <summary>Checks if the current user has a specific permission code</summary>
        Task<bool> HasPermissionAsync(string permissionCode);

        /// <summary>Seeds default permissions into DB if they don't exist</summary>
        Task SeedPermissionsAsync();

        /// <summary>Gets permissions for a role</summary>
        Task<List<string>> GetRolePermissionsAsync(Guid roleId);

        /// <summary>Assigns a permission to a role</summary>
        Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId);

        /// <summary>Assigns/overrides permission to a specific user</summary>
        Task AssignPermissionToUserAsync(Guid userId, Guid permissionId, bool isGranted);
    }
}

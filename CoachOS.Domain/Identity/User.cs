using CoachOS.Domain.Common;
using CoachOS.Domain.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoachOS.Domain.Identity
{
    public class User : TenantBaseEntity
    {
        public Guid RoleId { get; set; }
        public Guid? BranchId { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? MobileNumber { get; set; }
        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;

        public DateTime? LastLoginOn { get; set; }
        public bool IsPasswordChanged { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public Role? Role { get; set; }
        public Branch? Branch { get; set; }
    }
}


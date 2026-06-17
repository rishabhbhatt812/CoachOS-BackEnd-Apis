using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Tenancy
{
    public class Branch : TenantBaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

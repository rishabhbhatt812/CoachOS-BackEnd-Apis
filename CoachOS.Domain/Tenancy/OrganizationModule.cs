using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Tenancy
{
    public class OrganizationModule : TenantBaseEntity
    {
        public Guid ModuleId { get; set; }
        public bool IsEnabled { get; set; } = true;

        public Guid? EnabledBy { get; set; }
        public DateTime? EnabledOn { get; set; }
        public Guid? DisabledBy { get; set; }
        public DateTime? DisabledOn { get; set; }
        public string? Remarks { get; set; }

        public Module? Module { get; set; }
        public Institute? Institute { get; set; }
    }
}

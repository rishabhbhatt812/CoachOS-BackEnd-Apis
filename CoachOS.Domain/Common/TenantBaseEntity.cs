using System;

namespace CoachOS.Domain.Common
{
    public abstract class TenantBaseEntity : BaseEntity
    {
        public Guid InstituteId { get; set; }
    }
}

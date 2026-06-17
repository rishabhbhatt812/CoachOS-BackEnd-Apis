using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Academic
{
    public class AcademicSession : TenantBaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Identity
{
    public class EmergencyContact : TenantBaseEntity
    {
        public Guid TeacherProfileId { get; set; }
        public TeacherProfile? TeacherProfile { get; set; }

        public string ContactPersonName { get; set; } = string.Empty;
        public string Relation { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
    }
}

using CoachOS.Domain.Common;
using CoachOS.Domain.Academic;
using System;

namespace CoachOS.Domain.Identity
{
    public class TeacherBatch : TenantBaseEntity
    {
        public Guid? TeacherProfileId { get; set; }
        public Guid BatchId { get; set; }
        public Guid SubjectId { get; set; }

        public DateTime AssignedOn { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public TeacherProfile? TeacherProfile { get; set; }
        public Batch? Batch { get; set; }
        public Subject? Subject { get; set; }
    }
}

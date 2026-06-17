using CoachOS.Domain.Common;
using CoachOS.Domain.Academic;
using System;

namespace CoachOS.Domain.Identity
{
    public class TeacherSubject : TenantBaseEntity
    {
        public Guid UserId { get; set; }
        public Guid SubjectId { get; set; }

        public User? User { get; set; }
        public Subject? Subject { get; set; }
    }
}

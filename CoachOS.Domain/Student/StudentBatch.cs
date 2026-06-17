using CoachOS.Domain.Common;
using CoachOS.Domain.Academic;
using System;

namespace CoachOS.Domain.Student
{
    public class StudentBatch : TenantBaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid BatchId { get; set; }
        public DateOnly JoinedDate { get; set; }
        public DateOnly? LeftDate { get; set; }
        public bool IsActive { get; set; } = true;

        public Student? Student { get; set; }
        public Batch? Batch { get; set; }
    }
}

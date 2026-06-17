using CoachOS.Domain.Common;
using CoachOS.Domain.Academic;
using System;

namespace CoachOS.Domain.Learning
{
    public class Test : TenantBaseEntity
    {
        public Guid CourseId { get; set; }
        public Guid BatchId { get; set; }
        public Guid? SubjectId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public DateOnly TestDate { get; set; }
        public decimal MaxMarks { get; set; }

        public Course? Course { get; set; }
        public Batch? Batch { get; set; }
        public Subject? Subject { get; set; }
    }
}

using CoachOS.Domain.Common;
using CoachOS.Domain.Student;
using System;

namespace CoachOS.Domain.Learning
{
    public class TestResult : TenantBaseEntity
    {
        public Guid TestId { get; set; }
        public Guid StudentId { get; set; }
        public decimal MarksObtained { get; set; }
        public string? Remark { get; set; }

        public Test? Test { get; set; }
        public CoachOS.Domain.Student.Student? Student { get; set; }
    }
}

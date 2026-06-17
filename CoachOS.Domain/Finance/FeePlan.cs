using CoachOS.Domain.Common;
using CoachOS.Domain.Student;
using CoachOS.Domain.Academic;
using System;

namespace CoachOS.Domain.Finance
{
    public class FeePlan : TenantBaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public Guid? BatchId { get; set; }
        public decimal TotalFee { get; set; }
        public decimal DiscountAmount { get; set; } = 0;
        public decimal FinalFee { get; set; }
        public string PlanType { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public CoachOS.Domain.Student.Student? Student { get; set; }
        public Course? Course { get; set; }
        public Batch? Batch { get; set; }
    }
}

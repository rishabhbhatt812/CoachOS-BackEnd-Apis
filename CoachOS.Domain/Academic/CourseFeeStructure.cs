using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Academic
{
    public class CourseFeeStructure : TenantBaseEntity
    {
        public Guid CourseId { get; set; }
        public Course? Course { get; set; }

        public decimal TotalFee { get; set; }
        public decimal? AdmissionFee { get; set; }
        public decimal? MonthlyFee { get; set; }
        public int? InstallmentCount { get; set; }
        public bool DiscountAllowed { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}

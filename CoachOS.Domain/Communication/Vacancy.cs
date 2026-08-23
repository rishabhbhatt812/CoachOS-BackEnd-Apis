using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Communication
{
    public class Vacancy : TenantBaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string ExamCategory { get; set; } = string.Empty;
        public string? QualificationRequired { get; set; }
        public string? AgeLimit { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly LastDate { get; set; }
        public string? OfficialLink { get; set; }
        public string? Description { get; set; }
        public string? TotalPosts { get; set; }
        public string? SalaryRange { get; set; }
        public string? ApplicationFee { get; set; }
        public string? NotificationPdfUrl { get; set; }
        public string? EligibilityDetails { get; set; }
        public bool NotificationSent { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}

using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Identity
{
    public class TeacherSalary : TenantBaseEntity
    {
        public Guid TeacherProfileId { get; set; }
        public TeacherProfile? TeacherProfile { get; set; }

        public string SalaryType { get; set; } = string.Empty; // Monthly, Hourly
        public decimal SalaryAmount { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? IfscCode { get; set; }
        public string? PanNumber { get; set; }
        public DateTime EffectiveFrom { get; set; }
    }
}

using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Identity
{
    public class StaffProfile : TenantBaseEntity
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public string StaffCode { get; set; } = string.Empty;
        public string StaffType { get; set; } = string.Empty; // Full-Time, Part-Time, Contract, etc.
        public DateTime JoiningDate { get; set; }
        public string? Designation { get; set; }
        public string? Department { get; set; }
        public string? Qualification { get; set; }
        public int ExperienceYears { get; set; }
        public string? Address { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactNumber { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

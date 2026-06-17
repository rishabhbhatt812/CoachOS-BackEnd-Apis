using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Student
{
    public class Student : TenantBaseEntity
    {
        public string StudentCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Mobile { get; set; }
        public string? ParentName { get; set; }
        public string? ParentMobile { get; set; }
        public string? Email { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Qualification { get; set; }
        public string? ProfileImagePath { get; set; }
        public string Status { get; set; } = "Active";
        public DateOnly AdmissionDate { get; set; }
    }
}

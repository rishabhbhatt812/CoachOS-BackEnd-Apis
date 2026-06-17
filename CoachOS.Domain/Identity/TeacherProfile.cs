using CoachOS.Domain.Common;
using System;
using System.Collections.Generic;

namespace CoachOS.Domain.Identity
{
    public class TeacherProfile : TenantBaseEntity
    {
        public Guid UserId { get; set; } // Links to User entity
        public User? User { get; set; }

        public string SubjectExpertise { get; set; } = string.Empty;
        public string TeacherType { get; set; } = string.Empty; // Full Time, Part Time, Guest Faculty
        public int TeachingExperienceYears { get; set; }
        public string? Bio { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public ICollection<TeacherQualification> Qualifications { get; set; } = new List<TeacherQualification>();
        public ICollection<TeacherDocument> Documents { get; set; } = new List<TeacherDocument>();
        public TeacherSalary? Salary { get; set; }
        public EmergencyContact? EmergencyContact { get; set; }
    }
}



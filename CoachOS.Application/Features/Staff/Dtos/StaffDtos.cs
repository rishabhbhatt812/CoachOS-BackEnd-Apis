using System;

namespace CoachOS.Application.Features.Staff.Dtos
{
    public class CreateStaffRequest
    {
        public Guid? InstituteId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public Guid RoleId { get; set; }
        public Guid BranchId { get; set; }

        // Common Staff Profile fields
        public string? StaffCode { get; set; }
        public string StaffType { get; set; } = string.Empty; // Full-Time, Part-Time, etc.
        public DateTime JoiningDate { get; set; } = DateTime.UtcNow.Date;
        public string? Designation { get; set; }
        public string? Department { get; set; }
        public string? Qualification { get; set; }
        public int ExperienceYears { get; set; }
        public string? Address { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactNumber { get; set; }

        // Teacher specific fields (Only used if selected role is Teacher)
        public string? SubjectExpertise { get; set; }
        public string? TeacherType { get; set; }
        public int TeachingExperienceYears { get; set; }
        public string? Bio { get; set; }
    }

    public class UpdateStaffRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public Guid RoleId { get; set; }
        public Guid BranchId { get; set; }
        public bool IsActive { get; set; }

        // Common Staff Profile fields
        public string StaffType { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
        public string? Designation { get; set; }
        public string? Department { get; set; }
        public string? Qualification { get; set; }
        public int ExperienceYears { get; set; }
        public string? Address { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactNumber { get; set; }

        // Teacher specific fields
        public string? SubjectExpertise { get; set; }
        public string? TeacherType { get; set; }
        public int TeachingExperienceYears { get; set; }
        public string? Bio { get; set; }
    }

    public class UpdateTeacherProfileRequest
    {
        public string? SubjectExpertise { get; set; }
        public string? TeacherType { get; set; }
        public int TeachingExperienceYears { get; set; }
        public string? Bio { get; set; }
    }

    public class StaffResponse
    {
        public Guid Id { get; set; } // UserId
        public Guid InstituteId { get; set; }
        public string? InstituteName { get; set; }
        public string? InstituteCode { get; set; }
        public Guid? BranchId { get; set; }
        public string? BranchName { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? MobileNumber { get; set; }
        public string Username { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string RoleCode { get; set; } = string.Empty;
        public bool IsPasswordChanged { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public StaffProfileResponse? StaffProfile { get; set; }
        public TeacherProfileResponse? TeacherProfile { get; set; }
    }

    public class StaffProfileResponse
    {
        public Guid Id { get; set; }
        public string StaffCode { get; set; } = string.Empty;
        public string StaffType { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
        public string? Designation { get; set; }
        public string? Department { get; set; }
        public string? Qualification { get; set; }
        public int ExperienceYears { get; set; }
        public string? Address { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactNumber { get; set; }
        public bool IsActive { get; set; }
    }

    public class TeacherProfileResponse
    {
        public Guid Id { get; set; }
        public string? SubjectExpertise { get; set; }
        public string? TeacherType { get; set; }
        public int TeachingExperienceYears { get; set; }
        public string? Bio { get; set; }
        public bool IsActive { get; set; }
    }

    public class TeacherResponse
    {
        public Guid Id { get; set; } // UserId
        public Guid InstituteId { get; set; }
        public string? InstituteName { get; set; }
        public string? InstituteCode { get; set; }
        public Guid? BranchId { get; set; }
        public string? BranchName { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? MobileNumber { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        
        public StaffProfileResponse? StaffProfile { get; set; }
        public TeacherProfileResponse? TeacherProfile { get; set; }
    }

    public class RoleDto
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}

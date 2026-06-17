using CoachOS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface ITeacherRegistrationService
    {
        Task<ApiResponse<Guid>> RegisterTeacherAsync(RegisterTeacherDto request);
        Task<ApiResponse<TeacherProfileDto>> GetTeacherProfileAsync(Guid teacherProfileId);
    }

    public class RegisterTeacherDto
    {
        // Basic Info
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
        public string AlternateMobileNumber { get; set; } = string.Empty;
        public string MaritalStatus { get; set; } = string.Empty;
        public string ProfilePhotoPath { get; set; } = string.Empty;

        // Address
        public string CurrentAddress { get; set; } = string.Empty;
        public string PermanentAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        // Professional
        public DateOnly? JoiningDate { get; set; }
        public string TeacherType { get; set; } = string.Empty; // Full Time, Part Time
        public string Department { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public string PreviousInstituteName { get; set; } = string.Empty;

        // Qualifications
        public List<TeacherQualificationDto> Qualifications { get; set; } = new List<TeacherQualificationDto>();

        // Subjects (TeacherSubjects)
        public List<Guid> SubjectIds { get; set; } = new List<Guid>();

        // Documents
        public List<TeacherDocumentDto> Documents { get; set; } = new List<TeacherDocumentDto>();
    }

    public class TeacherQualificationDto
    {
        public string Qualification { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string University { get; set; } = string.Empty;
        public int PassingYear { get; set; }
        public string PercentageOrCGPA { get; set; } = string.Empty;
        public string CertificateFilePath { get; set; } = string.Empty;
    }

    public class TeacherDocumentDto
    {
        public string DocumentType { get; set; } = string.Empty; // Aadhaar, PAN, Resume
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }
    
    public class TeacherProfileDto 
    {
        public Guid Id { get; set; }
        public string TeacherCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
        // ... more properties omitted for brevity
    }
}

using System;

namespace CoachOS.Application.Features.Students.Dtos
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public Guid InstituteId { get; set; }
        public string? InstituteName { get; set; }
        public string? InstituteCode { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime AdmissionDate { get; set; }
    }

    public class CreateStudentRequest
    {
        public Guid? InstituteId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime AdmissionDate { get; set; }
    }

    public class UpdateStudentRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}

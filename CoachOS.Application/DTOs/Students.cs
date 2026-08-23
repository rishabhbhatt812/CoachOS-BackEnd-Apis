using System;

namespace CoachOS.Application.DTOs
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public Guid InstituteId { get; set; }
        public string? InstituteName { get; set; }
        public string? InstituteCode { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? ParentName { get; set; }
        public string? ParentMobile { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Qualification { get; set; }
        public string? ProfileImagePath { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime AdmissionDate { get; set; }
    }

    public class StudentCreateRequest
    {
        public Guid? InstituteId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? ParentName { get; set; }
        public string? ParentMobile { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime AdmissionDate { get; set; }
    }

    public class StudentBatchDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid BatchId { get; set; }
        public DateTime JoinedDate { get; set; }
        public DateTime? LeftDate { get; set; }
        public bool IsActive { get; set; }
    }
}

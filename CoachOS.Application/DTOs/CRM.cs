using System;

namespace CoachOS.Application.DTOs
{
    public class EnquiryDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }
        public Guid? InterestedCourseId { get; set; }
        public string? Source { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? NextFollowUpDate { get; set; }
        public string? Remark { get; set; }
        public Guid? AssignedToUserId { get; set; }
    }

    public class EnquiryCreateRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }
        public Guid? InterestedCourseId { get; set; }
        public string? Source { get; set; }
        public string? Remark { get; set; }
    }

    public class FollowUpDto
    {
        public Guid Id { get; set; }
        public Guid EnquiryId { get; set; }
        public DateTime FollowUpDate { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public string? Remark { get; set; }
    }

    public class DemoClassDto
    {
        public Guid Id { get; set; }
        public Guid EnquiryId { get; set; }
        public Guid BatchId { get; set; }
        public DateTime DemoDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Remark { get; set; }
    }
}

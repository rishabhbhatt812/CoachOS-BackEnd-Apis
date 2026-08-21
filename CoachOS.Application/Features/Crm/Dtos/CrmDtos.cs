using System;

namespace CoachOS.Application.Features.Crm.Dtos
{
    public class EnquiryDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PreviousSchoolOrCollege { get; set; }
        public string? Source { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid? InterestedCourseId { get; set; }
        public string InterestedCourseName { get; set; } = string.Empty;
        public Guid? AssignedToUserId { get; set; }
    }

    public class CreateEnquiryRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PreviousSchoolOrCollege { get; set; }
        public string? Source { get; set; }
        public Guid? InterestedCourseId { get; set; }
        public Guid? AssignedToUserId { get; set; }
    }

    public class UpdateEnquiryRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PreviousSchoolOrCollege { get; set; }
        public string? Source { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid? InterestedCourseId { get; set; }
        public Guid? AssignedToUserId { get; set; }
    }

    public class CreateFollowUpRequest
    {
        public Guid EnquiryId { get; set; }
        public DateTime FollowUpDate { get; set; } = DateTime.UtcNow;
        public DateOnly? NextFollowUpDate { get; set; }
        public string Remark { get; set; } = string.Empty;
    }

    public class FollowUpDto
    {
        public Guid Id { get; set; }
        public Guid EnquiryId { get; set; }
        public DateTime FollowUpDate { get; set; }
        public DateOnly? NextFollowUpDate { get; set; }
        public string Remark { get; set; } = string.Empty;
    }

    public class ScheduleDemoClassRequest
    {
        public Guid EnquiryId { get; set; }
        public Guid? BatchId { get; set; }
        public DateTime DemoDate { get; set; }
        public string? Remark { get; set; }
    }

    public class DemoClassDto
    {
        public Guid Id { get; set; }
        public Guid EnquiryId { get; set; }
        public Guid? BatchId { get; set; }
        public string BatchName { get; set; } = string.Empty;
        public DateTime DemoDate { get; set; }
        public string Status { get; set; } = "Scheduled";
        public string? Remark { get; set; }
    }
}

using CoachOS.Domain.Common;
using CoachOS.Domain.Academic;
using CoachOS.Domain.Identity;
using System;

namespace CoachOS.Domain.CRM
{
    public class Enquiry : TenantBaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }
        public Guid? InterestedCourseId { get; set; }
        public string? Source { get; set; }
        public string Status { get; set; } = "New";
        public DateOnly? NextFollowUpDate { get; set; }
        public string? Remark { get; set; }
        public Guid? AssignedToUserId { get; set; }

        public Course? InterestedCourse { get; set; }
        public User? AssignedToUser { get; set; }
    }
}

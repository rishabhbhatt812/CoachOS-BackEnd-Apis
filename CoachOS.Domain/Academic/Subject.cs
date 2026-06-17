using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Academic
{
    public class Subject : TenantBaseEntity
    {
        public Guid CourseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public Course? Course { get; set; }
    }
}

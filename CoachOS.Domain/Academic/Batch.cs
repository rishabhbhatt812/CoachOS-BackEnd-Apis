using CoachOS.Domain.Common;
using CoachOS.Domain.Identity;
using System;

namespace CoachOS.Domain.Academic
{
    public class Batch : TenantBaseEntity
    {
        public string BatchCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty; // Batch Name
        public Guid CourseId { get; set; }
        public Guid? BranchId { get; set; }
        
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        
        public int Capacity { get; set; }
        public string BatchStatus { get; set; } = "Upcoming"; // Upcoming, Running, Completed, Cancelled
        public string? RoomNumber { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;

        public Course? Course { get; set; }
        
        public ICollection<BatchSchedule> Schedules { get; set; } = new List<BatchSchedule>();
        public ICollection<TeacherBatch> Teachers { get; set; } = new List<TeacherBatch>();
    }
}

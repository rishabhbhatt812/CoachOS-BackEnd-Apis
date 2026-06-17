using CoachOS.Domain.Common;
using CoachOS.Domain.Identity;
using System;

namespace CoachOS.Domain.Academic
{
    public class BatchSchedule : TenantBaseEntity
    {
        public Guid BatchId { get; set; }
        public Batch? Batch { get; set; }

        public string DayOfWeek { get; set; } = string.Empty; // Monday, Tuesday
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        
        public Guid? TeacherProfileId { get; set; }
        public TeacherProfile? TeacherProfile { get; set; }

        public string? RoomNumber { get; set; }
    }
}

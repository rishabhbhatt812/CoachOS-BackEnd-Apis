using CoachOS.Domain.Common;
using CoachOS.Domain.Student;
using System;

namespace CoachOS.Domain.Learning
{
    public class AttendanceRecord : TenantBaseEntity
    {
        public Guid AttendanceSessionId { get; set; }
        public Guid StudentId { get; set; }
        public string Status { get; set; } = "Present";
        public string? Remark { get; set; }

        public AttendanceSession? AttendanceSession { get; set; }
        public CoachOS.Domain.Student.Student? Student { get; set; }
    }
}

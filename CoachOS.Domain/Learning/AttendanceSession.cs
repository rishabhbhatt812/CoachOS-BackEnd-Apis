using CoachOS.Domain.Common;
using CoachOS.Domain.Academic;
using CoachOS.Domain.Identity;
using System;

namespace CoachOS.Domain.Learning
{
    public class AttendanceSession : TenantBaseEntity
    {
        public Guid BatchId { get; set; }
        public DateOnly AttendanceDate { get; set; }
        public Guid TakenByUserId { get; set; }

        public Batch? Batch { get; set; }
        public User? TakenByUser { get; set; }
    }
}

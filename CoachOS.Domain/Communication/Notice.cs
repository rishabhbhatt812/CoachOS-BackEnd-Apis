using CoachOS.Domain.Common;
using CoachOS.Domain.Academic;
using System;

namespace CoachOS.Domain.Communication
{
    public class Notice : TenantBaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string TargetType { get; set; } = "All";
        public Guid? CourseId { get; set; }
        public Guid? BatchId { get; set; }
        public bool IsActive { get; set; } = true;

        public Course? Course { get; set; }
        public Batch? Batch { get; set; }
    }
}

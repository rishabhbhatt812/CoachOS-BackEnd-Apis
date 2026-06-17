using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Audit
{
    public class AuditLog : BaseEntity
    {
        public Guid? UserId { get; set; }
        public Guid? InstituteId { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty; // Created, Updated, Deleted
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string? IpAddress { get; set; }
    }
}

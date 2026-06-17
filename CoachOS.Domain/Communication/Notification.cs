using CoachOS.Domain.Common;
using CoachOS.Domain.Identity;
using System;

namespace CoachOS.Domain.Communication
{
    public class Notification : TenantBaseEntity
    {
        public Guid UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string NotificationType { get; set; } = "SYSTEM";
        public string? LinkUrl { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime? ReadAt { get; set; }

        public User? User { get; set; }
    }
}

using CoachOS.Domain.Common;
using CoachOS.Domain.Identity;
using System;

namespace CoachOS.Domain.Learning
{
    public class DoubtReply : TenantBaseEntity
    {
        public Guid DoubtId { get; set; }
        public Guid RepliedByUserId { get; set; }
        public string ReplyMessage { get; set; } = string.Empty;
        
        public Doubt? Doubt { get; set; }
        public User? RepliedByUser { get; set; }
    }
}

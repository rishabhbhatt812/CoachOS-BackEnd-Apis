using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Identity
{
    public class UserPermission : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid PermissionId { get; set; }
        public bool IsGranted { get; set; } = true; // false = explicitly revoked

        public User? User { get; set; }
        public Permission? Permission { get; set; }
    }
}

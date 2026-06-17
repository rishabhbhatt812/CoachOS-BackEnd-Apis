using CoachOS.Domain.Common;

namespace CoachOS.Domain.Identity
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsSystemRole { get; set; } = true;
        public bool IsActive { get; set; } = true;
    }
}


using CoachOS.Domain.Common;

namespace CoachOS.Domain.Identity
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}

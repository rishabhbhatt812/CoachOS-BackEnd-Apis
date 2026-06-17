using CoachOS.Domain.Common;

namespace CoachOS.Domain.Student
{
    public class Parent : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Occupation { get; set; }
    }
}

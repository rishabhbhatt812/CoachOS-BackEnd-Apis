using CoachOS.Domain.Common;

namespace CoachOS.Domain.Academic
{
    public class Course : TenantBaseEntity
    {
        public string CourseCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string CourseCategory { get; set; } = string.Empty;
        public string CourseType { get; set; } = "Offline"; // Online, Offline, Hybrid
        public int DurationValue { get; set; }
        public string DurationType { get; set; } = "Months"; // Days, Months, Years
        public decimal TotalFees { get; set; }
        public decimal? RegistrationFees { get; set; }
        public int? MaxStudents { get; set; }
        public int? MinimumStudents { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
        public ICollection<CourseFeeStructure> FeeStructures { get; set; } = new List<CourseFeeStructure>();
    }
}

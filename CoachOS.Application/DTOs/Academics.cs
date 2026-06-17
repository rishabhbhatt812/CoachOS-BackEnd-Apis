using System;

namespace CoachOS.Application.DTOs
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? DurationInMonths { get; set; }
        public bool IsActive { get; set; }
    }

    public class CourseCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? DurationInMonths { get; set; }
    }

    public class SubjectDto
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class BatchDto
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid TeacherUserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal DefaultFee { get; set; }
        public int MaxStudents { get; set; }
        public bool IsActive { get; set; }
    }
}

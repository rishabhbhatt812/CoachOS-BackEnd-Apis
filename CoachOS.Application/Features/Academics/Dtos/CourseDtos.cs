using System;
using System.Collections.Generic;

namespace CoachOS.Application.Features.Academics.Dtos
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseCategory { get; set; } = string.Empty;
        public string CourseType { get; set; } = string.Empty;
        public int DurationValue { get; set; }
        public string DurationType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<SubjectDto> Subjects { get; set; } = new();
    }

    public class CreateCourseRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseCategory { get; set; } = string.Empty;
        public string CourseType { get; set; } = string.Empty;
        public int DurationValue { get; set; }
        public string DurationType { get; set; } = string.Empty;
        public List<string>? SubjectNames { get; set; } = new();
    }

    public class UpdateCourseRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseCategory { get; set; } = string.Empty;
        public string CourseType { get; set; } = string.Empty;
        public int DurationValue { get; set; }
        public string DurationType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<string>? SubjectNames { get; set; } = new();
    }
}

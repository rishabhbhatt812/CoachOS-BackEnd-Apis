using System;

namespace CoachOS.Application.Features.Academics.Dtos
{
    public class SubjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
    }

    public class CreateSubjectRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
    }

    public class UpdateSubjectRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
    }
}

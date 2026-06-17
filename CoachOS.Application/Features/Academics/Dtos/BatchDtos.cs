using System;

namespace CoachOS.Application.Features.Academics.Dtos
{
    public class BatchDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public Guid? SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public Guid? TeacherUserId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public List<Guid> SubjectIds { get; set; } = new List<Guid>();
        public List<string> SubjectNames { get; set; } = new List<string>();
        public List<Guid> TeacherUserIds { get; set; } = new List<Guid>();
        public List<string> TeacherNames { get; set; } = new List<string>();
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal? DefaultFee { get; set; }
        public string BatchCode { get; set; } = string.Empty;
        public Guid? BranchId { get; set; }
        public int Capacity { get; set; }
        public string BatchStatus { get; set; } = "Upcoming";
        public string? RoomNumber { get; set; }
    }

    public class CreateBatchRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? TeacherUserId { get; set; }
        public List<Guid> SubjectIds { get; set; } = new List<Guid>();
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal? DefaultFee { get; set; }
        public string BatchCode { get; set; } = string.Empty;
        public Guid? BranchId { get; set; }
        public int Capacity { get; set; }
        public string BatchStatus { get; set; } = "Upcoming";
        public string? RoomNumber { get; set; }
    }

    public class UpdateBatchRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? TeacherUserId { get; set; }
        public List<Guid> SubjectIds { get; set; } = new List<Guid>();
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal? DefaultFee { get; set; }
        public string BatchCode { get; set; } = string.Empty;
        public Guid? BranchId { get; set; }
        public int Capacity { get; set; }
        public string BatchStatus { get; set; } = "Upcoming";
        public string? RoomNumber { get; set; }
    }
}

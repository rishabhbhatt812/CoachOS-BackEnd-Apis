using System;

namespace CoachOS.Application.DTOs
{
    public class AttendanceSessionDto
    {
        public Guid Id { get; set; }
        public Guid BatchId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public Guid TakenByUserId { get; set; }
    }

    public class AttendanceRecordDto
    {
        public Guid Id { get; set; }
        public Guid AttendanceSessionId { get; set; }
        public Guid StudentId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Remark { get; set; }
    }

    public class NoteDto
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid BatchId { get; set; }
        public Guid SubjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSizeInBytes { get; set; }
    }

    public class TestDto
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid BatchId { get; set; }
        public Guid SubjectId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public DateTime TestDate { get; set; }
        public int MaxMarks { get; set; }
    }
}

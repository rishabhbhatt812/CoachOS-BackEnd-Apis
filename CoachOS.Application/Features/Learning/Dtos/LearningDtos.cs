using System;

namespace CoachOS.Application.Features.Learning.Dtos
{
    public class AttendanceSessionDto
    {
        public Guid Id { get; set; }
        public Guid BatchId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public Guid TakenByUserId { get; set; }
        public string BatchName { get; set; } = string.Empty;
        public string TakenByName { get; set; } = string.Empty;
        public int PresentCount { get; set; }
        public int TotalStudents { get; set; }
    }

    public class StudentAttendanceItemDto
    {
        public Guid StudentId { get; set; }
        public string? RollNo { get; set; }
        public string? Name { get; set; }
        public bool IsPresent { get; set; } = true;
        public string? Status { get; set; } = "Present";
        public string? Remarks { get; set; }
    }

    public class CreateAttendanceSessionRequest
    {
        public Guid BatchId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public Guid TakenByUserId { get; set; }
        public System.Collections.Generic.List<StudentAttendanceItemDto>? Students { get; set; }
    }

    public class NoteDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        public Guid BatchId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string BatchName { get; set; } = string.Empty;
        public Guid? SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsExpired { get; set; }
    }

    public class CreateNoteRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSizeInBytes { get; set; }
        public Guid CourseId { get; set; }
        public Guid BatchId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid UploadedByUserId { get; set; }
    }

    public class TestDto
    {
        public Guid Id { get; set; }
        public string TestName { get; set; } = string.Empty;
        public DateTime TestDate { get; set; }
        public decimal MaxMarks { get; set; }
        public Guid CourseId { get; set; }
        public Guid BatchId { get; set; }
        public Guid SubjectId { get; set; }
    }

    public class CreateTestRequest
    {
        public string TestName { get; set; } = string.Empty;
        public DateTime TestDate { get; set; }
        public decimal MaxMarks { get; set; }
        public Guid CourseId { get; set; }
        public Guid BatchId { get; set; }
        public Guid SubjectId { get; set; }
    }

    public class AssignmentDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public Guid? CourseId { get; set; }
        public Guid? BatchId { get; set; }
        public Guid? SubjectId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string BatchName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string? FilePath { get; set; }
        public string? OriginalFileName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SubmissionCount { get; set; }
    }

    public class CreateAssignmentRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public Guid BatchId { get; set; }
        public Guid? SubjectId { get; set; }
        public string? FilePath { get; set; }
        public string? OriginalFileName { get; set; }
    }

    public class UpdateAssignmentRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public Guid? BatchId { get; set; }
        public Guid? SubjectId { get; set; }
    }

    public class StudentTestResultDto
    {
        public Guid StudentId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public decimal MarksObtained { get; set; }
        public decimal MaxMarks { get; set; }
        public string? Remarks { get; set; }
        public bool IsPresent { get; set; } = true;
    }

    public class SaveDraftResultRequest
    {
        public Guid TestId { get; set; }
        public List<StudentTestResultDto> Results { get; set; } = new();
    }

    public class PublishResultRequest
    {
        public Guid TestId { get; set; }
        public List<StudentTestResultDto> Results { get; set; } = new();
    }
}

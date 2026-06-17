using CoachOS.Domain.Common;
using CoachOS.Domain.Identity;
using CoachOS.Domain.Student;
using System;

namespace CoachOS.Domain.Learning
{
    public class AssignmentSubmission : TenantBaseEntity
    {
        public Guid AssignmentId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime SubmittedAt { get; set; }
        
        // Submission attachment details
        public string? OriginalFileName { get; set; }
        public string? StoredFileName { get; set; }
        public string? FilePath { get; set; }
        public string? FileType { get; set; }
        public long? FileSizeInBytes { get; set; }

        public string? StudentNotes { get; set; }
        
        // Teacher review
        public int? MarksAwarded { get; set; }
        public string? TeacherRemarks { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public Guid? ReviewedByUserId { get; set; }

        public Assignment? Assignment { get; set; }
        public CoachOS.Domain.Student.Student? Student { get; set; }
        public User? ReviewedByUser { get; set; }
    }
}

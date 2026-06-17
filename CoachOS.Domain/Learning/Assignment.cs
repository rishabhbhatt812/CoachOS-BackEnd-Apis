using CoachOS.Domain.Common;
using CoachOS.Domain.Academic;
using CoachOS.Domain.Identity;
using System;
using System.Collections.Generic;

namespace CoachOS.Domain.Learning
{
    public class Assignment : TenantBaseEntity
    {
        public Guid? CourseId { get; set; }
        public Guid? BatchId { get; set; }
        public Guid? SubjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        
        // Attachment details
        public string? OriginalFileName { get; set; }
        public string? StoredFileName { get; set; }
        public string? FilePath { get; set; }
        public string? FileType { get; set; }
        public long? FileSizeInBytes { get; set; }
        
        public Guid CreatedByUserId { get; set; }
        public bool IsActive { get; set; } = true;

        public Course? Course { get; set; }
        public Batch? Batch { get; set; }
        public Subject? Subject { get; set; }
        public User? CreatedByUser { get; set; }
        public ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
    }
}

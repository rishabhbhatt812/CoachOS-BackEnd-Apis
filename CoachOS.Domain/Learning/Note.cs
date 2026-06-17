using CoachOS.Domain.Common;
using CoachOS.Domain.Academic;
using CoachOS.Domain.Identity;
using System;

namespace CoachOS.Domain.Learning
{
    public class Note : TenantBaseEntity
    {
        public Guid? CourseId { get; set; }
        public Guid? BatchId { get; set; }
        public Guid? SubjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSizeInBytes { get; set; }
        public Guid UploadedByUserId { get; set; }
        public bool IsActive { get; set; } = true;

        public Course? Course { get; set; }
        public Batch? Batch { get; set; }
        public Subject? Subject { get; set; }
        public User? UploadedByUser { get; set; }
    }
}

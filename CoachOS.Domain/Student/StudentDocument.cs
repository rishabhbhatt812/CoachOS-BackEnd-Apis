using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Student
{
    public class StudentDocument : TenantBaseEntity
    {
        public Guid StudentId { get; set; }
        public string DocumentType { get; set; } = string.Empty; // Photo, Aadhaar, Marksheet, etc.
        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string? FileType { get; set; }
        public long FileSize { get; set; }

        public Student? Student { get; set; }
    }
}

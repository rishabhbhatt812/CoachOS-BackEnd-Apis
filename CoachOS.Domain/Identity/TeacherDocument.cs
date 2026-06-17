using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Identity
{
    public class TeacherDocument : TenantBaseEntity
    {
        public Guid TeacherProfileId { get; set; }
        public TeacherProfile? TeacherProfile { get; set; }

        public string DocumentType { get; set; } = string.Empty; // Aadhaar Card, PAN Card, Resume
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedOn { get; set; } = DateTime.UtcNow;
    }
}

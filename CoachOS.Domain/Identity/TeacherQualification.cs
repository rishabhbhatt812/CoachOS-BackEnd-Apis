using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Identity
{
    public class TeacherQualification : TenantBaseEntity
    {
        public Guid TeacherProfileId { get; set; }
        public TeacherProfile? TeacherProfile { get; set; }

        public string Qualification { get; set; } = string.Empty; // B.Sc, M.Sc, etc.
        public string Specialization { get; set; } = string.Empty;
        public string University { get; set; } = string.Empty;
        public int PassingYear { get; set; }
        public string PercentageOrCGPA { get; set; } = string.Empty;
        public string? CertificateFilePath { get; set; }
    }
}

using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.ImportExport
{
    public class ImportJob : TenantBaseEntity
    {
        public string ImportType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public int TotalRows { get; set; }
        public int SuccessRows { get; set; }
        public int FailedRows { get; set; }
        public string Status { get; set; } = "Pending";
        public Guid? UploadedBy { get; set; }
    }
}

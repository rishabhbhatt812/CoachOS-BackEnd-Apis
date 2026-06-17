using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.ImportExport
{
    public class ImportJobError : TenantBaseEntity
    {
        public Guid ImportJobId { get; set; }
        public int RowNumber { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string? RawData { get; set; }

        public ImportJob? ImportJob { get; set; }
    }
}

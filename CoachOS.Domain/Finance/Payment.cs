using CoachOS.Domain.Common;
using CoachOS.Domain.Student;
using System;

namespace CoachOS.Domain.Finance
{
    public class Payment : TenantBaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid FeePlanId { get; set; }
        public Guid? InstallmentId { get; set; }
        public string ReceiptNo { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public string? TransactionNo { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public string? Remark { get; set; }

        public CoachOS.Domain.Student.Student? Student { get; set; }
        public FeePlan? FeePlan { get; set; }
        public Installment? Installment { get; set; }
    }
}

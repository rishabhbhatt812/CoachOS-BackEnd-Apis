using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Finance
{
    public class Installment : TenantBaseEntity
    {
        public Guid FeePlanId { get; set; }
        public int InstallmentNo { get; set; }
        public DateOnly DueDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending";

        public FeePlan? FeePlan { get; set; }
    }
}

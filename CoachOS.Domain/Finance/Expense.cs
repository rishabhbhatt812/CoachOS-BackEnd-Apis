using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Finance
{
    public class Expense : TenantBaseEntity
    {
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateOnly ExpenseDate { get; set; }
        public string? PaymentMode { get; set; }
        public string? Remark { get; set; }
    }
}

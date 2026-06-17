using CoachOS.Domain.Common;
using System;

namespace CoachOS.Domain.Finance
{
    public class PaymentTransaction : TenantBaseEntity
    {
        public Guid PaymentId { get; set; }
        public string Gateway { get; set; } = string.Empty; // Cash, BankTransfer, Stripe, Razorpay, etc.
        public string TransactionRef { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending"; // Success, Failed, Pending

        public Payment? Payment { get; set; }
    }
}

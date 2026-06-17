using System;

namespace CoachOS.Application.DTOs
{
    public class FeePlanDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public Guid BatchId { get; set; }
        public decimal TotalFee { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalFee { get; set; }
        public string PlanType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class InstallmentDto
    {
        public Guid Id { get; set; }
        public Guid FeePlanId { get; set; }
        public int InstallmentNo { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class PaymentDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid FeePlanId { get; set; }
        public Guid InstallmentId { get; set; }
        public string ReceiptNo { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public string? TransactionNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Remark { get; set; }
    }
}

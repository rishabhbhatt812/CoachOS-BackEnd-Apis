using System;

namespace CoachOS.Application.Features.Finance.Dtos
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
        public string PlanType { get; set; } = string.Empty; // "Installment", "Lumpsum"
        public string StudentName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreateFeePlanRequest
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public Guid BatchId { get; set; }
        public decimal TotalFee { get; set; }
        public decimal DiscountAmount { get; set; }
        public string PlanType { get; set; } = string.Empty;
    }
}

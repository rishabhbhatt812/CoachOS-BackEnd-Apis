using System;
using System.Collections.Generic;

namespace CoachOS.Application.Features.Admissions.Dtos
{
    public class QuickAdmissionRequest
    {
        public Guid? InstituteId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }
        public Guid CourseId { get; set; }
        public Guid BatchId { get; set; }
    }

    public class FullAdmissionRequest
    {
        // Institute Details (for SuperAdmin creation)
        public Guid? InstituteId { get; set; }

        // Student Details
        public string StudentCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateOnly AdmissionDate { get; set; }

        // Parent Details
        public string ParentName { get; set; } = string.Empty;
        public string ParentMobile { get; set; } = string.Empty;
        public string? ParentEmail { get; set; }
        public string? ParentOccupation { get; set; }
        public string ParentRelationship { get; set; } = "Parent";

        // Academic Details
        public Guid CourseId { get; set; }
        public Guid BatchId { get; set; }

        // Fee Details
        public decimal TotalFee { get; set; }
        public decimal DiscountAmount { get; set; }
        public string PlanType { get; set; } = "Installments"; // Lumpsum or Installments
        public List<InstallmentRequest> Installments { get; set; } = new();
        public InitialPaymentRequest? InitialPayment { get; set; }
    }

    public class InstallmentRequest
    {
        public int InstallmentNo { get; set; }
        public DateOnly DueDate { get; set; }
        public decimal Amount { get; set; }
    }

    public class InitialPaymentRequest
    {
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public string? TransactionNo { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace CoachOS.Application.DTOs
{
    public class PlanInquiryRequestDto
    {
        public string PlanId { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public string BillingCycle { get; set; } = "yearly";
        public decimal CalculatedAmount { get; set; }
        public string InstituteName { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public int ExpectedStudents { get; set; }
        public string? Remarks { get; set; }
    }

    public class CreateSupportTicketDto
    {
        public string Category { get; set; } = "General"; // "General", "Technical Bug", "Billing", "Feature Request", "Account Access"
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Priority { get; set; } = "Medium"; // "Low", "Medium", "High", "Critical"
    }

    public class ReplySupportTicketDto
    {
        public string TicketId { get; set; } = string.Empty;
        public string ReplyMessage { get; set; } = string.Empty;
        public string? NewStatus { get; set; } = "Resolved"; // "Open", "In Progress", "Resolved", "Closed"
    }

    public class UpdateTicketStatusRequest
    {
        public string Status { get; set; } = "Closed";
    }

    public class SupportTicketResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string TicketNumber { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public string InstituteName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium";
        public string Status { get; set; } = "Open";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool HasUnreadReply { get; set; } = false;
        public int UnreadRepliesCount { get; set; } = 0;
        public DateTime? LastRepliedAt { get; set; }
        public string? LastRepliedBy { get; set; }
        public List<SupportTicketReplyDto> Replies { get; set; } = new();
    }

    public class SupportTicketReplyDto
    {
        public string Id { get; set; } = string.Empty;
        public string ReplyMessage { get; set; } = string.Empty;
        public string RepliedBy { get; set; } = string.Empty;
        public string RepliedByRole { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody, string? toName = null);

        Task<bool> SendPlanPurchaseLeadAlertAsync(
            string instituteName,
            string contactPerson,
            string phone,
            string email,
            string city,
            string state,
            string planName,
            string billingCycle,
            decimal calculatedAmount,
            int expectedStudents,
            string? remarks,
            string ticketId);

        Task<bool> SendPlanPurchaseConfirmationAsync(
            string toEmail,
            string contactPerson,
            string instituteName,
            string planName,
            string ticketId);

        Task<bool> SendSupportTicketAlertAsync(
            string ticketId,
            string senderName,
            string senderEmail,
            string senderRole,
            string instituteName,
            string category,
            string subject,
            string message);

        Task<bool> SendSupportTicketConfirmationToUserAsync(
            string toEmail,
            string recipientName,
            string ticketId,
            string category,
            string subject,
            string message);

        Task<bool> SendSupportTicketReplyAsync(
            string toEmail,
            string recipientName,
            string ticketId,
            string subject,
            string replyMessage,
            string repliedBy);

        Task<bool> SendOrganizationWelcomeEmailAsync(
            string toEmail,
            string instituteName,
            string adminName,
            string temporaryPassword,
            string loginUrl);

        Task<bool> SendStudentWelcomeEmailAsync(
            string toEmail,
            string studentName,
            string studentCode,
            string temporaryPassword,
            string courseName,
            string batchName,
            string loginUrl);
    }
}

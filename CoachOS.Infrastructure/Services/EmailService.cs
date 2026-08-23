using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CoachOS.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoachOS.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody, string? toName = null)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                _logger.LogWarning("SendEmailAsync called with empty recipient email.");
                return false;
            }

            try
            {
                var smtpHost = _configuration["SmtpSettings:Host"] ?? "smtp.gmail.com";
                var smtpPortStr = _configuration["SmtpSettings:Port"] ?? "587";
                int.TryParse(smtpPortStr, out int smtpPort);
                if (smtpPort == 0) smtpPort = 587;

                var enableSsl = bool.Parse(_configuration["SmtpSettings:EnableSsl"] ?? "true");
                var userName = _configuration["SmtpSettings:UserName"] ?? "";
                var password = _configuration["SmtpSettings:Password"] ?? "";
                var fromEmail = _configuration["SmtpSettings:FromEmail"] ?? "noreply@edunex.in";
                var fromName = _configuration["SmtpSettings:FromName"] ?? "EduNex Notifications";

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8
                };

                mailMessage.To.Add(string.IsNullOrWhiteSpace(toName) ? new MailAddress(toEmail) : new MailAddress(toEmail, toName));

                // If SMTP credentials are provided, send via SMTP
                if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
                {
                    using var smtpClient = new SmtpClient(smtpHost, smtpPort)
                    {
                        EnableSsl = enableSsl,
                        Credentials = new NetworkCredential(userName, password),
                        Timeout = 15000
                    };

                    await smtpClient.SendMailAsync(mailMessage);
                    _logger.LogInformation("Email '{Subject}' successfully sent to {ToEmail}", subject, toEmail);
                }
                else
                {
                    _logger.LogInformation("[SIMULATED EMAIL DISPATCH] (SMTP credentials not configured)\nTo: {ToEmail}\nSubject: {Subject}", toEmail, subject);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {ToEmail}. Error: {Message}", toEmail, ex.Message);
                return false;
            }
        }

        public async Task<bool> SendPlanPurchaseLeadAlertAsync(
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
            string ticketId)
        {
            var adminEmail = _configuration["SmtpSettings:AdminNotificationEmail"] ?? "rishabhbhatt437@gmail.com";
            var subject = $"🔥 [NEW PLAN PURCHASE LEAD] {instituteName} selected {planName} ({ticketId})";

            var html = $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'/>
  <style>
    body {{ font-family: 'Segoe UI', Helvetica, Arial, sans-serif; background-color: #0f172a; color: #f8fafc; margin: 0; padding: 20px; }}
    .email-container {{ max-width: 640px; margin: 0 auto; background: #1e293b; border-radius: 16px; border: 1px solid rgba(255,255,255,0.1); overflow: hidden; box-shadow: 0 10px 30px rgba(0,0,0,0.5); }}
    .email-header {{ background: linear-gradient(135deg, #4f46e5 0%, #7c3aed 100%); padding: 30px 24px; text-align: center; color: #ffffff; }}
    .email-header h1 {{ margin: 0; font-size: 22px; font-weight: 800; letter-spacing: -0.02em; }}
    .email-header p {{ margin: 6px 0 0; font-size: 13px; opacity: 0.9; }}
    .email-body {{ padding: 28px 24px; color: #cbd5e1; }}
    .alert-pill {{ display: inline-block; background: #eef2ff; color: #4338ca; padding: 4px 12px; border-radius: 9999px; font-weight: 800; font-size: 12px; margin-bottom: 16px; }}
    .card-box {{ background: #0f172a; border-radius: 12px; border: 1px solid rgba(255,255,255,0.08); padding: 18px; margin-bottom: 20px; }}
    .table-info {{ width: 100%; border-collapse: collapse; font-size: 13px; }}
    .table-info td {{ padding: 8px 6px; border-bottom: 1px solid rgba(255,255,255,0.05); }}
    .table-info td.label {{ color: #94a3b8; font-weight: 600; width: 140px; }}
    .table-info td.value {{ color: #ffffff; font-weight: 700; }}
    .price-box {{ background: rgba(79, 70, 229, 0.15); border: 1px solid rgba(99, 102, 241, 0.3); border-radius: 10px; padding: 14px; text-align: center; margin: 16px 0; }}
    .price-box .amt {{ font-size: 24px; font-weight: 900; color: #818cf8; }}
    .footer-note {{ font-size: 11px; color: #64748b; text-align: center; margin-top: 24px; border-top: 1px solid rgba(255,255,255,0.05); padding-top: 16px; }}
  </style>
</head>
<body>
  <div class='email-container'>
    <div class='email-header'>
      <h1>EduNex SaaS Notification</h1>
      <p>New Coaching Institute Plan Purchase Application</p>
    </div>
    <div class='email-body'>
      <span class='alert-pill'>Ticket ID: {ticketId}</span>
      <h2 style='color:#ffffff; margin: 0 0 12px; font-size: 18px;'>Incoming Institute Lead Details</h2>
      <p style='font-size: 13px; line-height: 1.5; color: #94a3b8;'>A prospective client has submitted an inquiry to purchase an EduNex coaching management subscription plan.</p>
      
      <div class='card-box'>
        <table class='table-info'>
          <tr><td class='label'>Institute Name:</td><td class='value'>{instituteName}</td></tr>
          <tr><td class='label'>Contact Person:</td><td class='value'>{contactPerson}</td></tr>
          <tr><td class='label'>Mobile / Phone:</td><td class='value'><a href='tel:{phone}' style='color:#38bdf8; text-decoration:none;'>{phone}</a></td></tr>
          <tr><td class='label'>Email Address:</td><td class='value'><a href='mailto:{email}' style='color:#38bdf8; text-decoration:none;'>{email}</a></td></tr>
          <tr><td class='label'>Location:</td><td class='value'>{city}, {state}</td></tr>
          <tr><td class='label'>Target Students:</td><td class='value'>~{expectedStudents} Active Students</td></tr>
          <tr><td class='label'>Selected Plan:</td><td class='value' style='color:#a5b4fc;'>{planName} ({billingCycle.ToUpper()})</td></tr>
          <tr><td class='label'>Custom Remarks:</td><td class='value' style='font-weight:400; color:#e2e8f0;'>{(string.IsNullOrWhiteSpace(remarks) ? "None specified" : remarks)}</td></tr>
        </table>
      </div>

      <div class='price-box'>
        <div style='font-size: 12px; color: #94a3b8; text-transform: uppercase;'>Calculated Subscription Quote</div>
        <div class='amt'>₹{calculatedAmount:N0} <span style='font-size:14px; font-weight:600; color:#cbd5e1;'>/ {billingCycle}</span></div>
      </div>

      <div class='footer-note'>
        Generated by EduNex Cloud Platform Engine • Internal Lead Dispatch to {adminEmail}
      </div>
    </div>
  </div>
</body>
</html>";

            // 1. Send alert to personal admin email
            await SendEmailAsync(adminEmail, subject, html, "Rishabh Bhatt");

            // 2. Also send confirmation to the client
            await SendPlanPurchaseConfirmationAsync(email, contactPerson, instituteName, planName, ticketId);

            return true;
        }

        public async Task<bool> SendPlanPurchaseConfirmationAsync(
            string toEmail,
            string contactPerson,
            string instituteName,
            string planName,
            string ticketId)
        {
            var subject = $"✓ EduNex Subscription Application Received [{ticketId}]";
            var html = $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'/>
  <style>
    body {{ font-family: 'Segoe UI', Helvetica, Arial, sans-serif; background-color: #f8fafc; color: #0f172a; margin: 0; padding: 20px; }}
    .email-container {{ max-width: 600px; margin: 0 auto; background: #ffffff; border-radius: 16px; border: 1px solid #e2e8f0; overflow: hidden; box-shadow: 0 10px 25px rgba(0,0,0,0.05); }}
    .email-header {{ background: linear-gradient(135deg, #4f46e5 0%, #4338ca 100%); padding: 32px 24px; text-align: center; color: #ffffff; }}
    .email-header h1 {{ margin: 0; font-size: 24px; font-weight: 800; }}
    .email-body {{ padding: 32px 24px; }}
    .ticket-badge {{ font-family: monospace; font-weight: 800; background: #eef2ff; color: #4f46e5; padding: 4px 10px; border-radius: 6px; }}
    .next-steps {{ background: #f8fafc; border: 1px solid #e2e8f0; border-radius: 10px; padding: 18px; margin: 20px 0; }}
    .next-steps li {{ margin-bottom: 8px; font-size: 13px; color: #334155; }}
    .footer {{ text-align: center; font-size: 12px; color: #94a3b8; border-top: 1px solid #f1f5f9; padding-top: 18px; }}
  </style>
</head>
<body>
  <div class='email-container'>
    <div class='email-header'>
      <h1>Welcome to EduNex</h1>
      <p>Next-Gen Coaching & Academy Infrastructure</p>
    </div>
    <div class='email-body'>
      <h2 style='font-size: 18px; margin-top: 0;'>Hello {contactPerson},</h2>
      <p style='font-size: 14px; line-height: 1.6; color: #475569;'>
        Thank you for choosing EduNex for <strong>{instituteName}</strong>. We have received your subscription application for the <strong>{planName}</strong> plan.
      </p>
      <p style='font-size: 14px; color: #475569;'>
        Your application reference ticket ID is: <span class='ticket-badge'>{ticketId}</span>
      </p>

      <div class='next-steps'>
        <h4 style='margin: 0 0 10px; font-size: 13px; text-transform: uppercase; color: #4f46e5;'>What happens next:</h4>
        <ul style='padding-left: 20px; margin: 0;'>
          <li>Our solutions specialist will verify your coaching institute specifications.</li>
          <li>You will receive your official tenant activation invite & sandbox access within 2 hours.</li>
          <li>Our technical engineering team will assist with zero-cost data migration from Excel/sheets.</li>
        </ul>
      </div>

      <p style='font-size: 13px; color: #64748b;'>
        If you have urgent questions, reply directly to this email or contact support at <a href='mailto:support@edunex.in' style='color:#4f46e5;'>support@edunex.in</a>.
      </p>

      <div class='footer'>
        © 2026 EduNex Technologies. All rights reserved. • ISO 27001 Certified Academic Cloud
      </div>
    </div>
  </div>
</body>
</html>";

            return await SendEmailAsync(toEmail, subject, html, contactPerson);
        }

        public async Task<bool> SendSupportTicketAlertAsync(
            string ticketId,
            string senderName,
            string senderEmail,
            string senderRole,
            string instituteName,
            string category,
            string subject,
            string message)
        {
            var adminEmail = _configuration["SmtpSettings:AdminNotificationEmail"] ?? "rishabhbhatt437@gmail.com";
            var emailSubject = $"🚨 [NEW SUPPORT TICKET] {category.ToUpper()}: {subject} ({ticketId})";

            var html = $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'/>
  <style>
    body {{ font-family: 'Segoe UI', Helvetica, Arial, sans-serif; background-color: #0f172a; color: #f8fafc; margin: 0; padding: 20px; }}
    .email-container {{ max-width: 640px; margin: 0 auto; background: #1e293b; border-radius: 16px; border: 1px solid rgba(255,255,255,0.1); overflow: hidden; }}
    .email-header {{ background: linear-gradient(135deg, #0284c7 0%, #0369a1 100%); padding: 26px 20px; text-align: center; color: #ffffff; }}
    .email-body {{ padding: 24px; }}
    .ticket-badge {{ font-family: monospace; font-weight: 800; background: #e0f2fe; color: #0369a1; padding: 4px 10px; border-radius: 6px; }}
    .card-box {{ background: #0f172a; border-radius: 10px; padding: 16px; margin: 16px 0; border: 1px solid rgba(255,255,255,0.06); }}
    .msg-box {{ background: #111827; border-left: 4px solid #38bdf8; padding: 14px; border-radius: 6px; color: #e2e8f0; font-size: 13px; line-height: 1.6; margin: 16px 0; }}
  </style>
</head>
<body>
  <div class='email-container'>
    <div class='email-header'>
      <h2 style='margin:0;'>Client Helpdesk Ticket Raised</h2>
      <p style='margin:4px 0 0; font-size: 12px;'>Ticket #{ticketId}</p>
    </div>
    <div class='email-body'>
      <span class='ticket-badge'>{category}</span>
      <h3 style='color:#ffffff; margin: 12px 0 6px;'>{subject}</h3>
      <p style='font-size:12px; color:#94a3b8;'>Submitted by <strong>{senderName}</strong> ({senderRole}) from <strong>{instituteName}</strong></p>

      <div class='card-box'>
        <div style='font-size:12px; color:#94a3b8;'>Client Email: <a href='mailto:{senderEmail}' style='color:#38bdf8;'>{senderEmail}</a></div>
      </div>

      <div class='msg-box'>
        <strong>Message Description:</strong><br/>
        {message}
      </div>

      <div style='font-size: 11px; color: #64748b; text-align: center; margin-top: 20px;'>
        Log into the EduNex Global Admin Panel to review and respond to this ticket.
      </div>
    </div>
  </div>
</body>
</html>";

            return await SendEmailAsync(adminEmail, emailSubject, html, "Rishabh Bhatt (Support Desk)");
        }

        public async Task<bool> SendSupportTicketConfirmationToUserAsync(
            string toEmail,
            string recipientName,
            string ticketId,
            string category,
            string subject,
            string message)
        {
            var emailSubject = $"🎫 [Ticket #{ticketId}] Confirmation: {subject}";
            var html = $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'/>
  <style>
    body {{ font-family: 'Segoe UI', Helvetica, Arial, sans-serif; background-color: #f8fafc; color: #0f172a; margin: 0; padding: 20px; }}
    .email-container {{ max-width: 600px; margin: 0 auto; background: #ffffff; border-radius: 14px; border: 1px solid #e2e8f0; overflow: hidden; }}
    .email-header {{ background: linear-gradient(135deg, #4f46e5 0%, #6366f1 100%); padding: 24px; color: #ffffff; text-align: center; }}
    .email-body {{ padding: 24px; }}
    .badge {{ display: inline-block; background: #eef2ff; color: #4f46e5; border: 1px solid #c7d2fe; padding: 4px 10px; border-radius: 6px; font-weight: 700; font-size: 12px; }}
    .msg-box {{ background: #f8fafc; border-left: 4px solid #4f46e5; padding: 14px; border-radius: 6px; color: #334155; font-size: 13px; line-height: 1.6; margin: 16px 0; }}
  </style>
</head>
<body>
  <div class='email-container'>
    <div class='email-header'>
      <h2 style='margin:0;'>Support Request Received</h2>
      <p style='margin:4px 0 0; font-size: 13px; opacity: 0.9;'>Ticket #{ticketId}</p>
    </div>
    <div class='email-body'>
      <p>Hello {recipientName},</p>
      <p style='color:#475569; font-size: 13px;'>
        Thank you for contacting EduNex Support. We have logged your support inquiry and assigned it ticket reference <strong>#{ticketId}</strong>.
      </p>

      <span class='badge'>{category}</span>
      <h3 style='margin: 12px 0 6px; color:#1e1b4b;'>{subject}</h3>

      <div class='msg-box'>
        {message}
      </div>

      <p style='font-size: 12px; color: #64748b;'>
        Our technical engineering team will review and reply directly to your dashboard and registered email address.
      </p>
    </div>
  </div>
</body>
</html>";

            return await SendEmailAsync(toEmail, emailSubject, html, recipientName);
        }

        public async Task<bool> SendSupportTicketReplyAsync(
            string toEmail,
            string recipientName,
            string ticketId,
            string subject,
            string replyMessage,
            string repliedBy)
        {
            var emailSubject = $"Re: [{ticketId}] {subject} - EduNex Support Response";
            var html = $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'/>
  <style>
    body {{ font-family: 'Segoe UI', Helvetica, Arial, sans-serif; background-color: #f8fafc; color: #0f172a; margin: 0; padding: 20px; }}
    .email-container {{ max-width: 600px; margin: 0 auto; background: #ffffff; border-radius: 14px; border: 1px solid #e2e8f0; overflow: hidden; }}
    .email-header {{ background: #4f46e5; padding: 22px; color: #ffffff; text-align: center; }}
    .email-body {{ padding: 24px; }}
    .reply-box {{ background: #f8fafc; border-left: 4px solid #4f46e5; padding: 16px; border-radius: 8px; font-size: 14px; line-height: 1.6; margin: 16px 0; color: #1e293b; }}
  </style>
</head>
<body>
  <div class='email-container'>
    <div class='email-header'>
      <h3 style='margin:0;'>EduNex Support Update</h3>
      <p style='margin:4px 0 0; font-size: 12px;'>Ticket #{ticketId}</p>
    </div>
    <div class='email-body'>
      <p>Hello {recipientName},</p>
      <p style='color:#475569; font-size: 13px;'>Our technical support team has reviewed your query regarding <strong>{subject}</strong>:</p>

      <div class='reply-box'>
        {replyMessage}
      </div>

      <p style='font-size: 12px; color: #64748b;'>Replied by: <strong>{repliedBy}</strong> (EduNex Global Support)</p>
    </div>
  </div>
</body>
</html>";

            return await SendEmailAsync(toEmail, emailSubject, html, recipientName);
        }

        public async Task<bool> SendOrganizationWelcomeEmailAsync(
            string toEmail,
            string instituteName,
            string adminName,
            string temporaryPassword,
            string loginUrl)
        {
            var subject = $"🎉 Welcome to EduNex - Official Onboarding & Admin Credentials for {instituteName}";
            var html = $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'/>
  <style>
    body {{ font-family: 'Segoe UI', Helvetica, Arial, sans-serif; background-color: #0f172a; color: #f8fafc; margin: 0; padding: 20px; }}
    .email-container {{ max-width: 600px; margin: 0 auto; background: #1e293b; border-radius: 16px; border: 1px solid rgba(255,255,255,0.1); overflow: hidden; }}
    .email-header {{ background: linear-gradient(135deg, #4f46e5 0%, #7c3aed 100%); padding: 32px 24px; text-align: center; color: #ffffff; }}
    .email-body {{ padding: 30px 24px; color: #cbd5e1; }}
    .cred-box {{ background: #0f172a; border: 1px solid #4f46e5; border-radius: 12px; padding: 20px; margin: 20px 0; }}
    .cred-row {{ display: flex; justify-content: space-between; padding: 8px 0; border-bottom: 1px solid rgba(255,255,255,0.06); font-size: 13px; }}
    .cred-val {{ font-family: monospace; font-weight: 800; color: #38bdf8; }}
    .btn-login {{ display: inline-block; background: #4f46e5; color: #ffffff; font-weight: 700; text-decoration: none; padding: 12px 28px; border-radius: 10px; margin-top: 14px; text-align: center; }}
  </style>
</head>
<body>
  <div class='email-container'>
    <div class='email-header'>
      <h1 style='margin:0; font-size:22px;'>Welcome to EduNex</h1>
      <p style='margin:6px 0 0; font-size:13px;'>Coaching & Academic Cloud Platform</p>
    </div>
    <div class='email-body'>
      <h2 style='color:#ffffff; font-size:18px; margin-top:0;'>Hello {adminName},</h2>
      <p style='font-size:14px; line-height:1.6; color:#94a3b8;'>
        Congratulations! Your coaching organization <strong>{instituteName}</strong> has been configured on the EduNex enterprise management infrastructure.
      </p>

      <div class='cred-box'>
        <h4 style='color:#a5b4fc; margin:0 0 12px; text-transform:uppercase; font-size:12px;'>Administrator Portal Credentials</h4>
        <div style='font-size:13px; margin-bottom:8px;'><strong>Login Portal:</strong> <a href='{loginUrl}' style='color:#38bdf8;'>{loginUrl}</a></div>
        <div style='font-size:13px; margin-bottom:8px;'><strong>Registered Email:</strong> <span class='cred-val'>{toEmail}</span></div>
        <div style='font-size:13px; margin-bottom:8px;'><strong>Initial Password:</strong> <span class='cred-val'>{temporaryPassword}</span></div>
      </div>

      <div style='text-align: center; margin: 24px 0;'>
        <a href='{loginUrl}' class='btn-login'>Sign In to Admin Dashboard →</a>
      </div>

      <p style='font-size:12px; color:#64748b; line-height:1.5;'>
        🔒 <strong>Security Tip:</strong> Please log in and immediately change your temporary password under Profile Settings.
      </p>
    </div>
  </div>
</body>
</html>";

            return await SendEmailAsync(toEmail, subject, html, adminName);
        }

        public async Task<bool> SendStudentWelcomeEmailAsync(
            string toEmail,
            string studentName,
            string studentCode,
            string temporaryPassword,
            string courseName,
            string batchName,
            string loginUrl)
        {
            var subject = $"🎓 Welcome to EduNex Student Portal - Enrollment Confirmation ({studentCode})";
            var html = $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'/>
  <style>
    body {{ font-family: 'Segoe UI', Helvetica, Arial, sans-serif; background-color: #f8fafc; color: #0f172a; margin: 0; padding: 20px; }}
    .email-container {{ max-width: 600px; margin: 0 auto; background: #ffffff; border-radius: 16px; border: 1px solid #e2e8f0; overflow: hidden; }}
    .email-header {{ background: linear-gradient(135deg, #059669 0%, #047857 100%); padding: 30px 24px; text-align: center; color: #ffffff; }}
    .email-body {{ padding: 28px 24px; }}
    .cred-box {{ background: #f8fafc; border: 1px solid #cbd5e1; border-radius: 10px; padding: 18px; margin: 18px 0; }}
    .cred-val {{ font-family: monospace; font-weight: 800; color: #059669; }}
    .btn-login {{ display: inline-block; background: #059669; color: #ffffff; font-weight: 700; text-decoration: none; padding: 12px 26px; border-radius: 10px; margin-top: 14px; }}
  </style>
</head>
<body>
  <div class='email-container'>
    <div class='email-header'>
      <h1 style='margin:0; font-size:22px;'>Welcome to EduNex</h1>
      <p style='margin:6px 0 0; font-size:13px;'>Student & Parent Academic Portal</p>
    </div>
    <div class='email-body'>
      <h2 style='font-size:18px; margin-top:0;'>Hello {studentName},</h2>
      <p style='font-size:14px; line-height:1.6; color:#475569;'>
        Welcome! You have been enrolled in <strong>{courseName}</strong> ({batchName}). Your student portal account is now active.
      </p>

      <div class='cred-box'>
        <h4 style='color:#059669; margin:0 0 10px; font-size:12px; text-transform:uppercase;'>Your Student Portal Credentials</h4>
        <div style='font-size:13px; margin-bottom:6px;'><strong>Student Roll Code:</strong> <span class='cred-val'>{studentCode}</span></div>
        <div style='font-size:13px; margin-bottom:6px;'><strong>Login Email:</strong> <span class='cred-val'>{toEmail}</span></div>
        <div style='font-size:13px; margin-bottom:6px;'><strong>Temporary Password:</strong> <span class='cred-val'>{temporaryPassword}</span></div>
      </div>

      <div style='text-align: center; margin: 20px 0;'>
        <a href='{loginUrl}' class='btn-login'>Access Student Portal →</a>
      </div>

      <p style='font-size:12px; color:#64748b;'>
        Use the student portal to track batch attendance, download LMS study notes, view test marks & performance rankings, and manage fee receipts.
      </p>
    </div>
  </div>
</body>
</html>";

            return await SendEmailAsync(toEmail, subject, html, studentName);
        }
    }
}

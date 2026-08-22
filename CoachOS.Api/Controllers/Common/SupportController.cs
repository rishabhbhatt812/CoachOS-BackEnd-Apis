using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoachOS.Application.DTOs;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoachOS.Api.Controllers.Common
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SupportController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ICurrentUserService _currentUserService;
        private readonly AppDbContext _context;
        private static readonly ConcurrentBag<SupportTicketResponseDto> _tickets = new();

        public SupportController(
            IEmailService emailService,
            ICurrentUserService currentUserService,
            AppDbContext context)
        {
            _emailService = emailService;
            _currentUserService = currentUserService;
            _context = context;
        }

        [HttpPost("ticket")]
        public async Task<IActionResult> CreateTicket([FromBody] CreateSupportTicketDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Subject) || string.IsNullOrWhiteSpace(dto.Message))
            {
                return BadRequest(new { success = false, message = "Subject and message are required." });
            }

            var userId = _currentUserService.UserId?.ToString() ?? Guid.NewGuid().ToString();
            
            // Resolve user metadata
            var dbUser = _currentUserService.UserId.HasValue 
                ? await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId.Value)
                : null;

            var dbInstitute = (dbUser?.InstituteId != null)
                ? await _context.Institutes.FirstOrDefaultAsync(i => i.Id == dbUser.InstituteId)
                : null;

            var userName = dbUser?.FullName ?? User.FindFirstValue(ClaimTypes.Name) ?? "EduNex User";
            var userEmail = dbUser?.Email ?? User.FindFirstValue(ClaimTypes.Email) ?? "client@edunex.in";
            var userRole = dbUser?.Role?.RoleName ?? _currentUserService.RoleCode ?? "CLIENT";
            var instituteName = dbInstitute?.Name ?? "Coaching Academy";

            var ticketNumber = "TKT-" + new Random().Next(10000, 99999);

            var ticket = new SupportTicketResponseDto
            {
                Id = Guid.NewGuid().ToString(),
                TicketNumber = ticketNumber,
                UserId = userId,
                UserName = userName,
                UserEmail = userEmail,
                UserRole = userRole,
                InstituteName = instituteName,
                Category = dto.Category,
                Subject = dto.Subject,
                Message = dto.Message,
                Priority = dto.Priority ?? "Medium",
                Status = "Open",
                CreatedAt = DateTime.UtcNow,
                Replies = new List<SupportTicketReplyDto>()
            };

            _tickets.Add(ticket);

            // Send notification email to admin email
            _ = Task.Run(async () =>
            {
                await _emailService.SendSupportTicketAlertAsync(
                    ticket.TicketNumber,
                    ticket.UserName,
                    ticket.UserEmail,
                    ticket.UserRole,
                    ticket.InstituteName,
                    ticket.Category,
                    ticket.Subject,
                    ticket.Message);
            });

            return Ok(new
            {
                success = true,
                ticket = ticket,
                message = $"Support ticket #{ticketNumber} raised successfully. Our support team will assist you shortly."
            });
        }

        [HttpGet("my-tickets")]
        public IActionResult GetMyTickets()
        {
            var userIdStr = _currentUserService.UserId?.ToString();
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var list = _tickets
                .Where(t => string.IsNullOrEmpty(userIdStr) || t.UserId == userIdStr || (!string.IsNullOrEmpty(userEmail) && t.UserEmail == userEmail))
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            return Ok(new { isSuccess = true, data = list });
        }

        [HttpGet("all-tickets")]
        public IActionResult GetAllTickets()
        {
            var list = _tickets.OrderByDescending(t => t.CreatedAt).ToList();
            return Ok(new { isSuccess = true, data = list });
        }

        [HttpPost("reply")]
        public async Task<IActionResult> ReplyToTicket([FromBody] ReplySupportTicketDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.TicketId) || string.IsNullOrWhiteSpace(dto.ReplyMessage))
            {
                return BadRequest(new { success = false, message = "Ticket ID and reply message are required." });
            }

            var ticket = _tickets.FirstOrDefault(t => t.Id == dto.TicketId || t.TicketNumber == dto.TicketId);
            if (ticket == null)
            {
                return NotFound(new { success = false, message = "Ticket not found." });
            }

            var adminName = User.FindFirstValue(ClaimTypes.Name) ?? "EduNex Global Support";
            var adminRole = _currentUserService.RoleCode ?? "GLOBAL_ADMIN";

            var reply = new SupportTicketReplyDto
            {
                Id = Guid.NewGuid().ToString(),
                ReplyMessage = dto.ReplyMessage,
                RepliedBy = adminName,
                RepliedByRole = adminRole,
                CreatedAt = DateTime.UtcNow
            };

            ticket.Replies.Add(reply);
            ticket.Status = dto.NewStatus ?? "In Progress";

            // Send email reply back to the user
            _ = Task.Run(async () =>
            {
                await _emailService.SendSupportTicketReplyAsync(
                    ticket.UserEmail,
                    ticket.UserName,
                    ticket.TicketNumber,
                    ticket.Subject,
                    dto.ReplyMessage,
                    adminName);
            });

            return Ok(new
            {
                success = true,
                message = "Reply sent and client notified via email.",
                ticket = ticket
            });
        }
    }
}

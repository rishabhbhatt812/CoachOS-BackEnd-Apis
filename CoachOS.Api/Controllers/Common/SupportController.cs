using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
        private static readonly object _fileLock = new();
        private static readonly string _storageFilePath = Path.Combine(AppContext.BaseDirectory, "App_Data", "support_tickets.json");

        public SupportController(
            IEmailService emailService,
            ICurrentUserService currentUserService,
            AppDbContext context)
        {
            _emailService = emailService;
            _currentUserService = currentUserService;
            _context = context;
        }

        private List<SupportTicketResponseDto> GetStoredTickets()
        {
            lock (_fileLock)
            {
                try
                {
                    if (System.IO.File.Exists(_storageFilePath))
                    {
                        var json = System.IO.File.ReadAllText(_storageFilePath);
                        if (!string.IsNullOrWhiteSpace(json))
                        {
                            var list = JsonSerializer.Deserialize<List<SupportTicketResponseDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            if (list != null && list.Count > 0)
                            {
                                return list;
                            }
                        }
                    }
                }
                catch
                {
                    // Fallback to initial seeds
                }

                // Initial seed
                var initialList = new List<SupportTicketResponseDto>
                {
                    new SupportTicketResponseDto
                    {
                        Id = "tkt-init-1",
                        TicketNumber = "TKT-78291",
                        UserId = "c1c00000-0000-0000-0000-000000000001",
                        UserName = "Apex Coaching Administrator",
                        UserEmail = "admin@apex.com",
                        UserRole = "INSTITUTE_ADMIN",
                        InstituteName = "Apex Coaching Academy",
                        Category = "General Inquiry",
                        Subject = "Welcome to EduNex Support & Help Desk",
                        Message = "Welcome to your dedicated coaching management support channel. Our technical team is available 24/7 to assist with batch scheduling, LMS notes, fee collections, or custom institute configuration.",
                        Priority = "Medium",
                        Status = "Open",
                        CreatedAt = DateTime.UtcNow.AddHours(-2),
                        HasUnreadReply = false,
                        Replies = new List<SupportTicketReplyDto>
                        {
                            new SupportTicketReplyDto
                            {
                                Id = "rep-init-1",
                                ReplyMessage = "Hello! Our dedicated support desk is active. Feel free to raise any questions or feature suggestions right here.",
                                RepliedBy = "EduNex Global Team",
                                RepliedByRole = "GLOBAL_ADMIN",
                                CreatedAt = DateTime.UtcNow.AddHours(-1)
                            }
                        }
                    }
                };

                SaveStoredTickets(initialList);
                return initialList;
            }
        }

        private void SaveStoredTickets(List<SupportTicketResponseDto> tickets)
        {
            lock (_fileLock)
            {
                try
                {
                    var dir = Path.GetDirectoryName(_storageFilePath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    var json = JsonSerializer.Serialize(tickets, new JsonSerializerOptions { WriteIndented = true });
                    System.IO.File.WriteAllText(_storageFilePath, json);
                }
                catch
                {
                    // Ignore write failures
                }
            }
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

            var dbInstitute = (dbUser?.InstituteId != null && dbUser.InstituteId != Guid.Empty)
                ? await _context.Institutes.FirstOrDefaultAsync(i => i.Id == dbUser.InstituteId)
                : null;

            var userName = dbUser?.FullName ?? User.FindFirstValue(ClaimTypes.Name) ?? "EduNex User";
            var userEmail = dbUser?.Email ?? User.FindFirstValue(ClaimTypes.Email) ?? "client@edunex.in";
            var userRole = dbUser?.Role?.RoleName ?? _currentUserService.RoleCode ?? "ADMIN";
            var instituteName = dbInstitute?.Name ?? "Apex Coaching Academy";

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

            var list = GetStoredTickets();
            list.Insert(0, ticket);
            SaveStoredTickets(list);

            // Send notification email to engineering desk & confirmation to client
            _ = Task.Run(async () =>
            {
                try
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

                    await _emailService.SendSupportTicketConfirmationToUserAsync(
                        ticket.UserEmail,
                        ticket.UserName,
                        ticket.TicketNumber,
                        ticket.Category,
                        ticket.Subject,
                        ticket.Message);
                }
                catch
                {
                    // Logged in email service
                }
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
            var roleCode = (_currentUserService.RoleCode ?? "").ToUpperInvariant();

            var list = GetStoredTickets();

            // If user is SuperAdmin or GlobalAdmin, show all tickets
            if (roleCode.Contains("SUPER") || roleCode.Contains("GLOBAL") || User.IsInRole("SUPER_ADMIN") || User.IsInRole("GLOBAL_ADMIN"))
            {
                return Ok(new { isSuccess = true, data = list.OrderByDescending(t => t.CreatedAt).ToList() });
            }

            var filtered = list
                .Where(t => string.IsNullOrEmpty(userIdStr) || t.UserId == userIdStr || (!string.IsNullOrEmpty(userEmail) && t.UserEmail.Equals(userEmail, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            // If user has no specific tickets, still show public or institute tickets
            if (filtered.Count == 0 && list.Count > 0)
            {
                filtered = list.OrderByDescending(t => t.CreatedAt).ToList();
            }

            return Ok(new { isSuccess = true, data = filtered });
        }

        [HttpGet("all-tickets")]
        public IActionResult GetAllTickets()
        {
            var list = GetStoredTickets().OrderByDescending(t => t.CreatedAt).ToList();
            return Ok(new { isSuccess = true, data = list });
        }

        [HttpGet("unread-count")]
        public IActionResult GetUnreadCount()
        {
            var userIdStr = _currentUserService.UserId?.ToString();
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var roleCode = (_currentUserService.RoleCode ?? "").ToUpperInvariant();

            var list = GetStoredTickets();

            if (roleCode.Contains("SUPER") || roleCode.Contains("GLOBAL"))
            {
                var count = list.Count(t => t.HasUnreadReply || t.Status == "Open");
                return Ok(new { isSuccess = true, unreadCount = count });
            }

            var userCount = list
                .Where(t => (string.IsNullOrEmpty(userIdStr) || t.UserId == userIdStr || (!string.IsNullOrEmpty(userEmail) && t.UserEmail.Equals(userEmail, StringComparison.OrdinalIgnoreCase))) && t.HasUnreadReply)
                .Count();

            return Ok(new { isSuccess = true, unreadCount = userCount });
        }

        [HttpPost("ticket/{ticketId}/read")]
        public IActionResult MarkTicketAsRead(string ticketId)
        {
            var list = GetStoredTickets();
            var ticket = list.FirstOrDefault(t => t.Id == ticketId || t.TicketNumber == ticketId);
            if (ticket != null)
            {
                ticket.HasUnreadReply = false;
                ticket.UnreadRepliesCount = 0;
                SaveStoredTickets(list);
            }
            return Ok(new { isSuccess = true, message = "Ticket marked as read." });
        }

        [HttpPost("ticket/{ticketId}/status")]
        public IActionResult UpdateTicketStatus(string ticketId, [FromBody] UpdateTicketStatusRequest dto)
        {
            var list = GetStoredTickets();
            var ticket = list.FirstOrDefault(t => t.Id == ticketId || t.TicketNumber == ticketId);
            if (ticket == null)
            {
                return NotFound(new { isSuccess = false, message = "Ticket not found." });
            }

            ticket.Status = !string.IsNullOrWhiteSpace(dto?.Status) ? dto.Status : "Closed";
            SaveStoredTickets(list);
            return Ok(new { isSuccess = true, message = $"Ticket status updated to {ticket.Status}.", ticket });
        }

        [HttpPost("ticket/{ticketId}/close")]
        public IActionResult CloseTicket(string ticketId)
        {
            var list = GetStoredTickets();
            var ticket = list.FirstOrDefault(t => t.Id == ticketId || t.TicketNumber == ticketId);
            if (ticket == null)
            {
                return NotFound(new { isSuccess = false, message = "Ticket not found." });
            }

            ticket.Status = "Closed";
            SaveStoredTickets(list);
            return Ok(new { isSuccess = true, message = "Ticket closed successfully.", ticket });
        }

        [HttpPost("reply")]
        public async Task<IActionResult> ReplyToTicket([FromBody] ReplySupportTicketDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.TicketId) || string.IsNullOrWhiteSpace(dto.ReplyMessage))
            {
                return BadRequest(new { success = false, message = "Ticket ID and reply message are required." });
            }

            var list = GetStoredTickets();
            var ticket = list.FirstOrDefault(t => t.Id == dto.TicketId || t.TicketNumber == dto.TicketId);
            if (ticket == null)
            {
                return NotFound(new { success = false, message = "Ticket not found." });
            }

            var adminName = User.FindFirstValue(ClaimTypes.Name) ?? "Apex Super Admin";
            var adminRole = _currentUserService.RoleCode ?? "SUPER_ADMIN";

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
            ticket.HasUnreadReply = true;
            ticket.UnreadRepliesCount++;
            ticket.LastRepliedAt = DateTime.UtcNow;
            ticket.LastRepliedBy = adminName;

            SaveStoredTickets(list);

            // Send email reply back to the user
            _ = Task.Run(async () =>
            {
                try
                {
                    await _emailService.SendSupportTicketReplyAsync(
                        ticket.UserEmail,
                        ticket.UserName,
                        ticket.TicketNumber,
                        ticket.Subject,
                        dto.ReplyMessage,
                        adminName);
                }
                catch
                {
                    // Email logging handled
                }
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

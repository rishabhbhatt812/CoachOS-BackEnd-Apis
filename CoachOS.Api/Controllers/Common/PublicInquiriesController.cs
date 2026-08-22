using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachOS.Application.DTOs;
using CoachOS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoachOS.Api.Controllers.Common
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicInquiriesController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private static readonly ConcurrentBag<PlanInquiryRecord> _inquiries = new();

        public PublicInquiriesController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitPlanInquiry([FromBody] PlanInquiryRequestDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.InstituteName))
            {
                return BadRequest(new { success = false, message = "Institute name and email address are required." });
            }

            var ticketId = "INQ-" + new Random().Next(100000, 999999);

            var record = new PlanInquiryRecord
            {
                Id = ticketId,
                PlanId = dto.PlanId,
                PlanName = dto.PlanName,
                BillingCycle = dto.BillingCycle,
                CalculatedAmount = dto.CalculatedAmount,
                InstituteName = dto.InstituteName,
                ContactPerson = dto.ContactPerson,
                Email = dto.Email,
                Phone = dto.Phone,
                City = dto.City,
                State = dto.State,
                ExpectedStudents = dto.ExpectedStudents,
                Remarks = dto.Remarks,
                Status = "New",
                CreatedAt = DateTime.UtcNow
            };

            _inquiries.Add(record);

            // Send lead alert email to rishabhbhatt437@gmail.com and confirmation email to applicant
            _ = Task.Run(async () =>
            {
                await _emailService.SendPlanPurchaseLeadAlertAsync(
                    record.InstituteName,
                    record.ContactPerson,
                    record.Phone,
                    record.Email,
                    record.City,
                    record.State,
                    record.PlanName,
                    record.BillingCycle,
                    record.CalculatedAmount,
                    record.ExpectedStudents,
                    record.Remarks,
                    record.Id);
            });

            return Ok(new
            {
                success = true,
                ticketId = record.Id,
                message = "Plan purchase application received successfully! Our onboarding team will contact you shortly."
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetInquiries()
        {
            var list = _inquiries.OrderByDescending(x => x.CreatedAt).ToList();
            return Ok(new { isSuccess = true, data = list });
        }
    }

    public class PlanInquiryRecord
    {
        public string Id { get; set; } = string.Empty;
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
        public string Status { get; set; } = "New";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

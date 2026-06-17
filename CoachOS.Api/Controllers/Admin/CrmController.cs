using CoachOS.Api.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoachOS.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "INSTITUTE_ADMIN,GLOBAL_ADMIN,SUPER_ADMIN,RECEPTIONIST")]
    [ModuleAccess("CRM")]
    public class CrmController : ControllerBase
    {
        private readonly CoachOS.Application.Interfaces.Services.ICrmService _crmService;

        public CrmController(CoachOS.Application.Interfaces.Services.ICrmService crmService)
        {
            _crmService = crmService;
        }

        [HttpGet("enquiries")]
        public async System.Threading.Tasks.Task<IActionResult> GetEnquiries([FromQuery] CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            return Ok(await _crmService.GetEnquiriesAsync(paginationParams));
        }

        [HttpGet("enquiries/{id}")]
        public async System.Threading.Tasks.Task<IActionResult> GetEnquiryById(Guid id)
        {
            return Ok(await _crmService.GetEnquiryByIdAsync(id));
        }

        [HttpPost("enquiries")]
        public async System.Threading.Tasks.Task<IActionResult> CreateEnquiry([FromBody] CoachOS.Application.Features.Crm.Dtos.CreateEnquiryRequest request)
        {
            return Ok(await _crmService.CreateEnquiryAsync(request));
        }

        [HttpPut("enquiries/{id}")]
        public async System.Threading.Tasks.Task<IActionResult> UpdateEnquiry(Guid id, [FromBody] CoachOS.Application.Features.Crm.Dtos.UpdateEnquiryRequest request)
        {
            return Ok(await _crmService.UpdateEnquiryAsync(id, request));
        }

        [HttpDelete("enquiries/{id}")]
        public async System.Threading.Tasks.Task<IActionResult> DeleteEnquiry(Guid id)
        {
            return Ok(await _crmService.DeleteEnquiryAsync(id));
        }

        [HttpPost("followups")]
        public IActionResult AddFollowUp([FromBody] object request) => Ok(new { Success = true, Message = "FollowUp added" });

        [HttpPost("democlasses")]
        public IActionResult ScheduleDemo([FromBody] object request) => Ok(new { Success = true, Message = "Demo class scheduled" });

        [HttpPost("import")]
        public IActionResult ImportEnquiries() => Ok(new { Success = true, Message = "Import enquiries from Excel" });

        [HttpGet("export")]
        public IActionResult ExportEnquiries() => Ok(new { Success = true, Message = "Export enquiries to Excel" });
    }
}

using CoachOS.Application.Features.Admissions.Dtos;
using CoachOS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "BRANCH_ADMIN,INSTITUTE_ADMIN,GLOBAL_ADMIN,SUPER_ADMIN")]
    public class AdmissionsController : ControllerBase
    {
        private readonly IAdmissionsService _admissionsService;

        public AdmissionsController(IAdmissionsService admissionsService)
        {
            _admissionsService = admissionsService;
        }

        [HttpPost("quick-admission")]
        public async Task<IActionResult> QuickAdmission([FromBody] QuickAdmissionRequest request)
        {
            var studentId = await _admissionsService.QuickAdmissionAsync(request);
            return Ok(new { success = true, studentId, message = "Quick admission successful" });
        }

        [HttpPost("full-admission")]
        public async Task<IActionResult> FullAdmission([FromBody] FullAdmissionRequest request)
        {
            var studentId = await _admissionsService.FullAdmissionAsync(request);
            return Ok(new { success = true, studentId, message = "Full admission successful" });
        }

        [HttpGet("{id}/profile")]
        public async Task<IActionResult> GetStudentProfile(Guid id)
        {
            var profile = await _admissionsService.GetStudentProfileAsync(id);
            return Ok(profile);
        }

        [HttpGet("{id}/batch-history")]
        public async Task<IActionResult> GetBatchHistory(Guid id)
        {
            var history = await _admissionsService.GetStudentBatchHistoryAsync(id);
            return Ok(history);
        }

        [HttpGet("{id}/fee-history")]
        public async Task<IActionResult> GetFeeHistory(Guid id)
        {
            var history = await _admissionsService.GetStudentFeeHistoryAsync(id);
            return Ok(history);
        }

        [HttpPost("{id}/transfer-batch")]
        public async Task<IActionResult> TransferBatch(Guid id, [FromBody] TransferBatchRequest request)
        {
            await _admissionsService.TransferBatchAsync(id, request.NewBatchId);
            return Ok(new { success = true, message = "Batch transferred successfully" });
        }
    }

    public class TransferBatchRequest
    {
        public Guid NewBatchId { get; set; }
    }
}

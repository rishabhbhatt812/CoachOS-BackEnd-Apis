using CoachOS.Api.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoachOS.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "INSTITUTE_ADMIN,GLOBAL_ADMIN,SUPER_ADMIN,ACCOUNTANT,ADMIN,BRANCH_ADMIN")]
    [ModuleAccess("FEES")]
    public class FeesController : ControllerBase
    {
        private readonly CoachOS.Application.Interfaces.Services.IFeeService _feeService;

        public FeesController(CoachOS.Application.Interfaces.Services.IFeeService feeService)
        {
            _feeService = feeService;
        }

        [HttpGet("plans")]
        public async System.Threading.Tasks.Task<IActionResult> GetFeePlans([FromQuery] CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            return Ok(await _feeService.GetFeePlansAsync(paginationParams));
        }

        [HttpGet("plans/{id}")]
        public async System.Threading.Tasks.Task<IActionResult> GetFeePlanById(Guid id)
        {
            return Ok(await _feeService.GetFeePlanByIdAsync(id));
        }

        [HttpPost("plans")]
        public async System.Threading.Tasks.Task<IActionResult> CreateFeePlan([FromBody] CoachOS.Application.Features.Finance.Dtos.CreateFeePlanRequest request)
        {
            return Ok(await _feeService.CreateFeePlanAsync(request));
        }

        [HttpDelete("plans/{id}")]
        public async System.Threading.Tasks.Task<IActionResult> DeleteFeePlan(Guid id)
        {
            return Ok(await _feeService.DeleteFeePlanAsync(id));
        }
    }
}

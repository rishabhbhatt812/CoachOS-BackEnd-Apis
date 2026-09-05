using CoachOS.Api.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoachOS.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "INSTITUTE_ADMIN,GLOBAL_ADMIN,SUPER_ADMIN,ADMIN,BRANCH_ADMIN")]
    [ModuleAccess("ATTENDANCE")]
    public class AttendanceController : ControllerBase
    {
        private readonly CoachOS.Application.Interfaces.Services.IAttendanceService _attendanceService;

        public AttendanceController(CoachOS.Application.Interfaces.Services.IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet("sessions")]
        public async System.Threading.Tasks.Task<IActionResult> GetAttendanceSessions([FromQuery] CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            return Ok(await _attendanceService.GetAttendanceSessionsAsync(paginationParams));
        }

        [HttpPost("sessions")]
        public async System.Threading.Tasks.Task<IActionResult> CreateSession([FromBody] CoachOS.Application.Features.Learning.Dtos.CreateAttendanceSessionRequest request)
        {
            return Ok(await _attendanceService.CreateAttendanceSessionAsync(request));
        }

        [HttpDelete("sessions/{id}")]
        public async System.Threading.Tasks.Task<IActionResult> DeleteSession(Guid id)
        {
            return Ok(await _attendanceService.DeleteAttendanceSessionAsync(id));
        }
    }
}

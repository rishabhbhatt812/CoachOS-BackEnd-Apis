using CoachOS.Api.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Teacher
{
    [ApiController]
    [Route("api/teacher/[controller]")]
    [Authorize(Roles = "TEACHER,INSTITUTE_ADMIN,BRANCH_ADMIN,SUPER_ADMIN,GLOBAL_ADMIN")]
    [ModuleAccess("ATTENDANCE")]
    public class TeacherAttendanceController : ControllerBase
    {
        private readonly CoachOS.Application.Interfaces.Services.IAttendanceService _attendanceService;

        public TeacherAttendanceController(CoachOS.Application.Interfaces.Services.IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet("sessions")]
        public async Task<IActionResult> GetAttendanceSessions([FromQuery] CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            return Ok(await _attendanceService.GetAttendanceSessionsAsync(paginationParams));
        }

        [HttpPost("sessions")]
        public async Task<IActionResult> CreateSession([FromBody] CoachOS.Application.Features.Learning.Dtos.CreateAttendanceSessionRequest request)
        {
            return Ok(await _attendanceService.CreateAttendanceSessionAsync(request));
        }

        [HttpDelete("sessions/{id}")]
        public async Task<IActionResult> DeleteSession(Guid id)
        {
            return Ok(await _attendanceService.DeleteAttendanceSessionAsync(id));
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveAttendance([FromBody] CoachOS.Application.Features.Learning.Dtos.SaveBatchAttendanceRequest request)
        {
            return Ok(await _attendanceService.SaveAttendanceAsync(request));
        }
    }
}

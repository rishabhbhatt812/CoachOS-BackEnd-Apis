using CoachOS.Application.Features.Staff.Dtos;
using CoachOS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Common
{
    [ApiController]
    [Route("api/teachers")]
    [Authorize(Roles = "SUPER_ADMIN,GLOBAL_ADMIN,INSTITUTE_ADMIN,BRANCH_ADMIN,RECEPTIONIST,TEACHER")]
    public class TeachersController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public TeachersController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeachers()
        {
            var result = await _staffService.GetTeachersAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherById(Guid id)
        {
            var result = await _staffService.GetTeacherByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPut("{id}/profile")]
        public async Task<IActionResult> UpdateTeacherProfile(Guid id, [FromBody] UpdateTeacherProfileRequest request)
        {
            var result = await _staffService.UpdateTeacherProfileAsync(id, request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}

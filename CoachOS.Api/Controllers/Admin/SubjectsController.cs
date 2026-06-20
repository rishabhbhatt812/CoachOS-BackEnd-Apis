using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoachOS.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "BRANCH_ADMIN,INSTITUTE_ADMIN,GLOBAL_ADMIN,SUPER_ADMIN")]
    public class SubjectsController : ControllerBase
    {
        private readonly CoachOS.Application.Interfaces.Services.IAcademicsService _academicsService;

        public SubjectsController(CoachOS.Application.Interfaces.Services.IAcademicsService academicsService)
        {
            _academicsService = academicsService;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> GetAll([FromQuery] CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            return Ok(await _academicsService.GetSubjectsAsync(paginationParams));
        }

        [HttpGet("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _academicsService.GetSubjectByIdAsync(id));
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Create([FromBody] CoachOS.Application.Features.Academics.Dtos.CreateSubjectRequest request)
        {
            return Ok(await _academicsService.CreateSubjectAsync(request));
        }

        [HttpPut("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> Update(Guid id, [FromBody] CoachOS.Application.Features.Academics.Dtos.UpdateSubjectRequest request)
        {
            return Ok(await _academicsService.UpdateSubjectAsync(id, request));
        }

        [HttpDelete("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _academicsService.DeleteSubjectAsync(id));
        }
    }
}

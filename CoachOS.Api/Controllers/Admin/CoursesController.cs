using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoachOS.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly CoachOS.Application.Interfaces.Services.IAcademicsService _academicsService;

        public CoursesController(CoachOS.Application.Interfaces.Services.IAcademicsService academicsService)
        {
            _academicsService = academicsService;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> GetAll([FromQuery] CoachOS.Shared.Requests.PaginationParams? paginationParams)
        {
            var res = await _academicsService.GetCoursesAsync(paginationParams ?? new CoachOS.Shared.Requests.PaginationParams { PageNumber = 1, PageSize = 100 });
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _academicsService.GetCourseByIdAsync(id));
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Create([FromBody] CoachOS.Application.Features.Academics.Dtos.CreateCourseRequest request)
        {
            return Ok(await _academicsService.CreateCourseAsync(request));
        }

        [HttpPut("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> Update(Guid id, [FromBody] CoachOS.Application.Features.Academics.Dtos.UpdateCourseRequest request)
        {
            return Ok(await _academicsService.UpdateCourseAsync(id, request));
        }

        [HttpDelete("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _academicsService.DeleteCourseAsync(id));
        }
    }
}

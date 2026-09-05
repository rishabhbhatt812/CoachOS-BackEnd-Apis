using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoachOS.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "INSTITUTE_ADMIN,GLOBAL_ADMIN,SUPER_ADMIN,ADMIN,BRANCH_ADMIN")]
    public class BatchesController : ControllerBase
    {
        private readonly CoachOS.Application.Interfaces.Services.IAcademicsService _academicsService;

        public BatchesController(CoachOS.Application.Interfaces.Services.IAcademicsService academicsService)
        {
            _academicsService = academicsService;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> GetAll([FromQuery] CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            return Ok(await _academicsService.GetBatchesAsync(paginationParams));
        }

        [HttpGet("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _academicsService.GetBatchByIdAsync(id));
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Create([FromBody] CoachOS.Application.Features.Academics.Dtos.CreateBatchRequest request)
        {
            return Ok(await _academicsService.CreateBatchAsync(request));
        }

        [HttpPut("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> Update(Guid id, [FromBody] CoachOS.Application.Features.Academics.Dtos.UpdateBatchRequest request)
        {
            return Ok(await _academicsService.UpdateBatchAsync(id, request));
        }

        [HttpDelete("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _academicsService.DeleteBatchAsync(id));
        }
    }
}

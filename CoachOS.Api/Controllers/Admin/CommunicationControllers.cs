using CoachOS.Api.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "INSTITUTE_ADMIN,GLOBAL_ADMIN,SUPER_ADMIN,RECEPTIONIST")]
    [ModuleAccess("COMMUNICATION")]
    public class NoticesController : ControllerBase
    {
        private readonly CoachOS.Application.Interfaces.Services.ICommunicationService _communicationService;

        public NoticesController(CoachOS.Application.Interfaces.Services.ICommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            return Ok(await _communicationService.GetNoticesAsync(paginationParams));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CoachOS.Application.Features.Communication.Dtos.CreateNoticeRequest request)
        {
            return Ok(await _communicationService.CreateNoticeAsync(request));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _communicationService.DeleteNoticeAsync(id));
        }
    }
}

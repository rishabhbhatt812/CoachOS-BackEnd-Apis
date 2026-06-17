using CoachOS.Api.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Teacher
{
    [ApiController]
    [Route("api/teacher/results")]
    [Authorize(Roles = "TEACHER")]
    [ModuleAccess("LEARNING")]
    public class TeacherResultsController : ControllerBase
    {
        [HttpGet("{testId}")]
        public async Task<IActionResult> GetResults(Guid testId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("save-draft")]
        public async Task<IActionResult> SaveDraft([FromBody] object request)
        {
            throw new NotImplementedException();
        }

        [HttpPost("publish")]
        public async Task<IActionResult> PublishResult([FromBody] object request)
        {
            throw new NotImplementedException();
        }
    }
}

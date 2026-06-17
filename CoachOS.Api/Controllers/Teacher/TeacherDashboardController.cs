using CoachOS.Api.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Teacher
{
    [ApiController]
    [Route("api/teacher/dashboard")]
    [Authorize(Roles = "TEACHER")]
    public class TeacherDashboardController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetDashboardSummary()
        {
            throw new System.NotImplementedException();
        }
    }
}

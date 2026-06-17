using CoachOS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "INSTITUTE_ADMIN,GLOBAL_ADMIN,SUPER_ADMIN")]
    public class DashboardController : ControllerBase
    {
        private readonly IReportService _reportService;

        public DashboardController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetMetrics()
        {
            var metrics = await _reportService.GetDashboardMetricsAsync();
            return Ok(new { Success = true, Data = metrics });
        }

        [HttpGet("global-metrics")]
        [Authorize(Roles = "GLOBAL_ADMIN,SUPER_ADMIN")]
        public async Task<IActionResult> GetGlobalMetrics()
        {
            var metrics = await _reportService.GetGlobalMetricsAsync();
            return Ok(new { Success = true, Data = metrics });
        }
    }
}

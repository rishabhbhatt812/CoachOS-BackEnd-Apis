using CoachOS.Api.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Student
{
    [ApiController]
    [Route("api/student/portal")]
    [Authorize(Roles = "STUDENT")]
    [ModuleAccess("STUDENT_PORTAL")]
    public class StudentPortalController : ControllerBase
    {
        private readonly CoachOS.Application.Interfaces.Services.IStudentPortalService _studentPortalService;
        private readonly CoachOS.Application.Interfaces.Services.ICurrentUserService _currentUserService;

        public StudentPortalController(
            CoachOS.Application.Interfaces.Services.IStudentPortalService studentPortalService,
            CoachOS.Application.Interfaces.Services.ICurrentUserService currentUserService)
        {
            _studentPortalService = studentPortalService;
            _currentUserService = currentUserService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            return Ok(await _studentPortalService.GetStudentDashboardAsync(_currentUserService.UserId ?? Guid.Empty));
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetCourses()
        {
            return Ok(await _studentPortalService.GetStudentCoursesAsync(_currentUserService.UserId ?? Guid.Empty));
        }

        [HttpGet("fees")]
        public async Task<IActionResult> GetFees()
        {
            return Ok(await _studentPortalService.GetStudentFeesAsync(_currentUserService.UserId ?? Guid.Empty));
        }

        [HttpGet("notes")]
        public async Task<IActionResult> GetNotes()
        {
            return Ok(await _studentPortalService.GetStudentNotesAsync(_currentUserService.UserId ?? Guid.Empty));
        }

        [HttpGet("attendance")]
        public async Task<IActionResult> GetAttendance()
        {
            return Ok(await _studentPortalService.GetStudentAttendanceAsync(_currentUserService.UserId ?? Guid.Empty));
        }

        [HttpGet("results")]
        public async Task<IActionResult> GetResults()
        {
            return Ok(await _studentPortalService.GetStudentResultsAsync(_currentUserService.UserId ?? Guid.Empty));
        }

        [HttpGet("vacancies")]
        public async Task<IActionResult> GetVacancies()
        {
            return Ok(await _studentPortalService.GetStudentVacanciesAsync(_currentUserService.UserId ?? Guid.Empty));
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var studentId = _currentUserService.UserId ?? Guid.Empty;
            var admissionsService = HttpContext.RequestServices.GetRequiredService<CoachOS.Application.Interfaces.Services.IAdmissionsService>();
            var profile = await admissionsService.GetStudentProfileAsync(studentId);
            return Ok(profile);
        }

        [HttpPost("profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(CoachOS.Shared.Responses.ApiResponse<string>.Fail("No file uploaded."));

            var extension = System.IO.Path.GetExtension(file.FileName).ToLower();
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".webp" };
            if (!System.Linq.Enumerable.Contains(allowedExtensions, extension))
                return BadRequest(CoachOS.Shared.Responses.ApiResponse<string>.Fail("Invalid file type. Only PNG, JPG, JPEG, and WEBP are allowed."));

            var studentId = _currentUserService.UserId ?? Guid.Empty;
            var studentService = HttpContext.RequestServices.GetRequiredService<CoachOS.Application.Interfaces.Services.IStudentService>();
            
            using var stream = file.OpenReadStream();
            var result = await studentService.UploadProfilePictureAsync(studentId, stream, file.FileName);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}

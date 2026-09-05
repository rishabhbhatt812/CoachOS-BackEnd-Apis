using CoachOS.Api.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoachOS.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "INSTITUTE_ADMIN,GLOBAL_ADMIN,SUPER_ADMIN,ADMIN,BRANCH_ADMIN")]
    public class StudentsController : ControllerBase
    {
        private readonly CoachOS.Application.Interfaces.Services.IStudentService _studentService;

        public StudentsController(CoachOS.Application.Interfaces.Services.IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> GetAll([FromQuery] CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            return Ok(await _studentService.GetStudentsAsync(paginationParams));
        }

        [HttpGet("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _studentService.GetStudentByIdAsync(id));
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Create([FromBody] CoachOS.Application.Features.Students.Dtos.CreateStudentRequest request)
        {
            return Ok(await _studentService.CreateStudentAsync(request));
        }

        [HttpPut("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> Update(Guid id, [FromBody] CoachOS.Application.Features.Students.Dtos.UpdateStudentRequest request)
        {
            return Ok(await _studentService.UpdateStudentAsync(id, request));
        }

        [HttpDelete("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _studentService.DeleteStudentAsync(id));
        }

        [HttpGet("next-code")]
        public async System.Threading.Tasks.Task<IActionResult> GetNextCode([FromQuery] string prefix = "STU")
        {
            return Ok(await _studentService.GetNextStudentCodeAsync(prefix));
        }

        [HttpPost("import")]
        public IActionResult ImportExcel() => Ok(new { Success = true, Message = "Import students from Excel" });

        [HttpGet("export")]
        public IActionResult ExportExcel() => Ok(new { Success = true, Message = "Export students to Excel" });

        [HttpPost("{id}/profile-picture")]
        public async System.Threading.Tasks.Task<IActionResult> UploadProfilePicture(Guid id, Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(CoachOS.Shared.Responses.ApiResponse<string>.Fail("No file uploaded."));

            var extension = System.IO.Path.GetExtension(file.FileName).ToLower();
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".webp" };
            if (!System.Linq.Enumerable.Contains(allowedExtensions, extension))
                return BadRequest(CoachOS.Shared.Responses.ApiResponse<string>.Fail("Invalid file type. Only PNG, JPG, JPEG, and WEBP are allowed."));

            using var stream = file.OpenReadStream();
            var result = await _studentService.UploadProfilePictureAsync(id, stream, file.FileName);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}

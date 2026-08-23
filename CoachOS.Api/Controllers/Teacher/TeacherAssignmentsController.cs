using CoachOS.Api.Middlewares;
using CoachOS.Application.Features.Learning.Dtos;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Academic;
using CoachOS.Domain.Learning;
using CoachOS.Shared.Requests;
using CoachOS.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Teacher
{
    public class CreateAssignmentUploadModel
    {
        [FromForm(Name = "title")]
        public string Title { get; set; } = string.Empty;

        [FromForm(Name = "description")]
        public string? Description { get; set; }

        [FromForm(Name = "dueDate")]
        public DateTime DueDate { get; set; }

        [FromForm(Name = "batchId")]
        public Guid BatchId { get; set; }

        [FromForm(Name = "subjectId")]
        public Guid? SubjectId { get; set; }

        [FromForm(Name = "file")]
        public IFormFile? File { get; set; }
    }

    [ApiController]
    [Route("api/teacher/assignments")]
    [Authorize(Roles = "TEACHER,ADMIN,INSTITUTE_ADMIN,BRANCH_ADMIN,SUPER_ADMIN,GLOBAL_ADMIN")]
    [ModuleAccess("LEARNING")]
    public class TeacherAssignmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;

        public TeacherAssignmentsController(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams? paginationParams)
        {
            var assignments = await _unitOfWork.Repository<Assignment>().GetAllAsync();
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
            var batches = await _unitOfWork.Repository<Batch>().GetAllAsync();
            var subjects = await _unitOfWork.Repository<Subject>().GetAllAsync();

            var currentUserId = _currentUserService.UserId;
            var isTeacher = _currentUserService.RoleCode == "TEACHER";

            var filtered = assignments.Where(a => a.IsActive);
            if (isTeacher && currentUserId.HasValue)
            {
                filtered = filtered.Where(a => a.CreatedByUserId == currentUserId.Value || a.CreatedByUserId == Guid.Empty);
            }

            var dtos = filtered.OrderByDescending(a => a.CreatedAt).Select(a => new
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                DueDate = a.DueDate.ToString("yyyy-MM-dd"),
                CourseId = a.CourseId,
                BatchId = a.BatchId,
                SubjectId = a.SubjectId,
                CourseName = courses.FirstOrDefault(c => c.Id == a.CourseId)?.Name ?? "General Course",
                BatchName = batches.FirstOrDefault(b => b.Id == a.BatchId)?.Name ?? "All Batches",
                SubjectName = subjects.FirstOrDefault(s => s.Id == a.SubjectId)?.Name ?? "General Subject",
                FilePath = !string.IsNullOrEmpty(a.FilePath) ? $"/api/teacher/assignments/download/{a.Id}" : null,
                OriginalFileName = a.OriginalFileName,
                CreatedAt = a.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                Submissions = 0,
                TotalStudents = 30
            }).ToList();

            return Ok(ApiResponse<object>.Ok(dtos));
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateAssignmentUploadModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
                return BadRequest(ApiResponse<object>.Fail("Title is required."));

            if (model.BatchId == Guid.Empty)
                return BadRequest(ApiResponse<object>.Fail("Invalid Batch specified."));

            var batch = await _unitOfWork.Repository<Batch>().GetByIdAsync(model.BatchId);
            if (batch == null)
                return BadRequest(ApiResponse<object>.Fail("Invalid Batch specified."));

            string? savedRelativePath = null;
            string? originalFileName = null;

            if (model.File != null && model.File.Length > 0)
            {
                using (var stream = model.File.OpenReadStream())
                {
                    savedRelativePath = await _fileStorageService.SaveFileAsync(stream, model.File.FileName, "assignments");
                }
                originalFileName = model.File.FileName;
            }

            var assignment = new Assignment
            {
                Title = model.Title.Trim(),
                Description = model.Description?.Trim(),
                DueDate = model.DueDate != default ? model.DueDate : DateTime.UtcNow.AddDays(7),
                BatchId = model.BatchId,
                CourseId = batch.CourseId,
                SubjectId = model.SubjectId,
                FilePath = savedRelativePath ?? "",
                OriginalFileName = originalFileName ?? "",
                CreatedByUserId = _currentUserService.UserId ?? Guid.Empty,
                IsActive = true
            };

            await _unitOfWork.Repository<Assignment>().AddAsync(assignment);
            await _unitOfWork.SaveChangesAsync();

            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(batch.CourseId);

            var dto = new
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                DueDate = assignment.DueDate.ToString("yyyy-MM-dd"),
                CourseId = assignment.CourseId,
                BatchId = assignment.BatchId,
                CourseName = course?.Name ?? "General",
                BatchName = batch.Name,
                FilePath = !string.IsNullOrEmpty(assignment.FilePath) ? $"/api/teacher/assignments/download/{assignment.Id}" : null,
                OriginalFileName = assignment.OriginalFileName,
                CreatedAt = assignment.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return Ok(ApiResponse<object>.Ok(dto, "Assignment created successfully."));
        }

        [HttpGet("download/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Download(Guid id)
        {
            var assignment = await _unitOfWork.Repository<Assignment>().GetByIdAsync(id);
            if (assignment == null)
            {
                return NotFound("Assignment not found.");
            }

            if (string.IsNullOrEmpty(assignment.FilePath))
            {
                return NotFound("No document attached to this assignment.");
            }

            try
            {
                var fileBytes = await _fileStorageService.GetFileAsync(assignment.FilePath);
                var ext = Path.GetExtension(assignment.FilePath).ToLower();
                var contentType = ext switch
                {
                    ".pdf" => "application/pdf",
                    ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".zip" => "application/zip",
                    _ => "application/octet-stream"
                };

                return File(fileBytes, contentType, assignment.OriginalFileName ?? "Assignment_Document.pdf");
            }
            catch (FileNotFoundException)
            {
                return NotFound("Physical file not found on server.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving file: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var assignment = await _unitOfWork.Repository<Assignment>().GetByIdAsync(id);
            if (assignment == null)
                return NotFound(ApiResponse<bool>.Fail("Assignment not found."));

            try
            {
                if (!string.IsNullOrEmpty(assignment.FilePath))
                {
                    _fileStorageService.DeleteFile(assignment.FilePath);
                }
            }
            catch
            {
                // Ignore file system deletion error
            }

            _unitOfWork.Repository<Assignment>().Remove(assignment);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<bool>.Ok(true, "Assignment deleted successfully."));
        }
    }
}

using CoachOS.Api.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Teacher
{
    public class CreateNoteUploadModel
    {
        [FromForm(Name = "title")]
        public string Title { get; set; } = string.Empty;

        [FromForm(Name = "description")]
        public string? Description { get; set; }

        [FromForm(Name = "batchId")]
        public Guid BatchId { get; set; }

        [FromForm(Name = "file")]
        public Microsoft.AspNetCore.Http.IFormFile File { get; set; } = null!;
    }

    [ApiController]
    [Route("api/teacher/notes")]
    [Authorize(Roles = "TEACHER,INSTITUTE_ADMIN,BRANCH_ADMIN,SUPER_ADMIN,GLOBAL_ADMIN")]
    [ModuleAccess("LEARNING")]
    public class TeacherNotesController : ControllerBase
    {
        private readonly CoachOS.Application.Interfaces.Services.ILearningService _learningService;
        private readonly CoachOS.Application.Interfaces.Services.IFileStorageService _fileStorageService;
        private readonly CoachOS.Application.Interfaces.Services.ICurrentUserService _currentUserService;
        private readonly CoachOS.Application.Interfaces.Repositories.IUnitOfWork _unitOfWork;

        public TeacherNotesController(
            CoachOS.Application.Interfaces.Services.ILearningService learningService,
            CoachOS.Application.Interfaces.Services.IFileStorageService fileStorageService,
            CoachOS.Application.Interfaces.Services.ICurrentUserService currentUserService,
            CoachOS.Application.Interfaces.Repositories.IUnitOfWork unitOfWork)
        {
            _learningService = learningService;
            _fileStorageService = fileStorageService;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            return Ok(await _learningService.GetNotesAsync(paginationParams));
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateNoteUploadModel model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }

            var userId = _currentUserService.UserId ?? Guid.Empty;

            var batch = await _unitOfWork.Repository<CoachOS.Domain.Academic.Batch>().GetByIdAsync(model.BatchId);
            if (batch == null)
            {
                return BadRequest("Invalid Batch ID.");
            }

            var courseId = batch.CourseId;

            Guid? subjectId = null;
            var teacherBatches = await _unitOfWork.Repository<CoachOS.Domain.Identity.TeacherBatch>().GetAllAsync();
            var matchingTB = teacherBatches.FirstOrDefault(tb => tb.BatchId == model.BatchId && tb.IsActive);
            if (matchingTB != null)
            {
                subjectId = matchingTB.SubjectId;
            }
            else
            {
                var subjects = await _unitOfWork.Repository<CoachOS.Domain.Academic.Subject>().GetAllAsync();
                var courseSubject = subjects.FirstOrDefault(s => s.CourseId == courseId && s.IsActive);
                if (courseSubject != null)
                {
                    subjectId = courseSubject.Id;
                }
            }

            if (!subjectId.HasValue || subjectId == Guid.Empty)
            {
                return BadRequest("No subject is associated with this batch. Please contact the administrator.");
            }

            string relativePath;
            using (var stream = model.File.OpenReadStream())
            {
                relativePath = await _fileStorageService.SaveFileAsync(stream, model.File.FileName, "notes");
            }

            var req = new CoachOS.Application.Features.Learning.Dtos.CreateNoteRequest
            {
                Title = model.Title,
                Description = model.Description,
                FilePath = relativePath,
                OriginalFileName = model.File.FileName,
                StoredFileName = System.IO.Path.GetFileName(relativePath),
                FileType = model.File.ContentType,
                FileSizeInBytes = model.File.Length,
                CourseId = courseId,
                BatchId = model.BatchId,
                SubjectId = subjectId.Value,
                UploadedByUserId = userId
            };

            return Ok(await _learningService.CreateNoteAsync(req));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _learningService.DeleteNoteAsync(id));
        }
    }

    [ApiController]
    [Route("api/teacher/tests")]
    [Authorize(Roles = "TEACHER,INSTITUTE_ADMIN,BRANCH_ADMIN,SUPER_ADMIN,GLOBAL_ADMIN")]
    [ModuleAccess("LEARNING")]
    public class TeacherTestsController : ControllerBase
    {
        private readonly CoachOS.Application.Interfaces.Services.ILearningService _learningService;

        public TeacherTestsController(CoachOS.Application.Interfaces.Services.ILearningService learningService)
        {
            _learningService = learningService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            return Ok(await _learningService.GetTestsAsync(paginationParams));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CoachOS.Application.Features.Learning.Dtos.CreateTestRequest request)
        {
            return Ok(await _learningService.CreateTestAsync(request));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _learningService.DeleteTestAsync(id));
        }
    }
}

using CoachOS.Api.Middlewares;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Academic;
using CoachOS.Domain.Identity;
using CoachOS.Domain.Learning;
using CoachOS.Shared.Requests;
using CoachOS.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
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
        public IFormFile File { get; set; } = null!;
    }

    [ApiController]
    [Route("api/teacher/notes")]
    [Authorize(Roles = "TEACHER,ADMIN,INSTITUTE_ADMIN,BRANCH_ADMIN,SUPER_ADMIN,GLOBAL_ADMIN")]
    [ModuleAccess("LEARNING")]
    public class TeacherNotesController : ControllerBase
    {
        private readonly ILearningService _learningService;
        private readonly IFileStorageService _fileStorageService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public TeacherNotesController(
            ILearningService learningService,
            IFileStorageService fileStorageService,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _learningService = learningService;
            _fileStorageService = fileStorageService;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams? paginationParams)
        {
            var notes = await _unitOfWork.Repository<Note>().GetAllAsync();
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
            var batches = await _unitOfWork.Repository<Batch>().GetAllAsync();
            var subjects = await _unitOfWork.Repository<Subject>().GetAllAsync();

            var currentUserId = _currentUserService.UserId;
            var isTeacher = _currentUserService.RoleCode == "TEACHER";

            var filteredNotes = notes.Where(n => n.IsActive);
            if (isTeacher && currentUserId.HasValue)
            {
                filteredNotes = filteredNotes.Where(n => n.UploadedByUserId == currentUserId.Value || n.UploadedByUserId == Guid.Empty);
            }

            var dtos = filteredNotes.OrderByDescending(n => n.CreatedAt).Select(n =>
            {
                var batch = batches.FirstOrDefault(b => b.Id == n.BatchId);
                var course = courses.FirstOrDefault(c => c.Id == (n.CourseId ?? batch?.CourseId));
                var subject = subjects.FirstOrDefault(s => s.Id == n.SubjectId);

                return new
                {
                    n.Id,
                    n.Title,
                    n.Description,
                    n.OriginalFileName,
                    n.StoredFileName,
                    FilePath = $"/api/notes/download/{n.Id}",
                    n.FileType,
                    n.FileSizeInBytes,
                    CourseId = course?.Id ?? n.CourseId,
                    CourseName = course?.Name ?? "General Course",
                    BatchId = n.BatchId,
                    BatchName = batch?.Name ?? "All Batches",
                    SubjectId = n.SubjectId,
                    SubjectName = subject?.Name ?? "General Subject",
                    CreatedAt = n.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    UploadedByUserId = n.UploadedByUserId
                };
            }).ToList();

            return Ok(ApiResponse<object>.Ok(dtos));
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateNoteUploadModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                return BadRequest(ApiResponse<object>.Fail("Resource title is required."));
            }

            if (model.BatchId == Guid.Empty)
            {
                return BadRequest(ApiResponse<object>.Fail("Target batch is required."));
            }

            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest(ApiResponse<object>.Fail("Please select a document file to upload."));
            }

            var userId = _currentUserService.UserId ?? Guid.Empty;

            var batch = await _unitOfWork.Repository<Batch>().GetByIdAsync(model.BatchId);
            if (batch == null)
            {
                return BadRequest(ApiResponse<object>.Fail("Selected batch does not exist."));
            }

            var courseId = batch.CourseId;

            Guid? subjectId = null;
            var teacherBatches = await _unitOfWork.Repository<TeacherBatch>().GetAllAsync();
            var matchingTB = teacherBatches.FirstOrDefault(tb => tb.BatchId == model.BatchId && tb.IsActive);
            if (matchingTB != null)
            {
                subjectId = matchingTB.SubjectId;
            }
            else
            {
                var subjects = await _unitOfWork.Repository<Subject>().GetAllAsync();
                var courseSubject = subjects.FirstOrDefault(s => s.CourseId == courseId && s.IsActive);
                if (courseSubject != null)
                {
                    subjectId = courseSubject.Id;
                }
            }

            string relativePath;
            using (var stream = model.File.OpenReadStream())
            {
                relativePath = await _fileStorageService.SaveFileAsync(stream, model.File.FileName, "notes");
            }

            var note = new Note
            {
                Title = model.Title.Trim(),
                Description = model.Description?.Trim(),
                BatchId = model.BatchId,
                CourseId = courseId,
                SubjectId = subjectId,
                FilePath = relativePath,
                OriginalFileName = model.File.FileName,
                StoredFileName = Path.GetFileName(relativePath),
                FileType = model.File.ContentType ?? "application/octet-stream",
                FileSizeInBytes = model.File.Length,
                UploadedByUserId = userId,
                IsActive = true
            };

            await _unitOfWork.Repository<Note>().AddAsync(note);
            await _unitOfWork.SaveChangesAsync();

            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(courseId);

            var responseData = new
            {
                note.Id,
                note.Title,
                note.Description,
                note.OriginalFileName,
                FilePath = $"/api/notes/download/{note.Id}",
                note.FileType,
                note.FileSizeInBytes,
                CourseName = course?.Name ?? "General Course",
                BatchName = batch.Name,
                CreatedAt = note.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return Ok(ApiResponse<object>.Ok(responseData, "Study material uploaded and distributed successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var note = await _unitOfWork.Repository<Note>().GetByIdAsync(id);
            if (note == null)
            {
                return NotFound(ApiResponse<bool>.Fail("Study material not found."));
            }

            try
            {
                if (!string.IsNullOrEmpty(note.FilePath))
                {
                    _fileStorageService.DeleteFile(note.FilePath);
                }
            }
            catch
            {
                // Ignore file system deletion error if file does not exist
            }

            _unitOfWork.Repository<Note>().Remove(note);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<bool>.Ok(true, "Study material deleted successfully."));
        }
    }

    [ApiController]
    [Route("api/teacher/tests")]
    [Authorize(Roles = "TEACHER,ADMIN,INSTITUTE_ADMIN,BRANCH_ADMIN,SUPER_ADMIN,GLOBAL_ADMIN")]
    [ModuleAccess("LEARNING")]
    public class TeacherTestsController : ControllerBase
    {
        private readonly ILearningService _learningService;

        public TeacherTestsController(ILearningService learningService)
        {
            _learningService = learningService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
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

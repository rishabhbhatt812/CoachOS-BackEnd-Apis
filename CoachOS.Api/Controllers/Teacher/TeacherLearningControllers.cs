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
using System.Collections.Generic;
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

    public class CreateTestModel
    {
        public string TestName { get; set; } = string.Empty;
        public DateTime TestDate { get; set; }
        public decimal MaxMarks { get; set; }
        public Guid BatchId { get; set; }
        public Guid? CourseId { get; set; }
        public Guid? SubjectId { get; set; }
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public TeacherTestsController(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams? paginationParams)
        {
            var tests = await _unitOfWork.Repository<Test>().GetAllAsync();
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
            var batches = await _unitOfWork.Repository<Batch>().GetAllAsync();
            var subjects = await _unitOfWork.Repository<Subject>().GetAllAsync();

            var dtos = tests.OrderByDescending(t => t.TestDate).Select(t =>
            {
                var batch = batches.FirstOrDefault(b => b.Id == t.BatchId);
                var course = courses.FirstOrDefault(c => c.Id == (t.CourseId != Guid.Empty ? t.CourseId : batch?.CourseId));
                var subject = subjects.FirstOrDefault(s => s.Id == t.SubjectId);

                return new
                {
                    Id = t.Id,
                    TestName = t.TestName,
                    TestDate = t.TestDate.ToString("yyyy-MM-dd"),
                    MaxMarks = t.MaxMarks,
                    CourseId = course?.Id ?? t.CourseId,
                    CourseName = course?.Name ?? "General Course",
                    BatchId = t.BatchId,
                    BatchName = batch?.Name ?? "All Batches",
                    SubjectId = t.SubjectId,
                    SubjectName = subject?.Name ?? "General Subject"
                };
            }).ToList();

            return Ok(ApiResponse<object>.Ok(dtos));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTestModel request)
        {
            if (string.IsNullOrWhiteSpace(request.TestName))
                return BadRequest(ApiResponse<object>.Fail("Test name is required."));

            if (request.BatchId == Guid.Empty)
                return BadRequest(ApiResponse<object>.Fail("Target batch is required."));

            var batch = await _unitOfWork.Repository<Batch>().GetByIdAsync(request.BatchId);
            if (batch == null)
                return BadRequest(ApiResponse<object>.Fail("Invalid Batch specified."));

            var courseId = request.CourseId.HasValue && request.CourseId.Value != Guid.Empty ? request.CourseId.Value : batch.CourseId;
            var testDate = request.TestDate != default ? DateOnly.FromDateTime(request.TestDate) : DateOnly.FromDateTime(DateTime.UtcNow);

            var test = new Test
            {
                TestName = request.TestName.Trim(),
                TestDate = testDate,
                MaxMarks = request.MaxMarks > 0 ? request.MaxMarks : 100,
                BatchId = request.BatchId,
                CourseId = courseId,
                SubjectId = request.SubjectId
            };

            await _unitOfWork.Repository<Test>().AddAsync(test);
            await _unitOfWork.SaveChangesAsync();

            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(courseId);
            var subject = request.SubjectId.HasValue ? await _unitOfWork.Repository<Subject>().GetByIdAsync(request.SubjectId.Value) : null;

            var dto = new
            {
                Id = test.Id,
                TestName = test.TestName,
                TestDate = test.TestDate.ToString("yyyy-MM-dd"),
                MaxMarks = test.MaxMarks,
                CourseName = course?.Name ?? "General Course",
                BatchName = batch.Name,
                SubjectName = subject?.Name ?? "General Subject"
            };

            return Ok(ApiResponse<object>.Ok(dto, "Test scheduled successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateTestModel request)
        {
            var test = await _unitOfWork.Repository<Test>().GetByIdAsync(id);
            if (test == null)
                return NotFound(ApiResponse<bool>.Fail("Test not found."));

            if (!string.IsNullOrWhiteSpace(request.TestName))
                test.TestName = request.TestName.Trim();

            if (request.MaxMarks > 0)
                test.MaxMarks = request.MaxMarks;

            if (request.TestDate != default)
                test.TestDate = DateOnly.FromDateTime(request.TestDate);

            if (request.BatchId != Guid.Empty)
            {
                test.BatchId = request.BatchId;
                var batch = await _unitOfWork.Repository<Batch>().GetByIdAsync(request.BatchId);
                if (batch != null)
                {
                    test.CourseId = batch.CourseId;
                }
            }

            if (request.SubjectId.HasValue)
                test.SubjectId = request.SubjectId;

            _unitOfWork.Repository<Test>().Update(test);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<bool>.Ok(true, "Test updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var test = await _unitOfWork.Repository<Test>().GetByIdAsync(id);
            if (test == null)
                return NotFound(ApiResponse<bool>.Fail("Test not found."));

            var results = (await _unitOfWork.Repository<TestResult>().GetAllAsync())
                .Where(r => r.TestId == id).ToList();
            foreach (var r in results)
            {
                _unitOfWork.Repository<TestResult>().Remove(r);
            }

            _unitOfWork.Repository<Test>().Remove(test);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<bool>.Ok(true, "Test deleted successfully."));
        }
    }
}

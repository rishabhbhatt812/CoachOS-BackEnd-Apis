using CoachOS.Api.Middlewares;
using CoachOS.Application.Features.Learning.Dtos;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Academic;
using CoachOS.Domain.Learning;
using CoachOS.Shared.Requests;
using CoachOS.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Teacher
{
    [ApiController]
    [Route("api/teacher/assignments")]
    [Authorize(Roles = "TEACHER,INSTITUTE_ADMIN,BRANCH_ADMIN,SUPER_ADMIN,GLOBAL_ADMIN")]
    [ModuleAccess("LEARNING")]
    public class TeacherAssignmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public TeacherAssignmentsController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var paged = await _unitOfWork.Repository<Assignment>().GetPagedAsync(paginationParams);
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
            var batches = await _unitOfWork.Repository<Batch>().GetAllAsync();
            var subjects = await _unitOfWork.Repository<Subject>().GetAllAsync();

            var dtos = paged.Data.Select(a => new AssignmentDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                DueDate = a.DueDate,
                CourseId = a.CourseId,
                BatchId = a.BatchId,
                SubjectId = a.SubjectId,
                CourseName = courses.FirstOrDefault(c => c.Id == a.CourseId)?.Name ?? "General",
                BatchName = batches.FirstOrDefault(b => b.Id == a.BatchId)?.Name ?? "All Batches",
                SubjectName = subjects.FirstOrDefault(s => s.Id == a.SubjectId)?.Name ?? "General",
                FilePath = a.FilePath,
                OriginalFileName = a.OriginalFileName,
                CreatedAt = a.CreatedAt,
                SubmissionCount = 0
            }).ToList();

            var result = new PagedResult<AssignmentDto>(dtos, paged.TotalCount, paged.CurrentPage, paged.PageSize);
            return Ok(ApiResponse<PagedResult<AssignmentDto>>.Ok(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAssignmentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                return BadRequest(ApiResponse<AssignmentDto>.Fail("Title is required."));

            var batch = await _unitOfWork.Repository<Batch>().GetByIdAsync(request.BatchId);
            if (batch == null)
                return BadRequest(ApiResponse<AssignmentDto>.Fail("Invalid Batch specified."));

            var assignment = new Assignment
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                BatchId = request.BatchId,
                CourseId = batch.CourseId,
                SubjectId = request.SubjectId,
                FilePath = request.FilePath,
                OriginalFileName = request.OriginalFileName,
                CreatedByUserId = _currentUserService.UserId ?? Guid.Empty,
                IsActive = true
            };

            await _unitOfWork.Repository<Assignment>().AddAsync(assignment);
            await _unitOfWork.SaveChangesAsync();

            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(batch.CourseId);
            var subject = request.SubjectId.HasValue ? await _unitOfWork.Repository<Subject>().GetByIdAsync(request.SubjectId.Value) : null;

            var dto = new AssignmentDto
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                DueDate = assignment.DueDate,
                CourseId = assignment.CourseId,
                BatchId = assignment.BatchId,
                SubjectId = assignment.SubjectId,
                CourseName = course?.Name ?? "General",
                BatchName = batch.Name,
                SubjectName = subject?.Name ?? "General",
                FilePath = assignment.FilePath,
                OriginalFileName = assignment.OriginalFileName,
                CreatedAt = assignment.CreatedAt,
                SubmissionCount = 0
            };

            return Ok(ApiResponse<AssignmentDto>.Ok(dto, "Assignment created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAssignmentRequest request)
        {
            var assignment = await _unitOfWork.Repository<Assignment>().GetByIdAsync(id);
            if (assignment == null)
                return NotFound(ApiResponse<AssignmentDto>.Fail("Assignment not found."));

            assignment.Title = request.Title;
            assignment.Description = request.Description;
            assignment.DueDate = request.DueDate;
            if (request.BatchId.HasValue)
            {
                assignment.BatchId = request.BatchId.Value;
                var batch = await _unitOfWork.Repository<Batch>().GetByIdAsync(request.BatchId.Value);
                if (batch != null) assignment.CourseId = batch.CourseId;
            }
            if (request.SubjectId.HasValue) assignment.SubjectId = request.SubjectId.Value;

            _unitOfWork.Repository<Assignment>().Update(assignment);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<bool>.Ok(true, "Assignment updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var assignment = await _unitOfWork.Repository<Assignment>().GetByIdAsync(id);
            if (assignment == null)
                return NotFound(ApiResponse<bool>.Fail("Assignment not found."));

            _unitOfWork.Repository<Assignment>().Remove(assignment);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<bool>.Ok(true, "Assignment deleted successfully."));
        }
    }
}

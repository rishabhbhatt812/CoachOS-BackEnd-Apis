using CoachOS.Api.Middlewares;
using CoachOS.Application.Features.Learning.Dtos;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Learning;
using CoachOS.Domain.Student;
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
    [Route("api/teacher/results")]
    [Authorize(Roles = "TEACHER,INSTITUTE_ADMIN,BRANCH_ADMIN,SUPER_ADMIN,GLOBAL_ADMIN")]
    [ModuleAccess("LEARNING")]
    public class TeacherResultsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public TeacherResultsController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        [HttpGet("{testId}")]
        public async Task<IActionResult> GetResults(Guid testId)
        {
            var test = await _unitOfWork.Repository<Test>().GetByIdAsync(testId);
            if (test == null)
                return NotFound(ApiResponse<object>.Fail("Test not found."));

            var studentBatches = (await _unitOfWork.Repository<StudentBatch>().GetAllAsync())
                .Where(sb => sb.BatchId == test.BatchId && sb.IsActive).ToList();
            var studentIds = studentBatches.Select(sb => sb.StudentId).ToList();

            var allStudents = await _unitOfWork.Repository<CoachOS.Domain.Student.Student>().GetAllAsync();
            var batchStudents = allStudents.Where(s => studentIds.Contains(s.Id)).ToList();

            var existingResults = (await _unitOfWork.Repository<TestResult>().GetAllAsync())
                .Where(r => r.TestId == testId).ToList();

            var studentResults = batchStudents.Select(s =>
            {
                var res = existingResults.FirstOrDefault(r => r.StudentId == s.Id);
                return new StudentTestResultDto
                {
                    StudentId = s.Id,
                    StudentCode = s.StudentCode,
                    StudentName = s.FullName,
                    MarksObtained = res?.MarksObtained ?? 0,
                    MaxMarks = test.MaxMarks,
                    Remarks = res?.Remark ?? string.Empty,
                    IsPresent = res != null || true
                };
            }).ToList();

            var response = new
            {
                TestId = test.Id,
                TestName = test.TestName,
                TestDate = test.TestDate.ToString("yyyy-MM-dd"),
                MaxMarks = test.MaxMarks,
                BatchId = test.BatchId,
                Results = studentResults
            };

            return Ok(ApiResponse<object>.Ok(response));
        }

        [HttpPost("save-draft")]
        public async Task<IActionResult> SaveDraft([FromBody] SaveDraftResultRequest request)
        {
            var test = await _unitOfWork.Repository<Test>().GetByIdAsync(request.TestId);
            if (test == null)
                return NotFound(ApiResponse<bool>.Fail("Test not found."));

            var existingResults = (await _unitOfWork.Repository<TestResult>().GetAllAsync())
                .Where(r => r.TestId == request.TestId).ToList();

            foreach (var item in request.Results)
            {
                var existing = existingResults.FirstOrDefault(r => r.StudentId == item.StudentId);
                if (existing != null)
                {
                    existing.MarksObtained = item.MarksObtained;
                    existing.Remark = item.Remarks;
                    _unitOfWork.Repository<TestResult>().Update(existing);
                }
                else
                {
                    var newResult = new TestResult
                    {
                        TestId = request.TestId,
                        StudentId = item.StudentId,
                        MarksObtained = item.MarksObtained,
                        Remark = item.Remarks
                    };
                    await _unitOfWork.Repository<TestResult>().AddAsync(newResult);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(ApiResponse<bool>.Ok(true, "Results saved as draft."));
        }

        [HttpPost("publish")]
        public async Task<IActionResult> PublishResult([FromBody] PublishResultRequest request)
        {
            var test = await _unitOfWork.Repository<Test>().GetByIdAsync(request.TestId);
            if (test == null)
                return NotFound(ApiResponse<bool>.Fail("Test not found."));

            var existingResults = (await _unitOfWork.Repository<TestResult>().GetAllAsync())
                .Where(r => r.TestId == request.TestId).ToList();

            foreach (var item in request.Results)
            {
                var existing = existingResults.FirstOrDefault(r => r.StudentId == item.StudentId);
                if (existing != null)
                {
                    existing.MarksObtained = item.MarksObtained;
                    existing.Remark = item.Remarks;
                    _unitOfWork.Repository<TestResult>().Update(existing);
                }
                else
                {
                    var newResult = new TestResult
                    {
                        TestId = request.TestId,
                        StudentId = item.StudentId,
                        MarksObtained = item.MarksObtained,
                        Remark = item.Remarks
                    };
                    await _unitOfWork.Repository<TestResult>().AddAsync(newResult);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(ApiResponse<bool>.Ok(true, "Results published successfully."));
        }
    }
}

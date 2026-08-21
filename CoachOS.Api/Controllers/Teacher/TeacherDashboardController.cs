using CoachOS.Api.Middlewares;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Academic;
using CoachOS.Domain.Identity;
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
    [Route("api/teacher/dashboard")]
    [Authorize(Roles = "TEACHER,INSTITUTE_ADMIN,BRANCH_ADMIN,SUPER_ADMIN,GLOBAL_ADMIN")]
    public class TeacherDashboardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public TeacherDashboardController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var userId = _currentUserService.UserId ?? Guid.Empty;

            var profiles = await _unitOfWork.Repository<TeacherProfile>().GetAllAsync();
            var profile = profiles.FirstOrDefault(tp => tp.UserId == userId);

            var batches = await _unitOfWork.Repository<Batch>().GetAllAsync();
            var teacherBatches = await _unitOfWork.Repository<TeacherBatch>().GetAllAsync();

            List<Batch> myBatches;
            if (profile != null)
            {
                var myBatchIds = teacherBatches
                    .Where(tb => tb.TeacherProfileId == profile.Id && tb.IsActive)
                    .Select(tb => tb.BatchId)
                    .Distinct()
                    .ToList();

                myBatches = batches.Where(b => myBatchIds.Contains(b.Id)).ToList();
                if (!myBatches.Any()) myBatches = batches.ToList();
            }
            else
            {
                myBatches = batches.ToList();
            }

            var batchIds = myBatches.Select(b => b.Id).ToList();

            var studentBatches = (await _unitOfWork.Repository<StudentBatch>().GetAllAsync())
                .Where(sb => batchIds.Contains(sb.BatchId) && sb.IsActive).ToList();
            var uniqueStudentCount = studentBatches.Select(sb => sb.StudentId).Distinct().Count();

            var notes = await _unitOfWork.Repository<Note>().GetAllAsync();
            var myNotes = notes.Where(n => batchIds.Contains(n.BatchId ?? Guid.Empty) || n.CreatedBy == userId).ToList();

            var tests = await _unitOfWork.Repository<Test>().GetAllAsync();
            var myTests = tests.Where(t => batchIds.Contains(t.BatchId)).ToList();

            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();

            var dashboardData = new
            {
                TotalBatches = myBatches.Count,
                TotalStudents = uniqueStudentCount,
                TotalNotes = myNotes.Count,
                TotalTests = myTests.Count,
                Batches = myBatches.Select(b => new
                {
                    b.Id,
                    b.Name,
                    CourseName = courses.FirstOrDefault(c => c.Id == b.CourseId)?.Name ?? "Unknown",
                    StudentCount = studentBatches.Count(sb => sb.BatchId == b.Id)
                }).Take(5).ToList(),
                RecentTests = myTests.OrderByDescending(t => t.TestDate).Select(t => new
                {
                    t.Id,
                    t.TestName,
                    TestDate = t.TestDate.ToString("yyyy-MM-dd"),
                    t.MaxMarks,
                    BatchName = myBatches.FirstOrDefault(b => b.Id == t.BatchId)?.Name ?? "Unknown"
                }).Take(5).ToList()
            };

            return Ok(ApiResponse<object>.Ok(dashboardData));
        }
    }
}

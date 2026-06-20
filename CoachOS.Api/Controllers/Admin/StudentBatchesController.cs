using CoachOS.Application.Interfaces.Services;
using CoachOS.Infrastructure.Data;
using CoachOS.Domain.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "BRANCH_ADMIN,INSTITUTE_ADMIN,GLOBAL_ADMIN,SUPER_ADMIN")]
    public class StudentBatchesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public StudentBatchesController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>Get all batch assignments for a student</summary>
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentBatches(Guid studentId)
        {
            var batches = await _db.StudentBatches
                .Where(sb => sb.StudentId == studentId)
                .Include(sb => sb.Batch)
                .ThenInclude(b => b!.Course)
                .Select(sb => new {
                    sb.Id,
                    sb.StudentId,
                    sb.BatchId,
                    BatchName = sb.Batch!.Name,
                    CourseName = sb.Batch.Course!.Name,
                    sb.JoinedDate,
                    sb.LeftDate,
                    sb.IsActive
                })
                .ToListAsync();

            return Ok(batches);
        }

        /// <summary>Assign student to a batch</summary>
        [HttpPost("assign")]
        public async Task<IActionResult> AssignBatch([FromBody] AssignBatchRequest request)
        {
            // Deactivate previous active batch assignments
            var previous = await _db.StudentBatches
                .Where(sb => sb.StudentId == request.StudentId && sb.IsActive)
                .ToListAsync();

            foreach (var prev in previous)
            {
                prev.IsActive = false;
                prev.LeftDate = DateOnly.FromDateTime(DateTime.UtcNow);
            }

            // Create new assignment
            var newAssignment = new StudentBatch
            {
                StudentId = request.StudentId,
                BatchId = request.BatchId,
                JoinedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                IsActive = true
            };

            _db.StudentBatches.Add(newAssignment);
            await _db.SaveChangesAsync();

            return Ok(new { success = true, message = "Student assigned to batch successfully." });
        }

        /// <summary>Transfer student to a different batch</summary>
        [HttpPost("transfer")]
        public async Task<IActionResult> TransferBatch([FromBody] AssignBatchRequest request)
        {
            return await AssignBatch(request); // Same logic — marks old inactive, creates new
        }

        public record AssignBatchRequest(Guid StudentId, Guid BatchId);
    }
}

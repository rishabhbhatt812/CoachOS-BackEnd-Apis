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
    public class ParentsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ParentsController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>Get all parents linked to a student</summary>
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentParents(Guid studentId)
        {
            var parents = await _db.StudentParents
                .Where(sp => sp.StudentId == studentId)
                .Include(sp => sp.Parent)
                .Select(sp => new {
                    sp.Id,
                    sp.RelationshipType,
                    sp.Parent!.FullName,
                    sp.Parent.Mobile,
                    sp.Parent.Email,
                    sp.Parent.Occupation
                })
                .ToListAsync();

            return Ok(parents);
        }

        /// <summary>Add parent and link to student</summary>
        [HttpPost]
        public async Task<IActionResult> AddParent([FromBody] CreateParentRequest request)
        {
            var parent = new Parent
            {
                FullName = request.FullName,
                Mobile = request.Mobile,
                Email = request.Email,
                Occupation = request.Occupation
            };
            _db.Parents.Add(parent);
            await _db.SaveChangesAsync();

            var link = new StudentParent
            {
                StudentId = request.StudentId,
                ParentId = parent.Id,
                RelationshipType = request.RelationshipType
            };
            _db.StudentParents.Add(link);
            await _db.SaveChangesAsync();

            return Ok(new { success = true, parentId = parent.Id });
        }

        public record CreateParentRequest(
            Guid StudentId,
            string FullName,
            string Mobile,
            string? Email,
            string? Occupation,
            string RelationshipType = "Parent");
    }
}

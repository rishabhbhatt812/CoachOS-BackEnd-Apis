using CoachOS.Api.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Teacher
{
    [ApiController]
    [Route("api/teacher/batches")]
    [Authorize(Roles = "TEACHER,INSTITUTE_ADMIN,BRANCH_ADMIN,SUPER_ADMIN,GLOBAL_ADMIN")]
    [ModuleAccess("LEARNING")]
    public class TeacherBatchesController : ControllerBase
    {
        private readonly CoachOS.Application.Interfaces.Services.ICurrentUserService _currentUserService;
        private readonly CoachOS.Application.Interfaces.Repositories.IUnitOfWork _unitOfWork;

        public TeacherBatchesController(
            CoachOS.Application.Interfaces.Services.ICurrentUserService currentUserService,
            CoachOS.Application.Interfaces.Repositories.IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyBatches()
        {
            var userId = _currentUserService.UserId ?? Guid.Empty;
            var profiles = await _unitOfWork.Repository<CoachOS.Domain.Identity.TeacherProfile>().GetAllAsync();
            var profile = profiles.FirstOrDefault(tp => tp.UserId == userId);

            var batches = await _unitOfWork.Repository<CoachOS.Domain.Academic.Batch>().GetAllAsync();
            if (profile == null)
            {
                var fallbackResult = batches.Select(b => new { id = b.Id, name = b.Name }).ToList();
                return Ok(CoachOS.Shared.Responses.ApiResponse<object>.Ok(fallbackResult));
            }

            var teacherBatches = await _unitOfWork.Repository<CoachOS.Domain.Identity.TeacherBatch>().GetAllAsync();
            var myBatchIds = teacherBatches
                .Where(tb => tb.TeacherProfileId == profile.Id && tb.IsActive)
                .Select(tb => tb.BatchId)
                .Distinct()
                .ToList();

            var myBatches = batches.Where(b => myBatchIds.Contains(b.Id)).ToList();
            if (!myBatches.Any())
            {
                myBatches = batches.ToList();
            }

            var result = myBatches.Select(b => new { id = b.Id, name = b.Name }).ToList();
            return Ok(CoachOS.Shared.Responses.ApiResponse<object>.Ok(result));
        }

        [HttpGet("{batchId}/students")]
        public async Task<IActionResult> GetBatchStudents(Guid batchId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{batchId}/performance")]
        public async Task<IActionResult> GetBatchPerformance(Guid batchId)
        {
            throw new NotImplementedException();
        }
    }
}

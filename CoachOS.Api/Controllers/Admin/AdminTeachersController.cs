using CoachOS.Domain.Identity;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoachOS.Shared.Responses;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CoachOS.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/teachers")]
    [Authorize(Roles = "INSTITUTE_ADMIN,GLOBAL_ADMIN,SUPER_ADMIN")]
    public class AdminTeachersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeacherRegistrationService _teacherRegistrationService;

        public AdminTeachersController(IUnitOfWork unitOfWork, ITeacherRegistrationService teacherRegistrationService)
        {
            _unitOfWork = unitOfWork;
            _teacherRegistrationService = teacherRegistrationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeachers()
        {
            // Find teacher role
            var role = (await _unitOfWork.Repository<Role>().GetAllAsync()).FirstOrDefault(r => r.Code == "TEACHER");
            if (role == null) return Ok(ApiResponse<List<object>>.Ok(new List<object>()));

            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            var teachers = users.Where(u => u.RoleId == role.Id).ToList();

            var teacherSubjects = await _unitOfWork.Repository<TeacherSubject>().GetAllAsync();
            var subjects = await _unitOfWork.Repository<CoachOS.Domain.Academic.Subject>().GetAllAsync();
            var institutes = await _unitOfWork.Repository<CoachOS.Domain.Tenancy.Institute>().GetAllAsync();
            var instituteMap = institutes.ToDictionary(i => i.Id);

            var result = teachers.Select(t => new
            {
                t.Id,
                t.InstituteId,
                InstituteName = instituteMap.TryGetValue(t.InstituteId, out var inst) ? inst.Name : "Apex Coaching Academy",
                InstituteCode = instituteMap.TryGetValue(t.InstituteId, out var instCode) ? instCode.InstituteCode : "INST001",
                t.FullName,
                t.Email,
                Mobile = t.MobileNumber,
                t.IsActive,
                Subjects = teacherSubjects.Where(ts => ts.UserId == t.Id).Select(ts => new
                {
                    ts.SubjectId,
                    SubjectName = subjects.FirstOrDefault(s => s.Id == ts.SubjectId)?.Name
                }).ToList()
            });

            return Ok(ApiResponse<object>.Ok(result, "Teachers retrieved successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> RegisterTeacher([FromBody] RegisterTeacherDto request)
        {
            var result = await _teacherRegistrationService.RegisterTeacherAsync(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}

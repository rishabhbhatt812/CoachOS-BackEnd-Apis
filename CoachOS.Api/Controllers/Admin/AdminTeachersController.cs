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
    [Authorize(Roles = "INSTITUTE_ADMIN,GLOBAL_ADMIN,SUPER_ADMIN,ADMIN,BRANCH_ADMIN")]
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

            var users = (await _unitOfWork.Repository<User>().GetAllAsync())
                .Where(u => u.RoleId == role.Id && !u.IsDeleted).ToList();

            var staffProfiles = await _unitOfWork.Repository<StaffProfile>().GetAllAsync();
            var staffProfileMap = staffProfiles.Where(sp => !sp.IsDeleted).GroupBy(sp => sp.UserId).ToDictionary(g => g.Key, g => g.FirstOrDefault());

            var teacherProfiles = await _unitOfWork.Repository<TeacherProfile>().GetAllAsync();
            var teacherProfileMap = teacherProfiles.Where(tp => !tp.IsDeleted).GroupBy(tp => tp.UserId).ToDictionary(g => g.Key, g => g.FirstOrDefault());

            var teacherSubjects = await _unitOfWork.Repository<TeacherSubject>().GetAllAsync();
            var subjects = await _unitOfWork.Repository<CoachOS.Domain.Academic.Subject>().GetAllAsync();
            var institutes = await _unitOfWork.Repository<CoachOS.Domain.Tenancy.Institute>().GetAllAsync();
            var instituteMap = institutes.ToDictionary(i => i.Id);

            var result = users.Select(t =>
            {
                staffProfileMap.TryGetValue(t.Id, out var sp);
                teacherProfileMap.TryGetValue(t.Id, out var tp);

                return new
                {
                    t.Id,
                    TeacherProfileId = tp?.Id,
                    t.InstituteId,
                    InstituteName = instituteMap.TryGetValue(t.InstituteId, out var inst) ? inst.Name : "Apex Coaching Academy",
                    InstituteCode = instituteMap.TryGetValue(t.InstituteId, out var instCode) ? instCode.InstituteCode : "INST001",
                    StaffCode = sp?.StaffCode ?? "",
                    t.FullName,
                    t.Email,
                    Mobile = t.MobileNumber ?? "",
                    t.IsActive,
                    Status = t.IsActive ? "Active" : "Inactive",
                    Department = sp?.Department ?? "",
                    Designation = sp?.Designation ?? "",
                    TeacherType = sp?.StaffType ?? tp?.TeacherType ?? "Full Time",
                    ExperienceYears = sp?.ExperienceYears ?? tp?.TeachingExperienceYears ?? 0,
                    JoiningDate = sp?.JoiningDate,
                    Address = sp?.Address ?? "",
                    ProfilePhoto = sp?.ProfilePhoto ?? "",
                    Bio = tp?.Bio ?? "",
                    Subjects = teacherSubjects.Where(ts => ts.UserId == t.Id && !ts.IsDeleted).Select(ts => new
                    {
                        ts.SubjectId,
                        SubjectName = subjects.FirstOrDefault(s => s.Id == ts.SubjectId)?.Name
                    }).ToList()
                };
            });

            return Ok(ApiResponse<object>.Ok(result, "Teachers retrieved successfully."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacher(Guid id)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
            TeacherProfile? teacherProfile = null;

            if (user == null)
            {
                teacherProfile = await _unitOfWork.Repository<TeacherProfile>().GetByIdAsync(id);
                if (teacherProfile != null)
                {
                    user = await _unitOfWork.Repository<User>().GetByIdAsync(teacherProfile.UserId);
                }
            }

            if (user == null || user.IsDeleted)
                return NotFound(ApiResponse<object>.Fail("Teacher not found."));

            if (teacherProfile == null)
            {
                var teacherProfiles = await _unitOfWork.Repository<TeacherProfile>().GetAllAsync();
                teacherProfile = teacherProfiles.FirstOrDefault(tp => tp.UserId == user.Id && !tp.IsDeleted);
            }

            var staffProfiles = await _unitOfWork.Repository<StaffProfile>().GetAllAsync();
            var staffProfile = staffProfiles.FirstOrDefault(sp => sp.UserId == user.Id && !sp.IsDeleted);

            var teacherSubjects = (await _unitOfWork.Repository<TeacherSubject>().GetAllAsync())
                .Where(ts => ts.UserId == user.Id && !ts.IsDeleted).ToList();

            var subjects = await _unitOfWork.Repository<CoachOS.Domain.Academic.Subject>().GetAllAsync();

            List<TeacherQualification> qualifications = new();
            List<TeacherDocument> documents = new();

            if (teacherProfile != null)
            {
                qualifications = (await _unitOfWork.Repository<TeacherQualification>().GetAllAsync())
                    .Where(q => q.TeacherProfileId == teacherProfile.Id && !q.IsDeleted).ToList();

                documents = (await _unitOfWork.Repository<TeacherDocument>().GetAllAsync())
                    .Where(d => d.TeacherProfileId == teacherProfile.Id && !d.IsDeleted).ToList();
            }

            var institute = await _unitOfWork.Repository<CoachOS.Domain.Tenancy.Institute>().GetByIdAsync(user.InstituteId);

            var result = new
            {
                user.Id,
                TeacherProfileId = teacherProfile?.Id,
                user.InstituteId,
                InstituteName = institute?.Name ?? "Apex Coaching Academy",
                InstituteCode = institute?.InstituteCode ?? "INST001",
                StaffCode = staffProfile?.StaffCode ?? "",
                user.FullName,
                user.Email,
                Mobile = user.MobileNumber ?? "",
                user.IsActive,
                Status = user.IsActive ? "Active" : "Inactive",
                Department = staffProfile?.Department ?? "",
                Designation = staffProfile?.Designation ?? "",
                TeacherType = staffProfile?.StaffType ?? teacherProfile?.TeacherType ?? "Full Time",
                ExperienceYears = staffProfile?.ExperienceYears ?? teacherProfile?.TeachingExperienceYears ?? 0,
                JoiningDate = staffProfile?.JoiningDate,
                Address = staffProfile?.Address ?? "",
                ProfilePhoto = staffProfile?.ProfilePhoto ?? "",
                Bio = teacherProfile?.Bio ?? "",
                CreatedAt = user.CreatedAt,
                Subjects = teacherSubjects.Select(ts => new
                {
                    ts.SubjectId,
                    SubjectName = subjects.FirstOrDefault(s => s.Id == ts.SubjectId)?.Name
                }).ToList(),
                Qualifications = qualifications.Select(q => new
                {
                    q.Id,
                    q.Qualification,
                    q.Specialization,
                    q.University,
                    q.PassingYear,
                    q.PercentageOrCGPA,
                    q.CertificateFilePath
                }).ToList(),
                Documents = documents.Select(d => new
                {
                    d.Id,
                    d.DocumentType,
                    d.FileName,
                    d.FilePath,
                    d.UploadedOn
                }).ToList()
            };

            return Ok(ApiResponse<object>.Ok(result, "Teacher retrieved successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(Guid id)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
            TeacherProfile? teacherProfile = null;

            if (user == null)
            {
                teacherProfile = await _unitOfWork.Repository<TeacherProfile>().GetByIdAsync(id);
                if (teacherProfile != null)
                {
                    user = await _unitOfWork.Repository<User>().GetByIdAsync(teacherProfile.UserId);
                }
            }

            if (user == null)
                return NotFound(ApiResponse<bool>.Fail("Teacher not found."));

            // Soft delete user
            user.IsDeleted = true;
            user.IsActive = false;
            user.DeletedAt = DateTime.UtcNow;
            _unitOfWork.Repository<User>().Update(user);

            // Soft delete StaffProfile
            var staffProfiles = await _unitOfWork.Repository<StaffProfile>().GetAllAsync();
            var staffProfile = staffProfiles.FirstOrDefault(sp => sp.UserId == user.Id);
            if (staffProfile != null)
            {
                staffProfile.IsDeleted = true;
                staffProfile.IsActive = false;
                staffProfile.DeletedAt = DateTime.UtcNow;
                _unitOfWork.Repository<StaffProfile>().Update(staffProfile);
            }

            // Soft delete TeacherProfile
            if (teacherProfile == null)
            {
                var teacherProfiles = await _unitOfWork.Repository<TeacherProfile>().GetAllAsync();
                teacherProfile = teacherProfiles.FirstOrDefault(tp => tp.UserId == user.Id);
            }
            if (teacherProfile != null)
            {
                teacherProfile.IsDeleted = true;
                teacherProfile.IsActive = false;
                teacherProfile.DeletedAt = DateTime.UtcNow;
                _unitOfWork.Repository<TeacherProfile>().Update(teacherProfile);
            }

            // Soft delete TeacherSubject
            var teacherSubjects = (await _unitOfWork.Repository<TeacherSubject>().GetAllAsync())
                .Where(ts => ts.UserId == user.Id).ToList();
            foreach (var ts in teacherSubjects)
            {
                ts.IsDeleted = true;
                ts.DeletedAt = DateTime.UtcNow;
                _unitOfWork.Repository<TeacherSubject>().Update(ts);
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(ApiResponse<bool>.Ok(true, "Teacher deleted successfully."));
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

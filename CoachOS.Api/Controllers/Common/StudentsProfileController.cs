using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Academic;
using CoachOS.Domain.Finance;
using CoachOS.Domain.Identity;
using CoachOS.Domain.Student;
using CoachOS.Domain.Tenancy;
using CoachOS.Domain.Learning;
using CoachOS.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Common
{
    [ApiController]
    [Route("api/students")]
    public class StudentsProfileController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public StudentsProfileController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        [HttpGet("my-profile")]
        [Authorize(Roles = "STUDENT")]
        public async Task<IActionResult> GetMyProfile()
        {
            var studentId = _currentUserService.UserId;
            if (studentId == null || studentId == Guid.Empty)
                return Unauthorized(ApiResponse<object>.Fail("Student context not found."));

            var profile = await GetStudentProfileDataAsync(studentId.Value);
            if (profile == null)
                return NotFound(ApiResponse<object>.Fail("Student profile not found."));

            return Ok(ApiResponse<object>.Ok(profile));
        }

        [HttpGet("{studentId:guid}/profile")]
        [Authorize(Roles = "SUPER_ADMIN,GLOBAL_ADMIN,INSTITUTE_ADMIN,BRANCH_ADMIN,RECEPTIONIST")]
        public async Task<IActionResult> GetStudentProfile(Guid studentId)
        {
            var currentUserRole = _currentUserService.RoleCode;
            var currentUserId = _currentUserService.UserId ?? Guid.Empty;

            var userObj = await _unitOfWork.Repository<User>().FirstOrDefaultAsync(u => u.Id == currentUserId, ignoreQueryFilters: true);
            var currentUserBranchId = userObj?.BranchId;

            var studentUser = await _unitOfWork.Repository<User>().FirstOrDefaultAsync(u => u.Id == studentId, ignoreQueryFilters: true);
            if (studentUser == null)
                return NotFound(ApiResponse<object>.Fail("Student user record not found."));

            if ((currentUserRole == "BRANCH_ADMIN" || currentUserRole == "RECEPTIONIST") && studentUser.BranchId != currentUserBranchId)
            {
                return Forbid();
            }

            var profile = await GetStudentProfileDataAsync(studentId);
            if (profile == null)
                return NotFound(ApiResponse<object>.Fail("Student profile not found."));

            return Ok(ApiResponse<object>.Ok(profile));
        }

        private async Task<object?> GetStudentProfileDataAsync(Guid studentId)
        {
            var student = await _unitOfWork.Repository<CoachOS.Domain.Student.Student>().FirstOrDefaultAsync(s => s.Id == studentId, ignoreQueryFilters: true);
            if (student == null) return null;

            var user = await _unitOfWork.Repository<User>().FirstOrDefaultAsync(u => u.Id == studentId, ignoreQueryFilters: true);
            if (user == null) return null;

            string? parentName = null;
            string? parentMobileNumber = null;
            var studentParent = await _unitOfWork.Repository<StudentParent>().FirstOrDefaultAsync(sp => sp.StudentId == studentId, ignoreQueryFilters: true);
            if (studentParent != null)
            {
                var parent = await _unitOfWork.Repository<Parent>().FirstOrDefaultAsync(p => p.Id == studentParent.ParentId, ignoreQueryFilters: true);
                if (parent != null)
                {
                    parentName = parent.FullName;
                    parentMobileNumber = parent.Mobile;
                }
            }

            string courseName = "Unknown Course";
            string batchName = "Unknown Batch";
            var studentBatch = await _unitOfWork.Repository<StudentBatch>().FirstOrDefaultAsync(sb => sb.StudentId == studentId && sb.IsActive, ignoreQueryFilters: true);
            if (studentBatch != null)
            {
                var batch = await _unitOfWork.Repository<Batch>().FirstOrDefaultAsync(b => b.Id == studentBatch.BatchId, ignoreQueryFilters: true);
                if (batch != null)
                {
                    batchName = batch.Name;
                    var course = await _unitOfWork.Repository<Course>().FirstOrDefaultAsync(c => c.Id == batch.CourseId, ignoreQueryFilters: true);
                    courseName = course?.Name ?? "Unknown Course";
                }
            }

            string instituteName = "Unknown Institute";
            string? instituteLogoUrl = null;
            if (user.InstituteId != Guid.Empty)
            {
                var institute = await _unitOfWork.Repository<Institute>().FirstOrDefaultAsync(i => i.Id == user.InstituteId, ignoreQueryFilters: true);
                if (institute != null)
                {
                    instituteName = institute.Name;
                    instituteLogoUrl = GetAbsoluteUrl(institute.LogoPath);
                }
            }

            string branchName = "Main Branch";
            if (user.BranchId.HasValue)
            {
                var branch = await _unitOfWork.Repository<Branch>().FirstOrDefaultAsync(b => b.Id == user.BranchId.Value, ignoreQueryFilters: true);
                branchName = branch?.Name ?? "Main Branch";
            }

            string feeStatus = "Paid";
            var feePlan = await _unitOfWork.Repository<FeePlan>().FirstOrDefaultAsync(fp => fp.StudentId == studentId, ignoreQueryFilters: true);
            if (feePlan != null)
            {
                var installments = await _unitOfWork.Repository<Installment>().GetAllAsync();
                var studentInstallments = installments.Where(i => i.FeePlanId == feePlan.Id).ToList();
                if (studentInstallments.Any(i => i.Status != "Paid"))
                {
                    feeStatus = "Pending";
                }
            }

            var records = await _unitOfWork.Repository<AttendanceRecord>().GetAllAsync();
            var studentRecords = records.Where(r => r.StudentId == studentId).ToList();
            var totalSessions = studentRecords.Count;
            var presentSessions = studentRecords.Count(r => r.Status == "Present");
            var attendancePercentage = totalSessions > 0 ? (int)Math.Round((double)presentSessions / totalSessions * 100) : 100;

            return new
            {
                studentId = student.Id,
                userId = user.Id,
                studentCode = student.StudentCode,
                fullName = student.FullName,
                email = student.Email ?? user.Email,
                mobileNumber = student.Mobile ?? user.MobileNumber,
                parentName = parentName,
                parentMobileNumber = parentMobileNumber,
                dateOfBirth = student.DateOfBirth?.ToString("yyyy-MM-dd"),
                gender = student.Gender,
                address = student.Address,
                profilePhotoUrl = GetAbsoluteUrl(student.ProfileImagePath),
                instituteName = instituteName,
                instituteLogoUrl = instituteLogoUrl,
                branchName = branchName,
                courseName = courseName,
                batchName = batchName,
                admissionDate = student.AdmissionDate.ToString("yyyy-MM-dd"),
                feeStatus = feeStatus,
                attendancePercentage = attendancePercentage
            };
        }

        private string GetAbsoluteUrl(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return "";
            if (relativePath.StartsWith("http://") || relativePath.StartsWith("https://"))
                return relativePath;

            var request = HttpContext.Request;
            var baseUri = $"{request.Scheme}://{request.Host}{request.PathBase}";
            return $"{baseUri}/{relativePath.TrimStart('/')}";
        }
    }
}

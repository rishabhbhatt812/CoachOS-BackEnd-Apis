using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Identity;
using CoachOS.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Application.Features.Teachers
{
    public class TeacherRegistrationService : ITeacherRegistrationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public TeacherRegistrationService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<Guid>> RegisterTeacherAsync(RegisterTeacherDto request)
        {
            var instituteId = _currentUserService.InstituteId ?? Guid.Empty;
            var role = (await _unitOfWork.Repository<Role>().GetAllAsync()).FirstOrDefault(r => r.Code == "TEACHER");
            if (role == null) return ApiResponse<Guid>.Fail("Teacher role not found in system.");

            // 1. Create User
            var user = new User
            {
                Id = Guid.NewGuid(),
                InstituteId = instituteId,
                FullName = request.FullName,
                Email = request.Email,
                MobileNumber = request.Mobile,
                Username = request.Email,
                RoleId = role.Id,
                IsActive = true
            };
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, request.Password);
            await _unitOfWork.Repository<User>().AddAsync(user);

            // 2. Create StaffProfile
            var staffProfile = new StaffProfile
            {
                Id = Guid.NewGuid(),
                InstituteId = instituteId,
                UserId = user.Id,
                StaffCode = $"STF-{DateTime.UtcNow.Ticks.ToString().Substring(10)}",
                StaffType = request.TeacherType,
                JoiningDate = request.JoiningDate.HasValue ? request.JoiningDate.Value.ToDateTime(TimeOnly.MinValue) : DateTime.UtcNow.Date,
                Designation = request.Designation,
                Department = request.Department,
                Qualification = request.Qualifications.FirstOrDefault()?.Qualification ?? string.Empty,
                ExperienceYears = request.ExperienceYears,
                Address = request.CurrentAddress,
                ProfilePhoto = request.ProfilePhotoPath,
                IsActive = true
            };
            await _unitOfWork.Repository<StaffProfile>().AddAsync(staffProfile);

            // 3. Create TeacherProfile
            var teacherProfile = new TeacherProfile
            {
                Id = Guid.NewGuid(),
                InstituteId = instituteId,
                UserId = user.Id,
                SubjectExpertise = request.SubjectIds.Any() ? "Mapped Subjects" : string.Empty,
                TeacherType = request.TeacherType,
                TeachingExperienceYears = request.ExperienceYears,
                Bio = string.Empty,
                IsActive = true
            };
            await _unitOfWork.Repository<TeacherProfile>().AddAsync(teacherProfile);

            // 4. Create TeacherQualifications
            if (request.Qualifications != null && request.Qualifications.Any())
            {
                foreach (var q in request.Qualifications)
                {
                    await _unitOfWork.Repository<TeacherQualification>().AddAsync(new TeacherQualification
                    {
                        Id = Guid.NewGuid(),
                        InstituteId = instituteId,
                        TeacherProfileId = teacherProfile.Id,
                        Qualification = q.Qualification,
                        Specialization = q.Specialization,
                        University = q.University,
                        PassingYear = q.PassingYear,
                        PercentageOrCGPA = q.PercentageOrCGPA,
                        CertificateFilePath = q.CertificateFilePath
                    });
                }
            }

            // 5. Create TeacherSubjects
            if (request.SubjectIds != null && request.SubjectIds.Any())
            {
                foreach (var subjectId in request.SubjectIds)
                {
                    await _unitOfWork.Repository<TeacherSubject>().AddAsync(new TeacherSubject
                    {
                        Id = Guid.NewGuid(),
                        InstituteId = instituteId,
                        UserId = user.Id,
                        SubjectId = subjectId
                    });
                }
            }

            // 6. Create TeacherDocuments
            if (request.Documents != null && request.Documents.Any())
            {
                foreach (var doc in request.Documents)
                {
                    await _unitOfWork.Repository<TeacherDocument>().AddAsync(new TeacherDocument
                    {
                        Id = Guid.NewGuid(),
                        InstituteId = instituteId,
                        TeacherProfileId = teacherProfile.Id,
                        DocumentType = doc.DocumentType,
                        FileName = doc.FileName,
                        FilePath = doc.FilePath,
                        UploadedOn = DateTime.UtcNow
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<Guid>.Ok(teacherProfile.Id, "Teacher registered successfully.");
        }

        public async Task<ApiResponse<TeacherProfileDto>> GetTeacherProfileAsync(Guid teacherProfileId)
        {
            var profile = await _unitOfWork.Repository<TeacherProfile>().GetByIdAsync(teacherProfileId);
            if (profile == null) return ApiResponse<TeacherProfileDto>.Fail("Profile not found");

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(profile.UserId);
            var staffProfile = (await _unitOfWork.Repository<StaffProfile>().GetAllAsync()).FirstOrDefault(sp => sp.UserId == profile.UserId);

            var dto = new TeacherProfileDto
            {
                Id = profile.Id,
                TeacherCode = staffProfile?.StaffCode ?? "",
                FullName = user?.FullName ?? "",
                Email = user?.Email ?? "",
                Mobile = user?.MobileNumber ?? "",
                Gender = "Not Specified",
                DateOfBirth = DateOnly.FromDateTime(DateTime.MinValue)
            };

            return ApiResponse<TeacherProfileDto>.Ok(dto);
        }
    }
}

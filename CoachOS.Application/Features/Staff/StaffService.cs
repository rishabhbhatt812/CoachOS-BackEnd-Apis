using CoachOS.Application.Features.Staff.Dtos;
using CoachOS.Application.Helpers;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Identity;
using CoachOS.Domain.Tenancy;
using CoachOS.Shared.Constants;
using CoachOS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Application.Features.Staff
{
    public class StaffService : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public StaffService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        private async Task<(bool Authorized, string Error, Guid? BranchId)> ValidateSecurityAndGetBranchAsync(string action, Guid? targetBranchId = null, Guid? targetRoleId = null)
        {
            var callerUserId = _currentUserService.UserId;
            var callerInstituteId = _currentUserService.InstituteId;
            var callerRoleCode = _currentUserService.RoleCode;

            if (callerUserId == null || callerInstituteId == null)
            {
                return (false, "Unauthorized.", null);
            }

            if (callerRoleCode == RoleCodes.Student)
            {
                return (false, "Access denied. Students cannot access staff APIs.", null);
            }

            if (callerRoleCode == RoleCodes.Teacher)
            {
                return (false, "Access denied. Teachers cannot manage staff.", null);
            }

            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            var callerUser = users.FirstOrDefault(u => u.Id == callerUserId.Value);
            var callerBranchId = callerUser?.BranchId;

            // Branch Admin security validation
            if (callerRoleCode == RoleCodes.BranchAdmin)
            {
                if (callerBranchId == null)
                {
                    return (false, "Access denied. Branch Admin is not assigned to a branch.", null);
                }

                if (targetBranchId.HasValue && targetBranchId.Value != callerBranchId.Value)
                {
                    return (false, "Access denied. Branch Admin can only manage staff within their assigned branch.", callerBranchId);
                }

                // Branch Admin cannot create/manage admin roles
                if (targetRoleId.HasValue)
                {
                    var targetRole = await _unitOfWork.Repository<Role>().GetByIdAsync(targetRoleId.Value);
                    if (targetRole != null && (targetRole.Code == RoleCodes.SuperAdmin || targetRole.Code == "GLOBAL_ADMIN" || targetRole.Code == RoleCodes.InstituteAdmin || targetRole.Code == RoleCodes.BranchAdmin))
                    {
                        return (false, "Access denied. Branch Admin cannot assign or manage admin roles.", callerBranchId);
                    }
                }
            }

            // Receptionist security validation
            if (callerRoleCode == RoleCodes.Receptionist)
            {
                // Receptionists cannot create or manage admin roles
                if (targetRoleId.HasValue)
                {
                    var targetRole = await _unitOfWork.Repository<Role>().GetByIdAsync(targetRoleId.Value);
                    if (targetRole != null && (targetRole.Code == RoleCodes.SuperAdmin || targetRole.Code == "GLOBAL_ADMIN" || targetRole.Code == RoleCodes.InstituteAdmin || targetRole.Code == RoleCodes.BranchAdmin))
                    {
                        return (false, "Access denied. Receptionist cannot create or manage admin users.", callerBranchId);
                    }
                }
            }

            return (true, string.Empty, callerBranchId);
        }

        public async Task<ApiResponse<Guid>> CreateStaffAsync(CreateStaffRequest request)
        {
            var security = await ValidateSecurityAndGetBranchAsync("create", request.BranchId, request.RoleId);
            if (!security.Authorized)
            {
                return ApiResponse<Guid>.Fail(security.Error);
            }

            var instituteId = _currentUserService.InstituteId!.Value;

            // 1. Validation
            var emailExists = !string.IsNullOrEmpty(request.Email) && await _unitOfWork.Repository<User>()
                .AnyAsync(u => u.Email.ToLower() == request.Email.ToLower() && u.InstituteId == instituteId);
            
            if (emailExists)
            {
                return ApiResponse<Guid>.Fail("Email must be unique inside the Institute.");
            }

            var mobileExists = !string.IsNullOrEmpty(request.MobileNumber) && await _unitOfWork.Repository<User>()
                .AnyAsync(u => u.MobileNumber == request.MobileNumber && u.InstituteId == instituteId);

            if (mobileExists)
            {
                return ApiResponse<Guid>.Fail("Mobile Number must be unique inside the Institute.");
            }

            var role = await _unitOfWork.Repository<Role>().GetByIdAsync(request.RoleId);
            if (role == null)
            {
                return ApiResponse<Guid>.Fail("Selected role not found.");
            }

            var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(request.BranchId);
            if (branch == null)
            {
                return ApiResponse<Guid>.Fail("Selected branch not found.");
            }

            // If role is Teacher, validate that subject expertise / teacher type is present
            if (role.Code == RoleCodes.Teacher)
            {
                if (string.IsNullOrEmpty(request.SubjectExpertise))
                {
                    return ApiResponse<Guid>.Fail("Subject expertise is required for Teacher role.");
                }
                if (string.IsNullOrEmpty(request.TeacherType))
                {
                    return ApiResponse<Guid>.Fail("Teacher type is required for Teacher role.");
                }
            }

            // 2. Username generation
            var username = !string.IsNullOrEmpty(request.Email) ? request.Email : request.MobileNumber;
            if (string.IsNullOrEmpty(username))
            {
                return ApiResponse<Guid>.Fail("Email or Mobile Number is required to generate username.");
            }

            // 3. Password generation
            var tempPassword = "Temp@" + Guid.NewGuid().ToString("N").Substring(0, 8);
            var (pwdHash, pwdSalt) = PasswordHelper.CreateHash(tempPassword);

            // 4. Create User record
            var user = new User
            {
                Id = Guid.NewGuid(),
                InstituteId = instituteId,
                BranchId = request.BranchId,
                FullName = request.FullName,
                Email = request.Email ?? string.Empty,
                MobileNumber = request.MobileNumber,
                Username = username,
                PasswordHash = pwdHash,
                PasswordSalt = pwdSalt,
                RoleId = request.RoleId,
                IsPasswordChanged = false,
                IsActive = true
            };

            await _unitOfWork.Repository<User>().AddAsync(user);

            // 5. Create StaffProfile
            var staffCode = request.StaffCode;
            if (string.IsNullOrEmpty(staffCode))
            {
                staffCode = $"STF-{DateTime.UtcNow.Ticks.ToString().Substring(10)}";
            }

            var staffProfile = new StaffProfile
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                InstituteId = instituteId,
                StaffCode = staffCode,
                StaffType = request.StaffType,
                JoiningDate = request.JoiningDate,
                Designation = request.Designation,
                Department = request.Department,
                Qualification = request.Qualification,
                ExperienceYears = request.ExperienceYears,
                Address = request.Address,
                ProfilePhoto = request.ProfilePhoto,
                EmergencyContactName = request.EmergencyContactName,
                EmergencyContactNumber = request.EmergencyContactNumber,
                IsActive = true
            };

            await _unitOfWork.Repository<StaffProfile>().AddAsync(staffProfile);

            // 6. Create TeacherProfile (if Teacher)
            if (role.Code == RoleCodes.Teacher)
            {
                var teacherProfile = new TeacherProfile
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    InstituteId = instituteId,
                    SubjectExpertise = request.SubjectExpertise ?? string.Empty,
                    TeacherType = request.TeacherType ?? string.Empty,
                    TeachingExperienceYears = request.TeachingExperienceYears,
                    Bio = request.Bio,
                    IsActive = true
                };

                await _unitOfWork.Repository<TeacherProfile>().AddAsync(teacherProfile);
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<Guid>.Ok(user.Id, $"Staff created successfully. Temporary Password: {tempPassword}");
        }

        public async Task<ApiResponse<List<StaffResponse>>> GetAllStaffAsync()
        {
            var security = await ValidateSecurityAndGetBranchAsync("read");
            if (!security.Authorized)
            {
                return ApiResponse<List<StaffResponse>>.Fail(security.Error);
            }

            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            var staffProfiles = await _unitOfWork.Repository<StaffProfile>().GetAllAsync();
            var teacherProfiles = await _unitOfWork.Repository<TeacherProfile>().GetAllAsync();
            var roles = await _unitOfWork.Repository<Role>().GetAllAsync();
            var branches = await _unitOfWork.Repository<Branch>().GetAllAsync();

            // Filter branch if branch admin
            var filteredUsers = users.AsEnumerable();
            if (_currentUserService.RoleCode == RoleCodes.BranchAdmin)
            {
                filteredUsers = filteredUsers.Where(u => u.BranchId == security.BranchId);
            }

            // Build responses
            var result = new List<StaffResponse>();
            foreach (var user in filteredUsers)
            {
                // Skip students
                var role = roles.FirstOrDefault(r => r.Id == user.RoleId);
                if (role == null || role.Code == RoleCodes.Student) continue;

                var staffProfile = staffProfiles.FirstOrDefault(sp => sp.UserId == user.Id);
                var teacherProfile = teacherProfiles.FirstOrDefault(tp => tp.UserId == user.Id);
                var branch = branches.FirstOrDefault(b => b.Id == user.BranchId);

                var staffRes = new StaffResponse
                {
                    Id = user.Id,
                    InstituteId = user.InstituteId,
                    BranchId = user.BranchId,
                    BranchName = branch?.Name,
                    FullName = user.FullName,
                    Email = user.Email,
                    MobileNumber = user.MobileNumber,
                    Username = user.Username,
                    RoleId = user.RoleId,
                    RoleName = role.RoleName,
                    RoleCode = role.Code,
                    IsPasswordChanged = user.IsPasswordChanged,
                    IsActive = user.IsActive,
                    LastLoginOn = user.LastLoginOn,
                    CreatedBy = user.CreatedBy,
                    CreatedOn = user.CreatedAt,
                    UpdatedBy = user.UpdatedBy,
                    UpdatedOn = user.UpdatedAt
                };

                if (staffProfile != null)
                {
                    staffRes.StaffProfile = new StaffProfileResponse
                    {
                        Id = staffProfile.Id,
                        StaffCode = staffProfile.StaffCode,
                        StaffType = staffProfile.StaffType,
                        JoiningDate = staffProfile.JoiningDate,
                        Designation = staffProfile.Designation,
                        Department = staffProfile.Department,
                        Qualification = staffProfile.Qualification,
                        ExperienceYears = staffProfile.ExperienceYears,
                        Address = staffProfile.Address,
                        ProfilePhoto = staffProfile.ProfilePhoto,
                        EmergencyContactName = staffProfile.EmergencyContactName,
                        EmergencyContactNumber = staffProfile.EmergencyContactNumber,
                        IsActive = staffProfile.IsActive
                    };
                }

                if (teacherProfile != null)
                {
                    staffRes.TeacherProfile = new TeacherProfileResponse
                    {
                        Id = teacherProfile.Id,
                        SubjectExpertise = teacherProfile.SubjectExpertise,
                        TeacherType = teacherProfile.TeacherType,
                        TeachingExperienceYears = teacherProfile.TeachingExperienceYears,
                        Bio = teacherProfile.Bio,
                        IsActive = teacherProfile.IsActive
                    };
                }

                result.Add(staffRes);
            }

            return ApiResponse<List<StaffResponse>>.Ok(result);
        }

        public async Task<ApiResponse<StaffResponse>> GetStaffByIdAsync(Guid id)
        {
            var security = await ValidateSecurityAndGetBranchAsync("read");
            if (!security.Authorized)
            {
                return ApiResponse<StaffResponse>.Fail(security.Error);
            }

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
            if (user == null)
            {
                return ApiResponse<StaffResponse>.Fail("Staff user not found.");
            }

            // Branch Admin scoping check
            if (_currentUserService.RoleCode == RoleCodes.BranchAdmin && user.BranchId != security.BranchId)
            {
                return ApiResponse<StaffResponse>.Fail("Access denied. Staff is outside your assigned branch.");
            }

            var role = await _unitOfWork.Repository<Role>().GetByIdAsync(user.RoleId);
            var branch = user.BranchId.HasValue ? await _unitOfWork.Repository<Branch>().GetByIdAsync(user.BranchId.Value) : null;
            var staffProfile = (await _unitOfWork.Repository<StaffProfile>().GetAllAsync()).FirstOrDefault(sp => sp.UserId == user.Id);
            var teacherProfile = (await _unitOfWork.Repository<TeacherProfile>().GetAllAsync()).FirstOrDefault(tp => tp.UserId == user.Id);

            var response = new StaffResponse
            {
                Id = user.Id,
                InstituteId = user.InstituteId,
                BranchId = user.BranchId,
                BranchName = branch?.Name,
                FullName = user.FullName,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                Username = user.Username,
                RoleId = user.RoleId,
                RoleName = role?.RoleName ?? string.Empty,
                RoleCode = role?.Code ?? string.Empty,
                IsPasswordChanged = user.IsPasswordChanged,
                IsActive = user.IsActive,
                LastLoginOn = user.LastLoginOn,
                CreatedBy = user.CreatedBy,
                CreatedOn = user.CreatedAt,
                UpdatedBy = user.UpdatedBy,
                UpdatedOn = user.UpdatedAt
            };

            if (staffProfile != null)
            {
                response.StaffProfile = new StaffProfileResponse
                {
                    Id = staffProfile.Id,
                    StaffCode = staffProfile.StaffCode,
                    StaffType = staffProfile.StaffType,
                    JoiningDate = staffProfile.JoiningDate,
                    Designation = staffProfile.Designation,
                    Department = staffProfile.Department,
                    Qualification = staffProfile.Qualification,
                    ExperienceYears = staffProfile.ExperienceYears,
                    Address = staffProfile.Address,
                    ProfilePhoto = staffProfile.ProfilePhoto,
                    EmergencyContactName = staffProfile.EmergencyContactName,
                    EmergencyContactNumber = staffProfile.EmergencyContactNumber,
                    IsActive = staffProfile.IsActive
                };
            }

            if (teacherProfile != null)
            {
                response.TeacherProfile = new TeacherProfileResponse
                {
                    Id = teacherProfile.Id,
                    SubjectExpertise = teacherProfile.SubjectExpertise,
                    TeacherType = teacherProfile.TeacherType,
                    TeachingExperienceYears = teacherProfile.TeachingExperienceYears,
                    Bio = teacherProfile.Bio,
                    IsActive = teacherProfile.IsActive
                };
            }

            return ApiResponse<StaffResponse>.Ok(response);
        }

        public async Task<ApiResponse<bool>> UpdateStaffAsync(Guid id, UpdateStaffRequest request)
        {
            var security = await ValidateSecurityAndGetBranchAsync("update", request.BranchId, request.RoleId);
            if (!security.Authorized)
            {
                return ApiResponse<bool>.Fail(security.Error);
            }

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
            if (user == null)
            {
                return ApiResponse<bool>.Fail("Staff user not found.");
            }

            // Branch Admin scoping check
            if (_currentUserService.RoleCode == RoleCodes.BranchAdmin && user.BranchId != security.BranchId)
            {
                return ApiResponse<bool>.Fail("Access denied. Staff is outside your assigned branch.");
            }

            var instituteId = _currentUserService.InstituteId!.Value;

            // Validate email/mobile uniqueness
            var emailExists = !string.IsNullOrEmpty(request.Email) && await _unitOfWork.Repository<User>()
                .AnyAsync(u => u.Email.ToLower() == request.Email.ToLower() && u.InstituteId == instituteId && u.Id != id);
            
            if (emailExists)
            {
                return ApiResponse<bool>.Fail("Email must be unique inside the Institute.");
            }

            var mobileExists = !string.IsNullOrEmpty(request.MobileNumber) && await _unitOfWork.Repository<User>()
                .AnyAsync(u => u.MobileNumber == request.MobileNumber && u.InstituteId == instituteId && u.Id != id);

            if (mobileExists)
            {
                return ApiResponse<bool>.Fail("Mobile Number must be unique inside the Institute.");
            }

            var role = await _unitOfWork.Repository<Role>().GetByIdAsync(request.RoleId);
            if (role == null)
            {
                return ApiResponse<bool>.Fail("Selected role not found.");
            }

            var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(request.BranchId);
            if (branch == null)
            {
                return ApiResponse<bool>.Fail("Selected branch not found.");
            }

            // If role is Teacher, validate that subject expertise / teacher type is present
            if (role.Code == RoleCodes.Teacher)
            {
                if (string.IsNullOrEmpty(request.SubjectExpertise))
                {
                    return ApiResponse<bool>.Fail("Subject expertise is required for Teacher role.");
                }
                if (string.IsNullOrEmpty(request.TeacherType))
                {
                    return ApiResponse<bool>.Fail("Teacher type is required for Teacher role.");
                }
            }

            // Update User details
            user.FullName = request.FullName;
            user.Email = request.Email ?? string.Empty;
            user.MobileNumber = request.MobileNumber;
            user.RoleId = request.RoleId;
            user.BranchId = request.BranchId;
            user.IsActive = request.IsActive;

            _unitOfWork.Repository<User>().Update(user);

            // Update StaffProfile details
            var staffProfile = (await _unitOfWork.Repository<StaffProfile>().GetAllAsync()).FirstOrDefault(sp => sp.UserId == user.Id);
            if (staffProfile != null)
            {
                staffProfile.StaffType = request.StaffType;
                staffProfile.JoiningDate = request.JoiningDate;
                staffProfile.Designation = request.Designation;
                staffProfile.Department = request.Department;
                staffProfile.Qualification = request.Qualification;
                staffProfile.ExperienceYears = request.ExperienceYears;
                staffProfile.Address = request.Address;
                staffProfile.ProfilePhoto = request.ProfilePhoto;
                staffProfile.EmergencyContactName = request.EmergencyContactName;
                staffProfile.EmergencyContactNumber = request.EmergencyContactNumber;
                staffProfile.IsActive = request.IsActive;

                _unitOfWork.Repository<StaffProfile>().Update(staffProfile);
            }

            // Handle TeacherProfile details
            var teacherProfile = (await _unitOfWork.Repository<TeacherProfile>().GetAllAsync()).FirstOrDefault(tp => tp.UserId == user.Id);
            if (role.Code == RoleCodes.Teacher)
            {
                if (teacherProfile == null)
                {
                    // Create if not exists
                    teacherProfile = new TeacherProfile
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        InstituteId = instituteId,
                        SubjectExpertise = request.SubjectExpertise ?? string.Empty,
                        TeacherType = request.TeacherType ?? string.Empty,
                        TeachingExperienceYears = request.TeachingExperienceYears,
                        Bio = request.Bio,
                        IsActive = request.IsActive
                    };
                    await _unitOfWork.Repository<TeacherProfile>().AddAsync(teacherProfile);
                }
                else
                {
                    // Update
                    teacherProfile.SubjectExpertise = request.SubjectExpertise ?? string.Empty;
                    teacherProfile.TeacherType = request.TeacherType ?? string.Empty;
                    teacherProfile.TeachingExperienceYears = request.TeachingExperienceYears;
                    teacherProfile.Bio = request.Bio;
                    teacherProfile.IsActive = request.IsActive;

                    _unitOfWork.Repository<TeacherProfile>().Update(teacherProfile);
                }
            }
            else
            {
                // If it was a teacher but role changed, deactivate or delete the teacher profile
                if (teacherProfile != null)
                {
                    _unitOfWork.Repository<TeacherProfile>().Remove(teacherProfile);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Staff details updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteStaffAsync(Guid id)
        {
            var security = await ValidateSecurityAndGetBranchAsync("delete");
            if (!security.Authorized)
            {
                return ApiResponse<bool>.Fail(security.Error);
            }

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
            if (user == null)
            {
                return ApiResponse<bool>.Fail("Staff user not found.");
            }

            // Branch Admin scoping check
            if (_currentUserService.RoleCode == RoleCodes.BranchAdmin && user.BranchId != security.BranchId)
            {
                return ApiResponse<bool>.Fail("Access denied. Staff is outside your assigned branch.");
            }

            // Soft-deleting staff user
            _unitOfWork.Repository<User>().Remove(user);

            var staffProfile = (await _unitOfWork.Repository<StaffProfile>().GetAllAsync()).FirstOrDefault(sp => sp.UserId == user.Id);
            if (staffProfile != null)
            {
                _unitOfWork.Repository<StaffProfile>().Remove(staffProfile);
            }

            var teacherProfile = (await _unitOfWork.Repository<TeacherProfile>().GetAllAsync()).FirstOrDefault(tp => tp.UserId == user.Id);
            if (teacherProfile != null)
            {
                _unitOfWork.Repository<TeacherProfile>().Remove(teacherProfile);
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Staff deactivated/deleted successfully.");
        }

        public async Task<ApiResponse<List<StaffResponse>>> GetStaffByRoleAsync(string roleName)
        {
            var security = await ValidateSecurityAndGetBranchAsync("read");
            if (!security.Authorized)
            {
                return ApiResponse<List<StaffResponse>>.Fail(security.Error);
            }

            var role = (await _unitOfWork.Repository<Role>().GetAllAsync())
                .FirstOrDefault(r => r.Code.Equals(roleName, StringComparison.OrdinalIgnoreCase) || r.RoleName.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            
            if (role == null)
            {
                return ApiResponse<List<StaffResponse>>.Fail("Role not found.");
            }

            var allStaff = await GetAllStaffAsync();
            if (!allStaff.Success || allStaff.Data == null) 
            {
                return ApiResponse<List<StaffResponse>>.Fail(allStaff.Message ?? "Failed to retrieve staff.");
            }

            var result = allStaff.Data.Where(s => s.RoleId == role.Id).ToList();
            return ApiResponse<List<StaffResponse>>.Ok(result);
        }

        public async Task<ApiResponse<List<TeacherResponse>>> GetTeachersAsync()
        {
            // Teachers list can be read by Staff/Admins, or Teachers
            var callerUserId = _currentUserService.UserId;
            var callerInstituteId = _currentUserService.InstituteId;
            var callerRoleCode = _currentUserService.RoleCode;

            if (callerUserId == null || callerInstituteId == null)
            {
                return ApiResponse<List<TeacherResponse>>.Fail("Unauthorized.");
            }

            if (callerRoleCode == RoleCodes.Student)
            {
                return ApiResponse<List<TeacherResponse>>.Fail("Access denied.");
            }

            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            var staffProfiles = await _unitOfWork.Repository<StaffProfile>().GetAllAsync();
            var teacherProfiles = await _unitOfWork.Repository<TeacherProfile>().GetAllAsync();
            var roles = await _unitOfWork.Repository<Role>().GetAllAsync();
            var branches = await _unitOfWork.Repository<Branch>().GetAllAsync();

            var teacherRole = roles.FirstOrDefault(r => r.Code == RoleCodes.Teacher);
            if (teacherRole == null)
            {
                return ApiResponse<List<TeacherResponse>>.Ok(new List<TeacherResponse>());
            }

            var callerUser = users.FirstOrDefault(u => u.Id == callerUserId.Value);
            var callerBranchId = callerUser?.BranchId;

            // Filter branch if branch admin
            var filteredUsers = users.Where(u => u.RoleId == teacherRole.Id);
            if (callerRoleCode == RoleCodes.BranchAdmin)
            {
                filteredUsers = filteredUsers.Where(u => u.BranchId == callerBranchId);
            }

            var result = new List<TeacherResponse>();
            foreach (var user in filteredUsers)
            {
                var staffProfile = staffProfiles.FirstOrDefault(sp => sp.UserId == user.Id);
                var teacherProfile = teacherProfiles.FirstOrDefault(tp => tp.UserId == user.Id);
                var branch = branches.FirstOrDefault(b => b.Id == user.BranchId);

                var teacherRes = new TeacherResponse
                {
                    Id = user.Id,
                    InstituteId = user.InstituteId,
                    BranchId = user.BranchId,
                    BranchName = branch?.Name,
                    FullName = user.FullName,
                    Email = user.Email,
                    MobileNumber = user.MobileNumber,
                    Username = user.Username,
                    IsActive = user.IsActive
                };

                if (staffProfile != null)
                {
                    teacherRes.StaffProfile = new StaffProfileResponse
                    {
                        Id = staffProfile.Id,
                        StaffCode = staffProfile.StaffCode,
                        StaffType = staffProfile.StaffType,
                        JoiningDate = staffProfile.JoiningDate,
                        Designation = staffProfile.Designation,
                        Department = staffProfile.Department,
                        Qualification = staffProfile.Qualification,
                        ExperienceYears = staffProfile.ExperienceYears,
                        Address = staffProfile.Address,
                        ProfilePhoto = staffProfile.ProfilePhoto,
                        EmergencyContactName = staffProfile.EmergencyContactName,
                        EmergencyContactNumber = staffProfile.EmergencyContactNumber,
                        IsActive = staffProfile.IsActive
                    };
                }

                if (teacherProfile != null)
                {
                    teacherRes.TeacherProfile = new TeacherProfileResponse
                    {
                        Id = teacherProfile.Id,
                        SubjectExpertise = teacherProfile.SubjectExpertise,
                        TeacherType = teacherProfile.TeacherType,
                        TeachingExperienceYears = teacherProfile.TeachingExperienceYears,
                        Bio = teacherProfile.Bio,
                        IsActive = teacherProfile.IsActive
                    };
                }

                result.Add(teacherRes);
            }

            return ApiResponse<List<TeacherResponse>>.Ok(result);
        }

        public async Task<ApiResponse<TeacherResponse>> GetTeacherByIdAsync(Guid id)
        {
            var callerUserId = _currentUserService.UserId;
            var callerInstituteId = _currentUserService.InstituteId;
            var callerRoleCode = _currentUserService.RoleCode;

            if (callerUserId == null || callerInstituteId == null)
            {
                return ApiResponse<TeacherResponse>.Fail("Unauthorized.");
            }

            if (callerRoleCode == RoleCodes.Student)
            {
                return ApiResponse<TeacherResponse>.Fail("Access denied.");
            }

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
            if (user == null)
            {
                return ApiResponse<TeacherResponse>.Fail("Teacher user not found.");
            }

            var role = await _unitOfWork.Repository<Role>().GetByIdAsync(user.RoleId);
            if (role == null || role.Code != RoleCodes.Teacher)
            {
                return ApiResponse<TeacherResponse>.Fail("Selected user is not a teacher.");
            }

            // Branch Admin scoping check
            if (callerRoleCode == RoleCodes.BranchAdmin)
            {
                var callerUser = (await _unitOfWork.Repository<User>().GetAllAsync()).FirstOrDefault(u => u.Id == callerUserId.Value);
                if (user.BranchId != callerUser?.BranchId)
                {
                    return ApiResponse<TeacherResponse>.Fail("Access denied. Teacher is outside your branch.");
                }
            }

            var branch = user.BranchId.HasValue ? await _unitOfWork.Repository<Branch>().GetByIdAsync(user.BranchId.Value) : null;
            var staffProfile = (await _unitOfWork.Repository<StaffProfile>().GetAllAsync()).FirstOrDefault(sp => sp.UserId == user.Id);
            var teacherProfile = (await _unitOfWork.Repository<TeacherProfile>().GetAllAsync()).FirstOrDefault(tp => tp.UserId == user.Id);

            var response = new TeacherResponse
            {
                Id = user.Id,
                InstituteId = user.InstituteId,
                BranchId = user.BranchId,
                BranchName = branch?.Name,
                FullName = user.FullName,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                Username = user.Username,
                IsActive = user.IsActive
            };

            if (staffProfile != null)
            {
                response.StaffProfile = new StaffProfileResponse
                {
                    Id = staffProfile.Id,
                    StaffCode = staffProfile.StaffCode,
                    StaffType = staffProfile.StaffType,
                    JoiningDate = staffProfile.JoiningDate,
                    Designation = staffProfile.Designation,
                    Department = staffProfile.Department,
                    Qualification = staffProfile.Qualification,
                    ExperienceYears = staffProfile.ExperienceYears,
                    Address = staffProfile.Address,
                    ProfilePhoto = staffProfile.ProfilePhoto,
                    EmergencyContactName = staffProfile.EmergencyContactName,
                    EmergencyContactNumber = staffProfile.EmergencyContactNumber,
                    IsActive = staffProfile.IsActive
                };
            }

            if (teacherProfile != null)
            {
                response.TeacherProfile = new TeacherProfileResponse
                {
                    Id = teacherProfile.Id,
                    SubjectExpertise = teacherProfile.SubjectExpertise,
                    TeacherType = teacherProfile.TeacherType,
                    TeachingExperienceYears = teacherProfile.TeachingExperienceYears,
                    Bio = teacherProfile.Bio,
                    IsActive = teacherProfile.IsActive
                };
            }

            return ApiResponse<TeacherResponse>.Ok(response);
        }

        public async Task<ApiResponse<bool>> UpdateTeacherProfileAsync(Guid id, UpdateTeacherProfileRequest request)
        {
            var callerUserId = _currentUserService.UserId;
            var callerInstituteId = _currentUserService.InstituteId;
            var callerRoleCode = _currentUserService.RoleCode;

            if (callerUserId == null || callerInstituteId == null)
            {
                return ApiResponse<bool>.Fail("Unauthorized.");
            }

            // Security check: Only Admins can update any teacher profile, Teachers can only update their own profile.
            if (callerRoleCode == RoleCodes.Teacher && id != callerUserId.Value)
            {
                return ApiResponse<bool>.Fail("Access denied. Teachers can only update their own profile.");
            }

            if (callerRoleCode == RoleCodes.Student)
            {
                return ApiResponse<bool>.Fail("Access denied.");
            }

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
            if (user == null)
            {
                return ApiResponse<bool>.Fail("Teacher user not found.");
            }

            var role = await _unitOfWork.Repository<Role>().GetByIdAsync(user.RoleId);
            if (role == null || role.Code != RoleCodes.Teacher)
            {
                return ApiResponse<bool>.Fail("Selected user is not a teacher.");
            }

            // Branch Admin scoping check
            if (callerRoleCode == RoleCodes.BranchAdmin)
            {
                var callerUser = (await _unitOfWork.Repository<User>().GetAllAsync()).FirstOrDefault(u => u.Id == callerUserId.Value);
                if (user.BranchId != callerUser?.BranchId)
                {
                    return ApiResponse<bool>.Fail("Access denied. Teacher is outside your branch.");
                }
            }

            var teacherProfile = (await _unitOfWork.Repository<TeacherProfile>().GetAllAsync()).FirstOrDefault(tp => tp.UserId == user.Id);
            if (teacherProfile == null)
            {
                // Create if not exists
                teacherProfile = new TeacherProfile
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    InstituteId = user.InstituteId,
                    SubjectExpertise = request.SubjectExpertise ?? string.Empty,
                    TeacherType = request.TeacherType ?? string.Empty,
                    TeachingExperienceYears = request.TeachingExperienceYears,
                    Bio = request.Bio,
                    IsActive = true
                };
                await _unitOfWork.Repository<TeacherProfile>().AddAsync(teacherProfile);
            }
            else
            {
                // Update
                teacherProfile.SubjectExpertise = request.SubjectExpertise ?? string.Empty;
                teacherProfile.TeacherType = request.TeacherType ?? string.Empty;
                teacherProfile.TeachingExperienceYears = request.TeachingExperienceYears;
                teacherProfile.Bio = request.Bio;

                _unitOfWork.Repository<TeacherProfile>().Update(teacherProfile);
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Teacher profile updated successfully.");
        }

        public async Task<ApiResponse<List<RoleDto>>> GetRolesAsync()
        {
            var roles = await _unitOfWork.Repository<Role>().GetAllAsync();
            var dtos = roles
                .Where(r => r.Code != RoleCodes.Student && r.Code != RoleCodes.SuperAdmin && r.Code != "GLOBAL_ADMIN")
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    RoleName = r.RoleName,
                    Code = r.Code,
                    Description = r.Description
                })
                .ToList();

            return ApiResponse<List<RoleDto>>.Ok(dtos);
        }
    }
}

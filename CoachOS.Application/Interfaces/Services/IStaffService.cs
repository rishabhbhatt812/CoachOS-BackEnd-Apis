using CoachOS.Application.Features.Staff.Dtos;
using CoachOS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IStaffService
    {
        Task<ApiResponse<Guid>> CreateStaffAsync(CreateStaffRequest request);
        Task<ApiResponse<List<StaffResponse>>> GetAllStaffAsync();
        Task<ApiResponse<StaffResponse>> GetStaffByIdAsync(Guid id);
        Task<ApiResponse<bool>> UpdateStaffAsync(Guid id, UpdateStaffRequest request);
        Task<ApiResponse<bool>> DeleteStaffAsync(Guid id);
        Task<ApiResponse<List<StaffResponse>>> GetStaffByRoleAsync(string roleName);

        // Teacher-specific APIs
        Task<ApiResponse<List<TeacherResponse>>> GetTeachersAsync();
        Task<ApiResponse<TeacherResponse>> GetTeacherByIdAsync(Guid id);
        Task<ApiResponse<bool>> UpdateTeacherProfileAsync(Guid id, UpdateTeacherProfileRequest request);
        Task<ApiResponse<List<RoleDto>>> GetRolesAsync();
    }
}

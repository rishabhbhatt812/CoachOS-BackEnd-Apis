using CoachOS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IStudentPortalService
    {
        Task<ApiResponse<object>> GetStudentDashboardAsync(Guid studentId);
        Task<ApiResponse<object>> GetStudentCoursesAsync(Guid studentId);
        Task<ApiResponse<object>> GetStudentFeesAsync(Guid studentId);
        Task<ApiResponse<object>> GetStudentNotesAsync(Guid studentId);
        Task<ApiResponse<object>> GetStudentAttendanceAsync(Guid studentId);
        Task<ApiResponse<object>> GetStudentResultsAsync(Guid studentId);
        Task<ApiResponse<object>> GetStudentVacanciesAsync(Guid studentId);
    }
}

using CoachOS.Application.Features.Students.Dtos;
using CoachOS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IStudentService
    {
        Task<ApiResponse<StudentDto>> CreateStudentAsync(CreateStudentRequest request);
        Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<StudentDto>>> GetStudentsAsync(CoachOS.Shared.Requests.PaginationParams paginationParams);
        Task<ApiResponse<StudentDto>> GetStudentByIdAsync(Guid id);
        Task<ApiResponse<StudentDto>> UpdateStudentAsync(Guid id, UpdateStudentRequest request);
        Task<ApiResponse<bool>> DeleteStudentAsync(Guid id);
        Task<ApiResponse<string>> GetNextStudentCodeAsync(string prefix);
        Task<ApiResponse<string>> UploadProfilePictureAsync(Guid id, System.IO.Stream fileStream, string fileName);
    }
}

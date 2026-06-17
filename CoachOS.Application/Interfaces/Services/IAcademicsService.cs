using CoachOS.Application.Features.Academics.Dtos;
using CoachOS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IAcademicsService
    {
        Task<ApiResponse<CourseDto>> CreateCourseAsync(CreateCourseRequest request);
        Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<CourseDto>>> GetCoursesAsync(CoachOS.Shared.Requests.PaginationParams paginationParams);
        Task<ApiResponse<CourseDto>> GetCourseByIdAsync(Guid id);
        Task<ApiResponse<CourseDto>> UpdateCourseAsync(Guid id, UpdateCourseRequest request);
        Task<ApiResponse<bool>> DeleteCourseAsync(Guid id);

        // Subjects
        Task<ApiResponse<SubjectDto>> CreateSubjectAsync(CreateSubjectRequest request);
        Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<SubjectDto>>> GetSubjectsAsync(CoachOS.Shared.Requests.PaginationParams paginationParams);
        Task<ApiResponse<SubjectDto>> GetSubjectByIdAsync(Guid id);
        Task<ApiResponse<SubjectDto>> UpdateSubjectAsync(Guid id, UpdateSubjectRequest request);
        Task<ApiResponse<bool>> DeleteSubjectAsync(Guid id);

        // Batches
        Task<ApiResponse<BatchDto>> CreateBatchAsync(CreateBatchRequest request);
        Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<BatchDto>>> GetBatchesAsync(CoachOS.Shared.Requests.PaginationParams paginationParams);
        Task<ApiResponse<BatchDto>> GetBatchByIdAsync(Guid id);
        Task<ApiResponse<BatchDto>> UpdateBatchAsync(Guid id, UpdateBatchRequest request);
        Task<ApiResponse<bool>> DeleteBatchAsync(Guid id);
    }
}

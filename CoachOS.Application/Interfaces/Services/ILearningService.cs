using CoachOS.Application.Features.Learning.Dtos;
using CoachOS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IAttendanceService
    {
        Task<ApiResponse<AttendanceSessionDto>> CreateAttendanceSessionAsync(CreateAttendanceSessionRequest request);
        Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<AttendanceSessionDto>>> GetAttendanceSessionsAsync(CoachOS.Shared.Requests.PaginationParams paginationParams);
        Task<ApiResponse<bool>> DeleteAttendanceSessionAsync(Guid id);
    }

    public interface ILearningService
    {
        Task<ApiResponse<NoteDto>> CreateNoteAsync(CreateNoteRequest request);
        Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<NoteDto>>> GetNotesAsync(CoachOS.Shared.Requests.PaginationParams paginationParams);
        Task<ApiResponse<bool>> DeleteNoteAsync(Guid id);

        Task<ApiResponse<TestDto>> CreateTestAsync(CreateTestRequest request);
        Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<TestDto>>> GetTestsAsync(CoachOS.Shared.Requests.PaginationParams paginationParams);
        Task<ApiResponse<bool>> DeleteTestAsync(Guid id);
    }
}

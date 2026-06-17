using CoachOS.Application.Features.Communication.Dtos;
using CoachOS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface ICommunicationService
    {
        Task<ApiResponse<NoticeDto>> CreateNoticeAsync(CreateNoticeRequest request);
        Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<NoticeDto>>> GetNoticesAsync(CoachOS.Shared.Requests.PaginationParams paginationParams);
        Task<ApiResponse<bool>> DeleteNoticeAsync(Guid id);

        Task<ApiResponse<VacancyDto>> CreateVacancyAsync(CreateVacancyRequest request);
        Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<VacancyDto>>> GetVacanciesAsync(CoachOS.Shared.Requests.PaginationParams paginationParams);
        Task<ApiResponse<bool>> DeleteVacancyAsync(Guid id);
    }
}

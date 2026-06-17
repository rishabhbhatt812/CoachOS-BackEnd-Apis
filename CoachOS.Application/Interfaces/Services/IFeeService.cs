using CoachOS.Application.Features.Finance.Dtos;
using CoachOS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IFeeService
    {
        Task<ApiResponse<FeePlanDto>> CreateFeePlanAsync(CreateFeePlanRequest request);
        Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<FeePlanDto>>> GetFeePlansAsync(CoachOS.Shared.Requests.PaginationParams paginationParams);
        Task<ApiResponse<FeePlanDto>> GetFeePlanByIdAsync(Guid id);
        Task<ApiResponse<bool>> DeleteFeePlanAsync(Guid id);
    }
}

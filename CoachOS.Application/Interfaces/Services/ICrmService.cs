using CoachOS.Application.Features.Crm.Dtos;
using CoachOS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface ICrmService
    {
        Task<ApiResponse<EnquiryDto>> CreateEnquiryAsync(CreateEnquiryRequest request);
        Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<EnquiryDto>>> GetEnquiriesAsync(CoachOS.Shared.Requests.PaginationParams paginationParams);
        Task<ApiResponse<EnquiryDto>> GetEnquiryByIdAsync(Guid id);
        Task<ApiResponse<EnquiryDto>> UpdateEnquiryAsync(Guid id, UpdateEnquiryRequest request);
        Task<ApiResponse<bool>> DeleteEnquiryAsync(Guid id);
    }
}

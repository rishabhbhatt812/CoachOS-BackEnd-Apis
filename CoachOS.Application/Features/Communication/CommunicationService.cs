using CoachOS.Application.Features.Communication.Dtos;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Communication;
using CoachOS.Shared.Responses;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Application.Features.Communication
{
    public class CommunicationService : ICommunicationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommunicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<NoticeDto>> CreateNoticeAsync(CreateNoticeRequest request)
        {
            var notice = request.Adapt<Notice>();
            await _unitOfWork.Repository<Notice>().AddAsync(notice);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<NoticeDto>.Ok(notice.Adapt<NoticeDto>(), "Notice created successfully.");
        }

        public async Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<NoticeDto>>> GetNoticesAsync(CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            var noticesPaged = await _unitOfWork.Repository<Notice>().GetPagedAsync(paginationParams);
            var noticesDto = noticesPaged.Data.Adapt<List<NoticeDto>>();
            var result = new CoachOS.Shared.Responses.PagedResult<NoticeDto>(noticesDto, noticesPaged.TotalCount, noticesPaged.CurrentPage, noticesPaged.PageSize);
            return ApiResponse<CoachOS.Shared.Responses.PagedResult<NoticeDto>>.Ok(result);
        }

        public async Task<ApiResponse<bool>> DeleteNoticeAsync(Guid id)
        {
            var notice = await _unitOfWork.Repository<Notice>().GetByIdAsync(id);
            if (notice == null) return ApiResponse<bool>.Fail("Notice not found.");

            _unitOfWork.Repository<Notice>().Remove(notice);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Notice deleted successfully.");
        }

        public async Task<ApiResponse<VacancyDto>> CreateVacancyAsync(CreateVacancyRequest request)
        {
            var vacancy = request.Adapt<Vacancy>();
            await _unitOfWork.Repository<Vacancy>().AddAsync(vacancy);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<VacancyDto>.Ok(vacancy.Adapt<VacancyDto>(), "Vacancy created successfully.");
        }

        public async Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<VacancyDto>>> GetVacanciesAsync(CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            var vacanciesPaged = await _unitOfWork.Repository<Vacancy>().GetPagedAsync(paginationParams);
            var vacanciesDto = vacanciesPaged.Data.Adapt<List<VacancyDto>>();
            var result = new CoachOS.Shared.Responses.PagedResult<VacancyDto>(vacanciesDto, vacanciesPaged.TotalCount, vacanciesPaged.CurrentPage, vacanciesPaged.PageSize);
            return ApiResponse<CoachOS.Shared.Responses.PagedResult<VacancyDto>>.Ok(result);
        }

        public async Task<ApiResponse<bool>> DeleteVacancyAsync(Guid id)
        {
            var vacancy = await _unitOfWork.Repository<Vacancy>().GetByIdAsync(id);
            if (vacancy == null) return ApiResponse<bool>.Fail("Vacancy not found.");

            _unitOfWork.Repository<Vacancy>().Remove(vacancy);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Vacancy deleted successfully.");
        }
    }
}

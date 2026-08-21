using CoachOS.Application.Features.Crm.Dtos;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.CRM;
using CoachOS.Shared.Responses;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Application.Features.Crm
{
    public class CrmService : ICrmService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CrmService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<EnquiryDto>> CreateEnquiryAsync(CreateEnquiryRequest request)
        {
            var enquiry = request.Adapt<Enquiry>();
            enquiry.Status = "New"; // Default status
            
            await _unitOfWork.Repository<Enquiry>().AddAsync(enquiry);
            await _unitOfWork.SaveChangesAsync();
            
            var dto = enquiry.Adapt<EnquiryDto>();
            if (enquiry.InterestedCourseId.HasValue)
            {
                var course = await _unitOfWork.Repository<CoachOS.Domain.Academic.Course>().GetByIdAsync(enquiry.InterestedCourseId.Value);
                dto.InterestedCourseName = course?.Name ?? "Unknown";
            }
            return ApiResponse<EnquiryDto>.Ok(dto, "Enquiry created successfully.");
        }

        public async Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<EnquiryDto>>> GetEnquiriesAsync(CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            var enquiriesPaged = await _unitOfWork.Repository<Enquiry>().GetPagedAsync(paginationParams);
            var courses = await _unitOfWork.Repository<CoachOS.Domain.Academic.Course>().GetAllAsync();
            
            var enquiriesDto = enquiriesPaged.Data.Select(e =>
            {
                var dto = e.Adapt<EnquiryDto>();
                if (e.InterestedCourseId.HasValue)
                {
                    dto.InterestedCourseName = courses.FirstOrDefault(c => c.Id == e.InterestedCourseId.Value)?.Name ?? "Unknown";
                }
                return dto;
            }).ToList();

            var result = new CoachOS.Shared.Responses.PagedResult<EnquiryDto>(enquiriesDto, enquiriesPaged.TotalCount, enquiriesPaged.CurrentPage, enquiriesPaged.PageSize);
            return ApiResponse<CoachOS.Shared.Responses.PagedResult<EnquiryDto>>.Ok(result);
        }

        public async Task<ApiResponse<EnquiryDto>> GetEnquiryByIdAsync(Guid id)
        {
            var enquiry = await _unitOfWork.Repository<Enquiry>().GetByIdAsync(id);
            if (enquiry == null) return ApiResponse<EnquiryDto>.Fail("Enquiry not found.");
            
            var dto = enquiry.Adapt<EnquiryDto>();
            if (enquiry.InterestedCourseId.HasValue)
            {
                var course = await _unitOfWork.Repository<CoachOS.Domain.Academic.Course>().GetByIdAsync(enquiry.InterestedCourseId.Value);
                dto.InterestedCourseName = course?.Name ?? "Unknown";
            }
            return ApiResponse<EnquiryDto>.Ok(dto);
        }

        public async Task<ApiResponse<EnquiryDto>> UpdateEnquiryAsync(Guid id, UpdateEnquiryRequest request)
        {
            var enquiry = await _unitOfWork.Repository<Enquiry>().GetByIdAsync(id);
            if (enquiry == null) return ApiResponse<EnquiryDto>.Fail("Enquiry not found.");

            request.Adapt(enquiry);
            _unitOfWork.Repository<Enquiry>().Update(enquiry);
            await _unitOfWork.SaveChangesAsync();
            
            var dto = enquiry.Adapt<EnquiryDto>();
            if (enquiry.InterestedCourseId.HasValue)
            {
                var course = await _unitOfWork.Repository<CoachOS.Domain.Academic.Course>().GetByIdAsync(enquiry.InterestedCourseId.Value);
                dto.InterestedCourseName = course?.Name ?? "Unknown";
            }
            return ApiResponse<EnquiryDto>.Ok(dto, "Enquiry updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteEnquiryAsync(Guid id)
        {
            var enquiry = await _unitOfWork.Repository<Enquiry>().GetByIdAsync(id);
            if (enquiry == null) return ApiResponse<bool>.Fail("Enquiry not found.");

            _unitOfWork.Repository<Enquiry>().Remove(enquiry);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Enquiry deleted successfully.");
        }

        public async Task<ApiResponse<FollowUpDto>> AddFollowUpAsync(CreateFollowUpRequest request)
        {
            var enquiry = await _unitOfWork.Repository<Enquiry>().GetByIdAsync(request.EnquiryId);
            if (enquiry == null) return ApiResponse<FollowUpDto>.Fail("Enquiry not found.");

            var followUp = new FollowUp
            {
                EnquiryId = request.EnquiryId,
                FollowUpDate = request.FollowUpDate,
                NextFollowUpDate = request.NextFollowUpDate,
                Remark = request.Remark
            };

            await _unitOfWork.Repository<FollowUp>().AddAsync(followUp);
            enquiry.Status = "FollowedUp";
            _unitOfWork.Repository<Enquiry>().Update(enquiry);
            await _unitOfWork.SaveChangesAsync();

            var dto = new FollowUpDto
            {
                Id = followUp.Id,
                EnquiryId = followUp.EnquiryId,
                FollowUpDate = followUp.FollowUpDate,
                NextFollowUpDate = followUp.NextFollowUpDate,
                Remark = followUp.Remark
            };

            return ApiResponse<FollowUpDto>.Ok(dto, "Follow-up added successfully.");
        }

        public async Task<ApiResponse<DemoClassDto>> ScheduleDemoClassAsync(ScheduleDemoClassRequest request)
        {
            var enquiry = await _unitOfWork.Repository<Enquiry>().GetByIdAsync(request.EnquiryId);
            if (enquiry == null) return ApiResponse<DemoClassDto>.Fail("Enquiry not found.");

            var demo = new DemoClass
            {
                EnquiryId = request.EnquiryId,
                BatchId = request.BatchId,
                DemoDate = request.DemoDate,
                Status = "Scheduled",
                Remark = request.Remark
            };

            await _unitOfWork.Repository<DemoClass>().AddAsync(demo);
            enquiry.Status = "DemoScheduled";
            _unitOfWork.Repository<Enquiry>().Update(enquiry);
            await _unitOfWork.SaveChangesAsync();

            var batch = request.BatchId.HasValue ? await _unitOfWork.Repository<CoachOS.Domain.Academic.Batch>().GetByIdAsync(request.BatchId.Value) : null;

            var dto = new DemoClassDto
            {
                Id = demo.Id,
                EnquiryId = demo.EnquiryId,
                BatchId = demo.BatchId,
                BatchName = batch?.Name ?? "General Demo",
                DemoDate = demo.DemoDate,
                Status = demo.Status,
                Remark = demo.Remark
            };

            return ApiResponse<DemoClassDto>.Ok(dto, "Demo class scheduled successfully.");
        }
    }
}

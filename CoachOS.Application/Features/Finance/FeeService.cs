using CoachOS.Application.Features.Finance.Dtos;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Finance;
using CoachOS.Shared.Responses;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Application.Features.Finance
{
    public class FeeService : IFeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<FeePlanDto>> CreateFeePlanAsync(CreateFeePlanRequest request)
        {
            var feePlan = request.Adapt<FeePlan>();
            feePlan.FinalFee = request.TotalFee - request.DiscountAmount;
            
            await _unitOfWork.Repository<FeePlan>().AddAsync(feePlan);
            await _unitOfWork.SaveChangesAsync();

            // Create installments automatically
            if (request.PlanType == "Installment")
            {
                var inst1 = new Installment
                {
                    FeePlanId = feePlan.Id,
                    InstallmentNo = 1,
                    Amount = feePlan.FinalFee / 2,
                    DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)),
                    Status = "Due",
                    InstituteId = feePlan.InstituteId
                };
                var inst2 = new Installment
                {
                    FeePlanId = feePlan.Id,
                    InstallmentNo = 2,
                    Amount = feePlan.FinalFee / 2,
                    DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                    Status = "Due",
                    InstituteId = feePlan.InstituteId
                };
                await _unitOfWork.Repository<Installment>().AddAsync(inst1);
                await _unitOfWork.Repository<Installment>().AddAsync(inst2);
            }
            else
            {
                var inst = new Installment
                {
                    FeePlanId = feePlan.Id,
                    InstallmentNo = 1,
                    Amount = feePlan.FinalFee,
                    DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)),
                    Status = "Due",
                    InstituteId = feePlan.InstituteId
                };
                await _unitOfWork.Repository<Installment>().AddAsync(inst);
            }
            await _unitOfWork.SaveChangesAsync();

            var dto = feePlan.Adapt<FeePlanDto>();
            var student = await _unitOfWork.Repository<CoachOS.Domain.Student.Student>().GetByIdAsync(feePlan.StudentId);
            var course = await _unitOfWork.Repository<CoachOS.Domain.Academic.Course>().GetByIdAsync(feePlan.CourseId);
            dto.StudentName = student?.FullName ?? "Unknown Student";
            dto.CourseName = course?.Name ?? "Unknown Course";
            dto.PaidAmount = 0;
            dto.DueAmount = feePlan.FinalFee;
            dto.Status = "Due";

            return ApiResponse<FeePlanDto>.Ok(dto, "Fee plan created successfully.");
        }

        public async Task<ApiResponse<CoachOS.Shared.Responses.PagedResult<FeePlanDto>>> GetFeePlansAsync(CoachOS.Shared.Requests.PaginationParams paginationParams)
        {
            var feePlansPaged = await _unitOfWork.Repository<FeePlan>().GetPagedAsync(paginationParams);
            var students = await _unitOfWork.Repository<CoachOS.Domain.Student.Student>().GetAllAsync();
            var courses = await _unitOfWork.Repository<CoachOS.Domain.Academic.Course>().GetAllAsync();
            var payments = await _unitOfWork.Repository<Payment>().GetAllAsync();
            var installments = await _unitOfWork.Repository<Installment>().GetAllAsync();

            var feePlansDto = feePlansPaged.Data.Select(fp =>
            {
                var dto = fp.Adapt<FeePlanDto>();
                dto.StudentName = students.FirstOrDefault(s => s.Id == fp.StudentId)?.FullName ?? "Unknown Student";
                dto.CourseName = courses.FirstOrDefault(c => c.Id == fp.CourseId)?.Name ?? "Unknown Course";
                
                var planPayments = payments.Where(p => p.FeePlanId == fp.Id).ToList();
                dto.PaidAmount = planPayments.Sum(p => p.Amount);
                dto.DueAmount = fp.FinalFee - dto.PaidAmount;
                
                var planInstallments = installments.Where(i => i.FeePlanId == fp.Id).ToList();
                if (dto.DueAmount <= 0)
                {
                    dto.Status = "Paid";
                }
                else if (planInstallments.Any(i => i.Status == "Overdue" || (i.Status == "Due" && i.DueDate < DateOnly.FromDateTime(DateTime.UtcNow))))
                {
                    dto.Status = "Overdue";
                }
                else
                {
                    dto.Status = "Due";
                }
                
                return dto;
            }).ToList();

            var result = new CoachOS.Shared.Responses.PagedResult<FeePlanDto>(feePlansDto, feePlansPaged.TotalCount, feePlansPaged.CurrentPage, feePlansPaged.PageSize);
            return ApiResponse<CoachOS.Shared.Responses.PagedResult<FeePlanDto>>.Ok(result);
        }

        public async Task<ApiResponse<FeePlanDto>> GetFeePlanByIdAsync(Guid id)
        {
            var feePlan = await _unitOfWork.Repository<FeePlan>().GetByIdAsync(id);
            if (feePlan == null) return ApiResponse<FeePlanDto>.Fail("Fee plan not found.");
            
            var dto = feePlan.Adapt<FeePlanDto>();
            var student = await _unitOfWork.Repository<CoachOS.Domain.Student.Student>().GetByIdAsync(feePlan.StudentId);
            var course = await _unitOfWork.Repository<CoachOS.Domain.Academic.Course>().GetByIdAsync(feePlan.CourseId);
            var payments = await _unitOfWork.Repository<Payment>().GetAllAsync();
            var installments = await _unitOfWork.Repository<Installment>().GetAllAsync();

            dto.StudentName = student?.FullName ?? "Unknown Student";
            dto.CourseName = course?.Name ?? "Unknown Course";
            
            var planPayments = payments.Where(p => p.FeePlanId == feePlan.Id).ToList();
            dto.PaidAmount = planPayments.Sum(p => p.Amount);
            dto.DueAmount = feePlan.FinalFee - dto.PaidAmount;
            
            var planInstallments = installments.Where(i => i.FeePlanId == feePlan.Id).ToList();
            if (dto.DueAmount <= 0)
            {
                dto.Status = "Paid";
            }
            else if (planInstallments.Any(i => i.Status == "Overdue" || (i.Status == "Due" && i.DueDate < DateOnly.FromDateTime(DateTime.UtcNow))))
            {
                dto.Status = "Overdue";
            }
            else
            {
                dto.Status = "Due";
            }
            
            return ApiResponse<FeePlanDto>.Ok(dto);
        }

        public async Task<ApiResponse<bool>> DeleteFeePlanAsync(Guid id)
        {
            var feePlan = await _unitOfWork.Repository<FeePlan>().GetByIdAsync(id);
            if (feePlan == null) return ApiResponse<bool>.Fail("Fee plan not found.");

            _unitOfWork.Repository<FeePlan>().Remove(feePlan);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Fee plan deleted successfully.");
        }
    }
}

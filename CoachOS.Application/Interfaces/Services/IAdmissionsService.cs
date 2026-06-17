using CoachOS.Application.Features.Admissions.Dtos;
using System;
using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IAdmissionsService
    {
        Task<Guid> QuickAdmissionAsync(QuickAdmissionRequest request);
        Task<Guid> FullAdmissionAsync(FullAdmissionRequest request);
        
        Task<object> GetStudentProfileAsync(Guid studentId);
        Task<object> GetStudentBatchHistoryAsync(Guid studentId);
        Task<object> GetStudentFeeHistoryAsync(Guid studentId);
        
        Task TransferBatchAsync(Guid studentId, Guid newBatchId);
    }
}

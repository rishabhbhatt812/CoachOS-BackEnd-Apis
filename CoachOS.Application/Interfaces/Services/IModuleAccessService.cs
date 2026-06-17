using System;
using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IModuleAccessService
    {
        Task<bool> IsModuleEnabledAsync(Guid instituteId, string moduleCode);
    }
}

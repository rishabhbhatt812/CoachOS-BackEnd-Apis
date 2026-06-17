using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<object> GetDashboardMetricsAsync();
        Task<object> GetGlobalMetricsAsync();
    }
}

using CoachOS.Domain.Audit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IAuditLogService
    {
        Task LogAsync(string entityName, string entityId, string actionType, string? oldValue = null, string? newValue = null);
        Task<List<AuditLog>> GetLogsAsync(string? entityName = null, Guid? userId = null, int page = 1, int pageSize = 50);
    }
}

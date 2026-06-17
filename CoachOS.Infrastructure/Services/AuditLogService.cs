using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Audit;
using CoachOS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoachOS.Infrastructure.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly AppDbContext _db;
        private readonly ICurrentUserService _currentUser;

        public AuditLogService(AppDbContext db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task LogAsync(string entityName, string entityId, string actionType, string? oldValue = null, string? newValue = null)
        {
            var log = new AuditLog
            {
                UserId = _currentUser.UserId,
                InstituteId = _currentUser.InstituteId,
                EntityName = entityName,
                EntityId = entityId,
                ActionType = actionType,
                OldValue = oldValue,
                NewValue = newValue
            };
            _db.AuditLogs.Add(log);
            await _db.SaveChangesAsync();
        }

        public async Task<List<AuditLog>> GetLogsAsync(string? entityName = null, Guid? userId = null, int page = 1, int pageSize = 50)
        {
            var query = _db.AuditLogs.AsQueryable();

            if (!string.IsNullOrEmpty(entityName))
                query = query.Where(a => a.EntityName == entityName);

            if (userId.HasValue)
                query = query.Where(a => a.UserId == userId);

            return await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}

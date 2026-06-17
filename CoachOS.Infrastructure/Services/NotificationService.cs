using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Communication;
using CoachOS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _db;
        private readonly ICurrentUserService _currentUser;

        public NotificationService(AppDbContext db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<List<Notification>> GetMyNotificationsAsync(int count = 20)
        {
            return await _db.Notifications
                .Where(n => n.UserId == _currentUser.UserId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync()
        {
            return await _db.Notifications
                .CountAsync(n => n.UserId == _currentUser.UserId && !n.IsRead);
        }

        public async Task MarkAsReadAsync(Guid notificationId)
        {
            var notif = await _db.Notifications.FindAsync(notificationId);
            if (notif != null && notif.UserId == _currentUser.UserId)
            {
                notif.IsRead = true;
                notif.ReadAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync()
        {
            var unread = await _db.Notifications
                .Where(n => n.UserId == _currentUser.UserId && !n.IsRead)
                .ToListAsync();

            foreach (var n in unread)
            {
                n.IsRead = true;
                n.ReadAt = DateTime.UtcNow;
            }
            await _db.SaveChangesAsync();
        }

        public async Task CreateNotificationAsync(Guid userId, Guid instituteId, string title, string message, string type, string? linkUrl = null)
        {
            var notif = new Notification
            {
                UserId = userId,
                InstituteId = instituteId,
                Title = title,
                Message = message,
                NotificationType = type,
                LinkUrl = linkUrl
            };
            _db.Notifications.Add(notif);
            await _db.SaveChangesAsync();
        }
    }
}

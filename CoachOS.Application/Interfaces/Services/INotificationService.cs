using CoachOS.Domain.Communication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoachOS.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<List<Notification>> GetMyNotificationsAsync(int count = 20);
        Task<int> GetUnreadCountAsync();
        Task MarkAsReadAsync(Guid notificationId);
        Task MarkAllAsReadAsync();
        Task CreateNotificationAsync(Guid userId, Guid instituteId, string title, string message, string type, string? linkUrl = null);
    }
}

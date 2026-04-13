using GoodsApi.Infrastructure.Models;
using GoodsApi.Infrastructure.Models.Database;
using GoodsApi.Infrastructure.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace GoodsApi.Infrastructure.Services;

public class NotificationService(DataComponent component) : INotificationService
{
    public async Task<List<Models.DTO.Notification>> GetUserNotifications(int userId)
    {
        return await component.Notifications
            .Where(n => n.UserId == userId)
            .Select(n => new Models.DTO.Notification()
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                NotificationType = (NotificationType)n.NotificationType,
                IsRead = n.IsRead,
            })
            .ToListAsync();
    }
}
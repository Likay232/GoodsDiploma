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
            .OrderBy(n => n.IsRead)
            .ThenByDescending(n => n.CreatedAt)
            .Select(n => new Models.DTO.Notification()
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                NotificationType = (NotificationType)n.NotificationType,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,
            })
            .ToListAsync();
    }

    public async Task ReadNotification(int id)
    {
        var notification = await component.Notifications.FirstOrDefaultAsync(n => n.Id == id);

        if (notification is null) return;

        notification.IsRead = true;
        await component.Update(notification);
    }

    public async Task ReadAllNotifications(int userId)
    {
        var notifications = component.Notifications.Where(n => n.UserId == userId);

        await notifications
            .ExecuteUpdateAsync(x => x
                .SetProperty(n => n.IsRead, true));
    }

    public async Task CreateAdminLowRemainsNotifications(string article)
    {
        var adminIds = await component.UserRoles
            .Include(ur => ur.Role)
            .Where(ur => ur.Role != null && ur.Role.Name == "Admin")
            .Select(ur => ur.UserId)
            .ToListAsync();

        if (adminIds.Count == 0) return;

        var newNotifications = adminIds.Select(adminId => new Models.Storage.Notification()
        {
            Title = "Низкий остаток товара",
            Message = $"Остаток для товара с артикулом {article} меньше минимума",
            NotificationType = (int)NotificationType.Warning,
            UserId = adminId
        }).ToList();

        await component.BulkInsertAsync(newNotifications);
    }
}
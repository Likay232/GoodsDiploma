using GoodsApi.Infrastructure.Models.Enums;

namespace GoodsApi.Infrastructure.Models;

public interface INotificationService
{
    public Task<List<DTO.Notification>> GetUserNotifications(int userId);
    public Task ReadNotification(int id);
    public Task ReadAllNotifications(int userId);
    public Task CreateAdminLowRemainsNotifications(string article);
}
namespace GoodsApi.Infrastructure.Models;

public interface INotificationService
{
    public Task<List<DTO.Notification>> GetUserNotifications(int userId);
}
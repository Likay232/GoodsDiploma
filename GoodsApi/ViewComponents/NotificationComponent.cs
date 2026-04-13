using GoodsApi.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoodsApi.ViewComponents;

public class NotificationComponent(INotificationService notificationService) : ViewComponent
{
    private INotificationService _notificationService = notificationService;
    
    public async Task<IViewComponentResult> InvokeAsync(int userId)
    {
        var users = await _notificationService.GetUserNotifications(userId);
        return View(users);
    }
}
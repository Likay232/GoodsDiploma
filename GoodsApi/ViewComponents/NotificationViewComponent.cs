using GoodsApi.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoodsApi.ViewComponents;

public class NotificationViewComponent(INotificationService notificationService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(int userId)
    {
        var users = await notificationService.GetUserNotifications(userId);
        return View(users);
    }
}
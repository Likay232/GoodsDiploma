using System.Security.Claims;
using GoodsApi.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodsApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class NotificationsController(INotificationService notificationService) : ControllerBase
{   
    [HttpPost]
    public async Task<IActionResult> MarkAsRead([FromBody] int id)
    {
        await notificationService.ReadNotification(id);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var result) ? result : 0;
        await notificationService.ReadAllNotifications(userId);
        return Ok();
    }
}
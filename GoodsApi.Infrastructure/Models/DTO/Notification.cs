using GoodsApi.Infrastructure.Models.Enums;

namespace GoodsApi.Infrastructure.Models.DTO;

public class Notification
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType NotificationType { get; set; }
    
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }

}
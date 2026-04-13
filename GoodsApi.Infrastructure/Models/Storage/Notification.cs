namespace GoodsApi.Infrastructure.Models.Storage;

public class Notification : BaseEntity
{
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int NotificationType { get; set; }
    
    public bool IsRead { get; set; }
}
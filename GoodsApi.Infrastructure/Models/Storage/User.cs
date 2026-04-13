namespace GoodsApi.Infrastructure.Models.Storage;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
    public bool IsBlocked { get; set; }
}
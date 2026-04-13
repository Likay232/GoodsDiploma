namespace GoodsApi.Infrastructure.Models.Requests;

public class Login
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
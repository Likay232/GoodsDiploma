using GoodsApi.Infrastructure.Models.Enums;

namespace GoodsApi.Infrastructure.Models.Requests;

public class Register
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public AvailableRoles ChosenRole { get; set; } = new ();
}
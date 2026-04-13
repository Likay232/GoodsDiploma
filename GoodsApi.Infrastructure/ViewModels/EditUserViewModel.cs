namespace GoodsApi.Infrastructure.ViewModels;

public class UserEditViewModel
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public bool IsBlocked { get; set; }

    public int RoleId { get; set; }

    public DateTime RegisteredAt { get; set; }
    
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;

    public List<RoleItemViewModel> Roles { get; set; } = new();
}

public class RoleItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
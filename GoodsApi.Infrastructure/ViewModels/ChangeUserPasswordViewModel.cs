namespace GoodsApi.Infrastructure.ViewModels;

public class ChangeUserPasswordViewModel
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;

    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;

}
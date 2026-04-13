using Microsoft.AspNetCore.Identity;

namespace GoodsApi.Infrastructure.Services;

public static class PasswordService
{
    private static readonly PasswordHasher<object> _hasher = new();

    public static string HashPassword(string password)
    {
        return _hasher.HashPassword(null!, password);
    }

    public static bool VerifyPassword(string hashedPassword, string password)
    {
        var result = _hasher.VerifyHashedPassword(null!, hashedPassword, password);

        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}
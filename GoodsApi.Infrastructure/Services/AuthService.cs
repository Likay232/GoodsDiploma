using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GoodsApi.Infrastructure.Models.Database;
using GoodsApi.Infrastructure.Models.Requests;
using GoodsApi.Infrastructure.Models.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GoodsApi.Infrastructure.Services;

public class AuthService(DataComponent component)
{
    private string GetJwtKey()
    {
        return Environment.GetEnvironmentVariable("JWT_KEY")
               ?? "super_secret_key_12345";
    }

    private string GenerateAccessToken(int userId, string username, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(GetJwtKey());

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.Role, role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public async Task<string?> Login(Login request)
    {
        var user = await component.UserRoles
            .Include(u => u.Role)
            .Include(ur => ur.User)
            .FirstOrDefaultAsync(u => u.User!.Username == request.Username);
        
        if (user == null) return null;
        if (!PasswordService.VerifyPassword(user.User!.Password, request.Password)) return null;
        if (user.Role is null) return null;
        
        return GenerateAccessToken(user.UserId, request.Username, user.Role.Name); 
    }

    public async Task<bool> Register(Register request)
    {
        var user = await component.Users.FirstOrDefaultAsync(u =>
            u.Username == request.Username);
        
        var role = await component.Roles.FirstOrDefaultAsync(r => r.Name == request.ChosenRole.ToString());

        if (user != null) throw new Exception("Имя пользователя занято.");
        if (role == null) throw new Exception("Роль не найдена");
        
        var newUser = new User
        {
            Username = request.Username,
            Password = PasswordService.HashPassword(request.Password),
            Email =  request.Email,
            RegisteredAt = DateTime.UtcNow,
        };

        await component.Insert(newUser);

        var newUserRole = new UserRole()
        {
            UserId = newUser.Id,
            RoleId = (int)request.ChosenRole
        };
        
        return await component.Insert(newUserRole);
    }

}
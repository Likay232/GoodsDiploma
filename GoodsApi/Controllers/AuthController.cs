using System.IdentityModel.Tokens.Jwt;
using GoodsApi.Infrastructure.Models.Requests;
using GoodsApi.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodsApi.Controllers;

[Route("[controller]/[action]")]
public class AuthController(AuthService service) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Register(Register request)
    {
        try
        {
            return StatusCode(200, await  service.Register(request));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete("AuthToken");
        
        return RedirectToAction("Login", "Auth");
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View(new Login());
    }
    
    [HttpPost]
    public async Task<IActionResult> Login([FromForm] Login request)
    {
        try
        {
            var token = await service.Login(request);

            if (string.IsNullOrEmpty(token))
            {
                return View(request);
            }

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var role = jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(3)
            };

            Response.Cookies.Append("AuthToken", token, cookieOptions);

            return role switch
            {
                "Admin" => RedirectToAction("Users", "Admin"),
                "Manager" => RedirectToAction("Products", "Catalog"),
                "Storekeeper" => RedirectToAction("Products", "Catalog"),
                _ => View(request)
            };
        }
        catch
        {
            return View(request);
        }
    }
}
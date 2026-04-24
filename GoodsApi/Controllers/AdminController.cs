using GoodsApi.Infrastructure.Services;
using GoodsApi.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodsApi.Controllers;

[Authorize(Roles = "Admin")]
[Route("[controller]/[action]")]
public class AdminController(AdminService adminService) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Users));
    }

    [HttpGet]
    public async Task<IActionResult> Users()
    {
        return View(await adminService.GetUsers());
    }

    [HttpGet]
    public async Task<IActionResult> UserEditPage(int id = 0)
    {
        var model = await adminService.GetUserViewModel(id);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await adminService.DeleteUser(userId);
        return RedirectToAction(nameof(Users));
    }

    [HttpPost]
    public async Task<IActionResult> SaveUser(UserEditViewModel viewModel)
    {
        if (!ModelState.IsValid || viewModel.Password != viewModel.ConfirmPassword)
        {
            var roles = await adminService.GetRoles();
            viewModel.Roles = roles.Select(r => new RoleItemViewModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();

            return View("UserEditPage", viewModel);
        }

        await adminService.SaveUser(viewModel);

        return RedirectToAction(nameof(Users));
    }

    [HttpGet]
    public async Task<IActionResult> ChangeUserPasswordPage(int id)
    {
        var user = await adminService.GetUser(id);

        if (user == null) return NotFound();

        return View(new ChangeUserPasswordViewModel
        {
            UserId = user.Id,
            Username = user.Username
        });
    }

    [HttpPost]
    public async Task<IActionResult> ChangeUserPassword(ChangeUserPasswordViewModel model)
    {
        if (model.NewPassword != model.ConfirmPassword)
        {
            ModelState.AddModelError("", "Пароли не совпадают.");
            return View("ChangeUserPasswordPage", model);
        }

        await adminService.ChangeUserPassword(model.NewPassword, model.UserId);

        return RedirectToAction(nameof(Users));
    }
}
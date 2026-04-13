using GoodsApi.Infrastructure.Models.Database;
using GoodsApi.Infrastructure.Models.Storage;
using GoodsApi.Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GoodsApi.Infrastructure.Services;

public class AdminService(DataComponent component)
{
    public async Task<List<Models.DTO.User>> GetUsers()
    {
        return await component.UserRoles
            .Include(ur => ur.Role)
            .Include(ur => ur.User)
            .Select(u => new Models.DTO.User
            {
                Id = u.UserId,
                Username = u.User!.Username,
                Email = u.User.Email,
                RegisteredAt = u.User.RegisteredAt,
                RoleId = u.Role!.Id,
                RoleName = u.Role.Name,
                IsBlocked = u.User.IsBlocked,
            })
            .ToListAsync();
    }

    public async Task<Models.DTO.User?> GetUser(int userId)
    {
        var userRole = await component.UserRoles
            .Include(ur => ur.User)
            .Include(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (userRole == null) throw new Exception("Пользователь не найден");

        return new Models.DTO.User()
        {
            Id = userRole.UserId,
            Username = userRole.User!.Username,
            Email = userRole.User.Email,
            RegisteredAt = userRole.User.RegisteredAt,
            RoleId = userRole.Role!.Id,
            RoleName = userRole.Role.Name,
            IsBlocked = userRole.User.IsBlocked,
        };
    }

    public async Task<UserEditViewModel> GetUserViewModel(int userId)
    {
        var roles = (await GetRoles())
            .Convert<Models.DTO.Role, RoleItemViewModel>();

        var viewModel = new UserEditViewModel
        {
            Roles = roles
        };

        if (userId == 0) return viewModel;

        var user = await GetUser(userId);
        
        if (user == null) return viewModel;

        viewModel = user.Convert<Models.DTO.User, UserEditViewModel>();
        viewModel.Roles = roles;
        
        return viewModel;
    }

    public async Task<List<Models.DTO.Role>> GetRoles()
    {
        return component.Roles
            .Select(r => new  Models.DTO.Role()
            {
                Id = r.Id,
                Name = r.Name
            })
            .ToList();
    }

    public async Task<bool> ChangeUserPassword(string newPassword, int userId)
    {
        var userEntry = await component.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (userEntry == null)
            throw new Exception("Пользователь с заданным Id не найден");

        userEntry.Password = PasswordService.HashPassword(newPassword);

        return await component.Update(userEntry);
    }

    public async Task<bool> SaveUser(UserEditViewModel viewModel)
    {
        if (viewModel.Id == 0) return await AddUser(viewModel);
        
        return await ChangeUser(viewModel);
    }

    private async Task<bool> AddUser(UserEditViewModel viewModel)
    {
        var convertedUser = viewModel
            .Convert<UserEditViewModel, Models.DTO.User>()
            .Convert<Models.DTO.User, User>();
        
        convertedUser.Password = PasswordService.HashPassword(viewModel.Password);
        convertedUser.RegisteredAt = DateTime.UtcNow;
        
        if (!await component.Insert(convertedUser)) return false;

        var userRole = new UserRole
        {
            UserId = convertedUser.Id,
            RoleId = viewModel.RoleId
        };

        return await component.Insert(userRole);
    }

    private async Task<bool> ChangeUser(UserEditViewModel viewModel)
    {
        var changedUser = viewModel.Convert<UserEditViewModel, Models.DTO.User>();
        
        var userEntry = await component.Users.FirstOrDefaultAsync(u => u.Id == changedUser.Id);
        var userRoleEntry = await component.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == changedUser.Id);
        
        if (userEntry == null || userRoleEntry == null) throw new Exception("Пользователь не найден");

        userEntry.Username = changedUser.Username;
        userEntry.Email = changedUser.Email;

        userRoleEntry.RoleId = changedUser.RoleId;
        
        return await component.Update(userEntry) && await component.Update(userRoleEntry);
    }

    public async Task<bool> DeleteUser(int userId)
    {
        return await component.DeleteUser(userId);
    }
}
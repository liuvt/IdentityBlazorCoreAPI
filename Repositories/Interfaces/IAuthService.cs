using IdentityBlazorCoreAPI.Data.Entities;
using IdentityBlazorCoreAPI.Data.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityBlazorCoreAPI.Repositories.Interfaces;

public interface IAuthService
{   
    //For user
    Task<AppUser> GetMe();
    Task<bool> DeleteMe();
    Task<bool> ChangePassword(AppChangePasswordDTO changePassword);
    Task<bool> EditMe(AppEditDTO models);
    Task<bool> Register(AppRegisterDTO models);
    Task Login(AppLoginDTO models);
    Task LogOut();
    Task<bool> CheckAuthenState();
    Task<AuthenticationState> GetAuthenState();
    //For Admin
}
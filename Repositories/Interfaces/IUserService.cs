using IdentityBlazorCoreAPI.Data.Entities;
using IdentityBlazorCoreAPI.Data.Models;

namespace IdentityBlazorCoreAPI.Repositories.Interfaces;

public interface IUserService
{   
    Task<List<AppUser>> Gets();
    Task<bool> Create(UserCreateDTO models);
    Task<bool> Delete(string userId);
    Task<bool> DeleteSelect(IEnumerable<string> userIds);
}
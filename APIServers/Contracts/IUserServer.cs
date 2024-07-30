using IdentityBlazorCoreAPI.Data.Entities;
using IdentityBlazorCoreAPI.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityBlazorCoreAPI.APIServers.Contracts;

public interface IUserServer
{
    /* Manager Gets
        Xem toàn bộ thông tin ds user
    */
    Task<List<AppUser>> Gets();
    
    /* Manager Create
        Tạo tài khoản cho user
    */
    Task<IdentityResult> Create(UserCreateDTO userCreateDTO);

    /* Manager Delete
        Xóa user theo ID
    */
    Task<IdentityResult> Delete(string userId);


    /* Manager DeleteSelect
        Xóa user theo ds ID
    */
    Task<bool> DeleteSelect(IEnumerable<string> userIds);
}
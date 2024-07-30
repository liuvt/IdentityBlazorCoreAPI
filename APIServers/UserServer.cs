using IdentityBlazorCoreAPI.APIServers.Contracts;
using IdentityBlazorCoreAPI.Data;
using IdentityBlazorCoreAPI.Data.Entities;
using IdentityBlazorCoreAPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityBlazorCoreAPI.APIServers;

public class UserServer : IUserServer
{

    //User Manager
    protected readonly UserManager<AppUser> userManager;
    protected readonly IConfiguration configuration;

    //Constructor
    public UserServer(UserManager<AppUser> _userManager, IConfiguration _configuration)
    {
        this.userManager = _userManager;
        this.configuration = _configuration;
    }

    /* Manager Gets
        Xem toàn bộ thông tin ds user
    */
    public async Task<List<AppUser>> Gets()
        => await userManager.Users.OrderByDescending(p => p.PublishedAt).ToListAsync();

    /* Manager Create
        Tạo tài khoản cho user
    */
    public async Task<IdentityResult> Create(UserCreateDTO userCreateDTO)
    {
        try
        {
            // Kiểm tra email
            var user = await this.userManager.FindByEmailAsync(userCreateDTO.Email);
            if (user != null)
                throw new Exception("Email đã tồn tại, vui lòng chọn email khác để đăng ký!");

            var newUser = new AppUser
            {
                Email = userCreateDTO.Email,
                PhoneNumber = userCreateDTO.PhoneNumber,
                Biography = userCreateDTO.Biography,
                FirstName = userCreateDTO.FirstName,
                LastName = userCreateDTO.LastName,
                UserName = userCreateDTO.Email,
                Gender = userCreateDTO.Gender,
                Address = userCreateDTO.Address,
                PublishedAt = DateTime.Now
            };

            // Create password
            var create = await userManager.CreateAsync(newUser, userCreateDTO.Password);

            // Kiểm tra trạng thái đăng ký thành công
            if (!create.Succeeded)
                throw new Exception("Không thành công vui lòng làm lại!");
            else
            {
                #region Set role USER mặt định cho người dùng nếu đăng ký thành công
                // Khởi tạo IEnumerable<string> roles của hàm AddToRolesAsync
                List<UserRoles> roles = new List<UserRoles>();
                // Mặt định khi tạo mới tài khoản là quyền User
                var userRoles = new UserRoles() { RoleId = "5", RoleName = "Buyer", IsSelected = true };
                roles.Add(userRoles);

                // Đăng ký role cho user vừa được tạo trên bảng aspnetuserroles [Database]
                await userManager.AddToRolesAsync(newUser,
                                roles.Where(x => x.IsSelected).Select(y => y.RoleName));
                #endregion

                // Đăng ký thành công trả về thông tin IdentityResult
                return create;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error system {Create method}: " + ex.Message);
        }
    }

    /* Manager Delete
        Xóa user theo ID
    */
    public async Task<IdentityResult> Delete(string userId)
    {
        try
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("Người dùng không tồn tại!");

            var result = await this.userManager.DeleteAsync(user);
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("Error system {Delete method}: " + ex.Message);
        }
    }

    /* Manager DeleteSelect
        Xóa user theo ds ID
    */
    public async Task<bool> DeleteSelect(IEnumerable<string> userIds)
    {
        try
        {
            // Tìm kiếm ds users
            var users = this.userManager.Users
                                        .Where(u => userIds.Contains(u.Id)).ToList();

            if (users.Count() <= 0 ) return false;

            //  Note: do không có hàm RemoveRange nên sử dụng vòng lập để xóa từng user
            foreach(var item in users)
            {
                var result = await this.userManager.DeleteAsync(item);
            }
            //Console.WriteLine($"{context.ChangeTracker.DebugView.LongView}");
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Error system {DeleteSelect method}: " + ex.Message);
        }
    }
}
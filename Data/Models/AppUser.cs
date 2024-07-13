using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace IdentityBlazorCoreAPI.Data.Models;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime? BirthDay { get; set; }
}

//Register: set default role for new User
public partial class UserRoles
{
    public string RoleId { get; set; } = "5";
    public string RoleName { get; set; } = "Buyer";
    public bool IsSelected { get; set; } = true;
}

//Login: create token 
public partial class InfomationUserSaveInToken
{
    public string userId { get; set; } = string.Empty;
    public string userEmail { get; set; } = string.Empty;
    public string userName { get; set; } = string.Empty;
    public string userRole { get; set; } = string.Empty;
    public string userGuiId { get; set; } = string.Empty;
}


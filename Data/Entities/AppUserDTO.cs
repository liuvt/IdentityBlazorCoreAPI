
using System.ComponentModel.DataAnnotations;

namespace IdentityBlazorCoreAPI.Data.Entities;
public class AppUserDTO
{
    
}

//Login: Using Regular Expression
public class AppLoginDTO
{
    [Required(ErrorMessage = "Email không được bỏ trống.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Mật khẩu không được bỏ trống.")]
    public string Password { get; set; } = string.Empty;
}

//Register
public partial class AppRegisterDTO
{
    [Required(ErrorMessage = "Email không được bỏ trống.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    //Hổ trợ: gmail|homail|yahoo|viettel|outlook|skyper
    [RegularExpression(@"([a-zA-Z0-9_.-]+)@(gmail|homail|yahoo|viettel|outlook|skyper).([a-zA-Z]{2,4}|[0-9]{1,3})?.([a-zA-Z]{2,4}|[0-9]{1,3})"
        , ErrorMessage = "Email hổ trợ: gmail, outlook, homail, yahoo, viettel, skyper.")]
    public string Email { get; set; } = string.Empty;

    
    [Required(ErrorMessage = "Họ không được bỏ trống.")]
    [StringLength(30, ErrorMessage = "Nhập tối thiểu 3 ký tự.", MinimumLength = 3)]
    public string FirstName { get; set; } = string.Empty;

    
    [Required(ErrorMessage = "Tên không được bỏ trống.")]
    [StringLength(30, ErrorMessage = "Nhập tối thiểu 3 ký tự.", MinimumLength = 3)]
    public string LastName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Giới tính không được bỏ trống.")]
    public string Gender { get; set; } = string.Empty;

    
    [Required(ErrorMessage = "Mật khẩu không được bỏ trống.")]
    //Mật khẩu yêu cầu: 8-15 ký tự, 1 ký tự đặt biệt (!,#,$,%,..), 1 ký tự viết hoa, 1 chữ số. Ví dụ: Abc!1234
    [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\\]*+\\/|!\"£$%^&*()#[@~'?><,.=_-]).{8,15}$"
        , ErrorMessage = "Mật khẩu yêu cầu: 8-15 ký tự, 1 ký tự đặt biệt (!,#,$,%,..), 1 ký tự viết hoa, 1 chữ số. Ví dụ: Abc!1234.")]
    public string Password { get; set; } = string.Empty;

    
    [Compare(nameof(Password), ErrorMessage = "Nhập lại mật khẩu không khớp.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

//Update
public partial class AppEditDTO
{
    [Required(ErrorMessage = "Họ không được bỏ trống.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tên không được bỏ trống.")]
    public string LastName { get; set; } = string.Empty;
   
    [Required(ErrorMessage = "Địa chỉ không được bỏ trống.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Giới tính không được bỏ trống.")]
    public string Gender { get; set; } = string.Empty;
    public DateTime? BirthDay { get; set; }
    
    [Required(ErrorMessage = "Số điện thoại không được bỏ trống.")]
    [RegularExpression(@"((84|60|86|02|01|0)[1-9]{1})+(([0-9]{8})|([0-9]{9})|([0-9]{10}))", 
                                                    ErrorMessage = "Số điện thoại không hợp lệ.")]
    public string PhoneNumber { get; set; } = string.Empty;
}

//Change password
public partial class AppChangePasswordDTO
{
    [Required(ErrorMessage = "Không được bỏ trống.")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mật khẩu không được bỏ trống.")]
    //Mật khẩu yêu cầu: 8-15 ký tự, 1 ký tự đặt biệt (!,#,$,%,..), 1 ký tự viết hoa, 1 chữ số. Ví dụ: Abc!1234
    [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\\]*+\\/|!\"£$%^&*()#[@~'?><,.=_-]).{8,15}$"
        , ErrorMessage = "Mật khẩu yêu cầu: 8-15 ký tự, 1 ký tự đặt biệt (!,#,$,%,..), 1 ký tự viết hoa, 1 chữ số. Ví dụ: Abc!1234.")]
    public string Password { get; set; } = string.Empty;

    [Compare(nameof(Password), ErrorMessage = "Nhập lại mật khẩu không khớp.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
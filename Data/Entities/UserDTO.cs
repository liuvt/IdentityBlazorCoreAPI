
using System.ComponentModel.DataAnnotations;

namespace IdentityBlazorCoreAPI.Data.Entities;

/* Manager Create
    Tạo tài khoản cho user
*/
public partial class UserCreateDTO
{
    [Required(ErrorMessage = "Email không được bỏ trống.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    //Hổ trợ: gmail|homail|yahoo|viettel|outlook|skyper
    [RegularExpression(@"([a-zA-Z0-9_.-]+)@(gmail|homail|yahoo|viettel|outlook|skyper).([a-zA-Z]{2,4}|[0-9]{1,3})?.([a-zA-Z]{2,4}|[0-9]{1,3})"
        , ErrorMessage = "Email hổ trợ: gmail, outlook, homail, yahoo, viettel, skyper.")]
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    [Required(ErrorMessage = "Mật khẩu không được bỏ trống.")]
    //Mật khẩu yêu cầu: 8-15 ký tự, 1 ký tự đặt biệt (!,#,$,%,..), 1 ký tự viết hoa, 1 chữ số. Ví dụ: Abc!1234
    [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\\]*+\\/|!\"£$%^&*()#[@~'?><,.=_-]).{8,15}$"
        , ErrorMessage = "Mật khẩu yêu cầu: 8-15 ký tự, 1 ký tự đặt biệt (!,#,$,%,..), 1 ký tự viết hoa, 1 chữ số. Ví dụ: Abc!1234.")]
    public string Password { get; set; } = string.Empty;
}
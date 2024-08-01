using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace IdentityBlazorCoreAPI.Data.Models;

public class EtherealEmail
{
    [Required(ErrorMessage = "Email không được bỏ trống.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    //Hổ trợ: gmail|homail|yahoo|viettel|outlook|skyper
    [RegularExpression(@"([a-zA-Z0-9_.-]+)@(gmail|homail|yahoo|viettel|outlook|skyper).([a-zA-Z]{2,4}|[0-9]{1,3})?.([a-zA-Z]{2,4}|[0-9]{1,3})"
        , ErrorMessage = "Email hổ trợ: gmail, outlook, homail, yahoo, viettel, skyper.")]
    public string From { get; set; } = string.Empty;
    [Required(ErrorMessage = "Số điện thoại không được bỏ trống.")]
    [RegularExpression(@"((84|60|86|02|01|0)[1-9]{1})+(([0-9]{8})|([0-9]{9})|([0-9]{10}))", 
                                                    ErrorMessage = "Số điện thoại không hợp lệ.")]
    public string YourPhone { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}

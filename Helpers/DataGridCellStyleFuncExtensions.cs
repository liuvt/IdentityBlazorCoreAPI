
using IdentityBlazorCoreAPI.Data.Models;

namespace IdentityBlazorCoreAPI.Helpers;

//Thay đổi hiển thị trên Data Grid Cell Style
public static class DataGridCellStyleFuncExtensions
{
    //In đậm, chữ đỏ
    public static string color_red_bold = ";color:#F32432;font-weight:bold";
    //In đậm
    public static string bold = ";font-weight:bold";
    //In đậm, chữ vàng nâu đen
    public static string color_backgold_bold = ";color:#6F701C;font-weight:bold";

    public static Func<AppUser, string> ForAppUser(string format) => x => format;
}
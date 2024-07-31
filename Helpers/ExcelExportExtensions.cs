using Microsoft.JSInterop;
using IdentityBlazorCoreAPI.Data.Models;
namespace IdentityBlazorCoreAPI.Helpers;

// Class dùng để gọi Javascript Download file và lấy dữ liệu đầu ra từ ExcelConvertDataExtensions
// https://github.com/tossnet/Blazor-Excel-export/blob/master/Blazor-Excel-export/XLS/UseTemplateXLS.cs
public class ExcelExportExtensions
{
    public async Task DefaultExport(IJSRuntime Js, List<AppUser> users)
    {
        var excelConvertData = new ExcelConvertDataExtensions();
        var XLSStream = excelConvertData.Excel_Users(users);
        var fileName = "ExportFile.xlsx";
        await Js.InvokeVoidAsync("downloadFileFromStream", fileName, XLSStream);
    }
}
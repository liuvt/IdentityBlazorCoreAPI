using ClosedXML.Excel;
using IdentityBlazorCoreAPI.Data.Models;
namespace IdentityBlazorCoreAPI.Helpers;

// Class dùng để convert dữ liệu đầu vào và đẩy ra dữ liệu excel
// https://github.com/tossnet/Blazor-Excel-export/blob/master/Blazor-Excel-export/XLS/UseTemplateXLS.cs
public class ExcelConvertDataExtensions
{
    public byte[] Excel_Users(List<AppUser> users)
    {
        // Tạo file excel
        var xlsxWorkbook = new XLWorkbook();
        // Thông tin file
        xlsxWorkbook.Author = "Users";
        // Tạo sheet trong file excel
        var xlsxSheet = xlsxWorkbook.AddWorksheet("Users_sheet");

        // Tạo dữ liệu dòng dầu của sheet trong file excel: Tiêu đề
        xlsxSheet.Cell(1, 1).Value = "Id";
        xlsxSheet.Cell(1, 2).Value = "Email";
        xlsxSheet.Cell(1, 3).Value = "FirstName";
        xlsxSheet.Cell(1, 4).Value = "LastName";
        xlsxSheet.Cell(1, 5).Value = "Biography";
        xlsxSheet.Cell(1, 6).Value = "Gender";
        xlsxSheet.Cell(1, 7).Value = "BirthDay";
        xlsxSheet.Cell(1, 8).Value = "PhoneNumber";
        xlsxSheet.Cell(1, 9).Value = "Address";
        xlsxSheet.Cell(1, 10).Value = "PublishedAt";

        // Định dạng dữ liệu dòng đầu của sheet trong file excel: Tiêu đề
        xlsxSheet.FirstRow().Style.Font.Bold = true;
        xlsxSheet.FirstRow().Style.Font.FontColor = XLColor.Red;

        // Đếm tổng số dòng dữ liệu xuất file
        int total = users.Count();
        // Chạy vòng lập từ cho đến hết tổng số dữ liệu xuất file và gán vào các dòng trong sheet
        for (int row = 1; row <= total; row++)
        {

            xlsxSheet.Cell(row + 1, 1).Value = users[total - row].Id;
            xlsxSheet.Cell(row + 1, 2).Value = users[total - row].Email;
            xlsxSheet.Cell(row + 1, 3).Value = users[total - row].FirstName;
            xlsxSheet.Cell(row + 1, 4).Value = users[total - row].LastName;
            xlsxSheet.Cell(row + 1, 5).Value = users[total - row].Biography;
            xlsxSheet.Cell(row + 1, 6).Value = users[total - row].Gender;
            xlsxSheet.Cell(row + 1, 7).Value = users[total - row].BirthDay;
            xlsxSheet.Cell(row + 1, 8).Value = users[total - row].PhoneNumber;
            xlsxSheet.Cell(row + 1, 9).Value = users[total - row].Address;
            xlsxSheet.Cell(row + 1, 10).Value = users[total - row].PublishedAt;
        };

        //var _ifLiveIsTrueExcel = "=IF(H2=TRUE;1;2)";
        //var _conCateNate = "=CONCATENATE(\"(\";A2;\",\"\"\";B2;\"\"\",\"\"\";C2;\"\"\",\"\"\";D2;\"\"\",\"\"\";E2;\"\"\",\"\"\";F2;\"\"\",\"\"\";G2;\"\"\",\";K2;\",\"\"\";I2;\"\"\",\";J2;\"),\")";
        //xlsxSheet.Cell(2, 11).Value = _ifLiveIsTrueExcel;
        //xlsxSheet.Cell(2, 12).Value = _conCateNate;

        // Lưu lại dữ liệu
        MemoryStream XLSStream = new();
        xlsxWorkbook.SaveAs(XLSStream);

        // Thay đổi thuộc tính
        XLSStream.Position = 0;

        // Trả về một byte[] để có thể down tệp xuống
        return XLSStream.ToArray();
    }
}
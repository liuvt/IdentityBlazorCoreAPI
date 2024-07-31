using IdentityBlazorCoreAPI.Data.Models;
using IdentityBlazorCoreAPI.Repositories.Interfaces;
using IdentityBlazorCoreAPI.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using IdentityBlazorCoreAPI.Owners.Pages;
using IdentityBlazorCoreAPI.Data.Entities;

namespace IdentityBlazorCoreAPI.Owners.Bases;
public class ManagerUsersBase : ComponentBase
{
    [Inject] private IUserService userService { get; set; }

    [Inject] private ISnackbar snackBar { get; set; } //Thông báo
    [Inject] protected IJSRuntime Js { get; set; } //Thông báo

    protected IEnumerable<AppUser> appUsers { get; set; } = new List<AppUser>();
    protected AppUser appUser { get; set; } = new AppUser();
    protected bool _selectRow = true; // Row được chọn
    protected override async Task OnInitializedAsync()
    {
        await GetUsers();
    }

    #region Private method with back-end
    // Lấy ds users
    private async Task<IEnumerable<AppUser>> GetUsers() => appUsers = await userService.Gets();

    // Note: Xóa user sử dụng Button delete trên razor page nên đặt protected
    protected async Task DeleteUser(string userId) 
    {
        await userService.Delete(userId);
        // Reload lại bảng ds user
        await GetUsers();
        StateHasChanged();
    }

    // Note: Xóa ds user được chọn: sử dụng Button delete trên razor page nên đặt protected
    private IEnumerable<string> uIDs { get; set; } // Biến cục bộ class lưu trữ ID cần xóa
    protected async Task DeleteUsers()
    {
        await userService.DeleteSelect(uIDs);
        // Reload lại bảng ds user
        _selectRow = true;
        await GetUsers();
        StateHasChanged();
    }
    #endregion

    #region Import/Export excel
    //export
    protected async void ExportDataExcel()
    {
        ExcelExportExtensions excelExport = new ExcelExportExtensions();
        await excelExport.DefaultExport(Js, appUsers.ToList());

    }
    #endregion

    #region Tìm kiếm dữ liệu trên bảng dữ liệu
    // Thanh search tìm kiếm
    protected string _searchString;
    protected Func<AppUser, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        if (x.Email.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (x.UserName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (x.LastName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (x.FirstName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if ($"{x.PhoneNumber}".Contains(_searchString))
            return true;

        return false;
    };
    #endregion

    #region Nhóm dữ liệu theo email
    // Nhóm dữ liệu
    private bool _sortNameByLength;
    protected Func<AppUser, object> _sortBy => x =>
    {
        if (_sortNameByLength)
            return x.Email.Length;
        else
            return x.Email;
    };
    #endregion

    #region Chọn nhiều dòng: Xóa hoặc xuất dữ liệu đã chọn
    //Chọn nhiều
    protected void SelectedItemsChanged(HashSet<AppUser> items)
    {
        // Khởi tạo biến cục bộ method ds userIds
        var userIds = new List<string>();
        // Cho vòng lập chạy tất cả dữ liệu khi SelectedItems trên datagrid
        foreach (var item in items)
        {
            // Lưu toàn bộ giá trị hiện tại vào trong ds userIds
            userIds.Add(item.Id);
        }

        // Sau mổi lần thay đổi SelectedItems thì nhận được dữ liệu cuối là ds userIds. lưu vào biến uIDs
        uIDs = userIds;

        // Nếu có danh sách uIDs thì hiển thị nút Delete, nếu không thì ẩn
        _selectRow = (uIDs.Count() > 0) ? false : true;

        StateHasChanged();
    }
    #endregion

    #region Dialog
    [Inject] private IDialogService DialogService { get; set; }
    protected async Task CreateDialog()
    {
        bool isEdit = false; //Tham số
        var parameters = new DialogParameters { ["isEdit"] = isEdit };

        DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.ExtraExtraLarge, CloseOnEscapeKey = true };

        var dialog = await DialogService.ShowAsync<ManagerUsersDialog>("Tạo mới", parameters, options);
        var result = await dialog.Result;
        try
        {
            if (!result.Canceled)
            {
                var data = (UserCreateDTO)result.Data;

                var newUser = await this.userService.Create(data);
                if (newUser != null) snackBar.Add("Tạo mới thành công.", Severity.Success);
                else snackBar.Add("Tạo mới không thành công.", Severity.Error);
            }

            // Reload lại ds users
            await GetUsers();
            StateHasChanged();
        }
        catch (Exception ex)
        {

            snackBar.Add(ex.Message, Severity.Error);
        }

    }
    #endregion
}

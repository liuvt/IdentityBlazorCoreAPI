using Microsoft.AspNetCore.Components;
using IdentityBlazorCoreAPI.Repositories.Interfaces;
using IdentityBlazorCoreAPI.Data.Models;
using MudBlazor;
using Microsoft.AspNetCore.Components.Forms;
using IdentityBlazorCoreAPI.Data.Entities;

namespace IdentityBlazorCoreAPI.Pages.Bases;

public class AppMeBase : ComponentBase
{
    [Inject]
    private IAuthService authService { get; set; }
    [Inject]
    private ISnackbar snackBar { get; set; }
    [Inject]
    private NavigationManager nav { get; set; }
    //Text
    private string TEXT_EDIT = "Cập nhật";
    private string TEXT_UN_EDIT = "Hủy cập nhật";

    protected override async Task OnInitializedAsync()
    {
        try
        {
            //thay đổi tên text Edit
            nameEditControl = TEXT_EDIT;
            //Check authentication state
            if (!await authService.CheckAuthenState()) nav.NavigateTo("/", true);

            models = await GetMeHandler();
        }
        catch (Exception ex)
        {

        }
    }

    protected async void pageChangePassword() => nav.NavigateTo("/change-password", true);

    #region Dialog inject
    [Inject]
    private IDialogService dialogService { get; set; }

    //Xóa user
    protected async Task DeleteMe_Dialag()
    {
        var icon = new Icons.Material.Filled();
        var parameters = new DialogParameters<IdentityBlazorCoreAPI.Pages.ComponentDialogs.TemplatingDialog>
        {
            { x => x.TitleText, "XÓA TÀI KHOẢN" },
            { x => x.IconTittle, "<path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z\"/>" },
            { x => x.ContentText, $"Bạn có đồng ý thực hiện tài khoản {models.Email}? Sau khi xóa, mọi thông tin dữ liệu sẽ không thể phục hồi." },
            { x => x.ButtonText, "Delete" },
            { x => x.ColorButton, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await dialogService.ShowAsync<IdentityBlazorCoreAPI.Pages.ComponentDialogs.TemplatingDialog>("", parameters, options);
        var result = await dialog.Result;
        try
        {
            if (!result.Canceled)
            {  
                //Nội dung trả về
                var data = (bool)result.Data;

                //Lấy kết submit
                if (data)
                {
                    await DeleteMeHandler();
                    await authService.LogOut();
                    snackBar.Add($"Đã xóa thành công.", Severity.Success);
                    nav.NavigateTo(nav.Uri, true); //Refresh page
                }
            }
        }
        catch (Exception ex)
        {
            snackBar.Add("Xóa không thành công: " + ex.Message, Severity.Error);
        }
    }
    #endregion

    #region Private to handler with data

    //Cập nhật thông tin cơ bản của User
    private async Task EditHandler(AppEditDTO _models)
    {
        try
        {
            if (await this.authService.EditMe(_models))
            {
                snackBar.Add($"Đã cập nhật thành công.", Severity.Success);
                nav.NavigateTo(nav.Uri, true); //Refresh page
            }

        }
        catch (Exception ex)
        {
            snackBar.Add(ex.Message, Severity.Error);
            await CleanForm();
        }
    }

    //Lấy thông tin User
    private async Task<AppUser> GetMeHandler() => models = await this.authService.GetMe();

    //Xóa User
    private async Task<bool> DeleteMeHandler() => await this.authService.DeleteMe();

    #endregion

    #region EditFrom to edit information

    protected AppUser models = new AppUser();
    protected bool _processing = true;
    protected string textResult;
    protected string nameEditControl;

    //Clean models
    protected async Task editControl()
    {
        _processing = !_processing;
        if (!_processing)
            nameEditControl = TEXT_UN_EDIT;
        else
            nameEditControl = TEXT_EDIT;
    }

    //Submit
    protected async void OnValidSubmit(EditContext editContext)
    {
        await editControl();
        //Do something
        var editUser = new AppEditDTO
        {
            FirstName = models.FirstName,
            LastName = models.LastName,
            Address = models.Address,
            PhoneNumber = models.PhoneNumber,
            Gender = models.Gender,
            BirthDay = models.BirthDay
        };
        await EditHandler(editUser);
        StateHasChanged();
    }

    //Clean models
    protected async Task CleanForm() => models = await GetMeHandler();

    #endregion 
}
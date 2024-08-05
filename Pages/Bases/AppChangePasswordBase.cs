using Microsoft.AspNetCore.Components;
using IdentityBlazorCoreAPI.Repositories.Interfaces;
using IdentityBlazorCoreAPI.Data.Models;
using MudBlazor;
using Microsoft.AspNetCore.Components.Forms;
using IdentityBlazorCoreAPI.Data.Entities;
using IdentityBlazorCoreAPI.Pages.Displays;

namespace IdentityBlazorCoreAPI.Pages.Bases;

public class AppChangePasswordBase : ComponentBase
{
    [Inject]
    private IAuthService authService { get; set; }
    [Inject]
    private ISnackbar snackBar { get; set; }
    [Inject]
    private NavigationManager nav { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            if (!await authService.CheckAuthenState()) nav.NavigateTo("/", true);

        }
        catch (Exception ex)
        {

        }
    }

    #region Private to handler with data
    private async Task ChangePasswordHandler(AppChangePasswordDTO _models)
    {
        try
        {
            if (await this.authService.ChangePassword(_models))
            {
                snackBar.Add($"Đã thay đổi mật khẩu thành công.", Severity.Success);

                nav.NavigateTo("/me", true);
            }
            else
            {
                snackBar.Add($"Lỗi vui lòng đăng nhập lại.", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            snackBar.Add(ex.Message, Severity.Error);
            _processing = false;
            models.CurrentPassword = "";
        }
    }
    #endregion


    #region EditFrom razor
    protected AppChangePasswordDTO models = new AppChangePasswordDTO();
    protected bool _processing = false;

    //Submit
    protected async void OnValidSubmit(EditContext editContext)
    {
        _processing = true;
        //Do something
        await ChangePasswordHandler(models);
        StateHasChanged();
    }

    //Clean models
    protected async Task CleanForm() => models = new AppChangePasswordDTO();
    #endregion


    #region MudTextField Password
    protected bool isShowPassword = false;
    protected InputType PasswordInput = InputType.Password;
    protected string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected async Task ShowPasswordEvent()
    {
        if (isShowPassword)
        {
            isShowPassword = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            isShowPassword = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }

    #endregion
}
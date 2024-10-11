using Microsoft.AspNetCore.Components;
using MudBlazor;
using IdentityBlazorCoreAPI.Repositories.Interfaces;
using IdentityBlazorCoreAPI.Data.Entities;
using Microsoft.AspNetCore.Components.Forms;

namespace IdentityBlazorCoreAPI.Pages.Bases;

public class SignUpBase : ComponentBase
{
    [Inject]
    private IAuthService authService { get; set; }
    //Notifycation
    [Inject]
    private ISnackbar snackBar { get; set; }
    [Inject]
    private NavigationManager nav { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            //Check authentication state
            if (await authService.CheckAuthenState()) nav.NavigateTo("/", true);
        }
        catch (Exception ex)
        {

        }
    }

    #region Private to handler with data
    private async Task RegisterHandler(AppRegisterDTO register)
    {
        try
        {
            if (await this.authService.Register(register))
            {
                snackBar.Add($"Đã đăng ký thành công.", Severity.Success);

                //Dừng 3s
                Thread.Sleep(TimeSpan.FromSeconds(3));

                //Vào trang đăng ký
                nav.NavigateTo("/login", true);
            }
            else
            {
                snackBar.Add($"Đã phát sinh lỗi vui lòng kiểm tra lại.", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            snackBar.Add(ex.Message, Severity.Error);
            _processing = false;
        }
    }
    #endregion

    #region EditFrom to register
    protected SignUpDTO models = new SignUpDTO();
    protected bool _processing = false;
    protected string textResult;

    //Submit
    protected async void OnValidSubmit(EditContext editContext)
    {
        _processing = true;
        //Do something
        // Khai báo một biến register để lưu giá trị convert
        var register = new AppRegisterDTO();
        register.ConvertSignUptoAppAuthDTO(models);
        Console.WriteLine("register.Email: "+ register.Email);
        await RegisterHandler(register);
        StateHasChanged();
    }

    //Clean models
    protected async Task CleanForm() => models = new SignUpDTO();
    #endregion 

    #region MudTextField Password
    protected bool isShowPassword = false;
    protected InputType PasswordInput = InputType.Password;
    protected string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected async Task ShowPasswordHandler()
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
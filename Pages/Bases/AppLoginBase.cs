using Microsoft.AspNetCore.Components;
using MudBlazor;
using IdentityBlazorCoreAPI.Data.Entities;
using IdentityBlazorCoreAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace IdentityBlazorCoreAPI.Pages.Bases;
public class AppLoginBase : ComponentBase
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
            // Check authentication state
            if (await authService.CheckAuthenState()) nav.NavigateTo("/", true);
        }
        catch (Exception ex)
        {

        }
    }
    
    #region Load Js google authen  
    [Inject]
    private IJSRuntime js { get; set; }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await js.InvokeVoidAsync("googleSingInOnPage", null);
        }
    }
    #endregion

    #region Private to handler with data                             
    // Login
    private async Task LoginHandler(AppLoginDTO _models)
    {
        try
        {
            await authService.Login(_models);
            snackBar.Add($"Đăng nhập thành công.", Severity.Success);

            // Dừng 3s sau khi chuyển hướng
            Thread.Sleep(TimeSpan.FromSeconds(3));

            // Chuyển về trang chủ
            nav.NavigateTo("/", true);
        }
        catch (Exception ex)
        {
            await CleanForm();
            snackBar.Add(ex.Message, Severity.Error);
        }
    }
    #endregion

    #region EditFrom to login
    protected AppLoginDTO models = new AppLoginDTO();
    protected bool _processing = false;
    protected string textResult;

    // Submit
    protected async void OnValidSubmit(EditContext editContext)
    {
        _processing = true;
        // Do something
        await LoginHandler(models);
        StateHasChanged();
    }

    // Clean models
    protected async Task CleanForm()
    {
        models = new AppLoginDTO();
        _processing = false;
        StateHasChanged();
    }
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
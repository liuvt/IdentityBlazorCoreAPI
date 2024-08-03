using IdentityBlazorCoreAPI.Data.Models;
using IdentityBlazorCoreAPI.Repositories.Interfaces;
using IdentityBlazorCoreAPI.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using IdentityBlazorCoreAPI.Owners.Pages;
using IdentityBlazorCoreAPI.Data.Entities;

namespace IdentityBlazorCoreAPI.Owners.Pages;
public class ManagerUsersDialogBase : ComponentBase
{
// Dữ liệu trả về sau khi submit: user đầu ra dữ liệu
    protected UserCreateDTO user {get; set;} = new UserCreateDTO();
    // Dữ liệu được truyền vào, xem trước khi cập nhật, sử dụng nếu đây là Cập nhật với isEdit = true: userEdit đầu vào dữ liệu
    [Parameter] public UserCreateDTO userEdit {get; set;} = new UserCreateDTO();
    // Tham số trạng thái cập nhật hay thêm mới
    [Parameter] public bool isEdit {get; set;}
    // Khai báo dialog
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    // Kiểm tra validation các mudtextfield có phù hợp hay không, nếu không nút submit hiden cho đến khi phù hợp yêu cầu
    protected bool formInvalid = true;
    protected string textResult;
    // Khai báo nội dung editform
    protected EditContext editContext;
    // Dữ liệu được submit từ editform

    protected async Task Submit()
    {
        MudDialog.Close(DialogResult.Ok(user));
    }
    protected void Cancel() => MudDialog.Cancel();

    protected void CleanForm()
    {
        if(!isEdit)
        {
            user = new UserCreateDTO();
            editContext = new EditContext(user);
        }
        else 
        {
            editContext = new EditContext(userEdit);
        }
        StateHasChanged(); 
    }

    protected override void OnInitialized()
    {
        // Load dữ liệu from khi khởi tạo
        if(isEdit != true){
            // Create: các field đều rỗng
            editContext = new EditContext(user);
        }
        else
        {
            // Update: các field được nhận từ tham số userEdit
            editContext = new EditContext(userEdit);
        }

        editContext.OnFieldChanged += HandleFieldChanged;
    }

    protected void HandleFieldChanged(object sender, FieldChangedEventArgs e)
    {
        // Kiểm tra xem các điều kiện trên field đã phù hợp hay chưa
        formInvalid = !editContext.Validate();
        StateHasChanged();
    }

    protected void HandleValidSubmit()
    {
        // Process the valid form
        StateHasChanged();
    }

    public void Dispose()
    {
        editContext.OnFieldChanged -= HandleFieldChanged;
    }

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
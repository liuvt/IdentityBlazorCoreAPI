using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IdentityBlazorCoreAPI.Pages.ComponentDialogs;

public class TemplatingDialogBase : ComponentBase
{
    [CascadingParameter] 
    protected MudDialogInstance MudDialog { get; set; }
    [Parameter]
    public string ContentText { get; set; } //Nội dung

    [Parameter]
    public string ButtonText { get; set; } //Tên button

    [Parameter]
    public Color ColorButton { get; set; } //Màu button
    [Parameter]
    public string IconTittle { get; set; } //Icon tiêu đề
    [Parameter]
    public string TitleText { get; set; } //Tiêu đề
    protected void Submit() => MudDialog.Close(DialogResult.Ok(true));
    protected void Cancel() => MudDialog.Cancel();
}
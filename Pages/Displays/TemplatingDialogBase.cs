using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IdentityBlazorCoreAPI.Pages.Displays;

public class TemplatingDialogBase : ComponentBase
{
    [CascadingParameter] 
    public MudDialogInstance MudDialog { get; set; }
    [Parameter]
    public string TextContent { get; set; } //Nội dung

    [Parameter]
    public string TextButton { get; set; } //Tên button

    [Parameter]
    public Color ColorButton { get; set; } //Màu button
    [Parameter]
    public string IconTittle { get; set; } //Icon tiêu đề
    [Parameter]
    public string TextTitle { get; set; } //Tiêu đề
    protected void Submit() => MudDialog.Close(DialogResult.Ok(true));
    protected void Cancel() => MudDialog.Cancel();
}
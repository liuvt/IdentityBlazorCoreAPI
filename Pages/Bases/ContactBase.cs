
using Microsoft.AspNetCore.Components;
using MudBlazor;
using IdentityBlazorCoreAPI.Repositories.Interfaces;
using IdentityBlazorCoreAPI.Data.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace IdentityBlazorCoreAPI.Pages.Bases;

public class ContactBase : ComponentBase
{

    [Inject]
    private ISnackbar snackBar { get; set; }
    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Dừng 3s trước khi mở form edit
            Thread.Sleep(TimeSpan.FromSeconds(3));
            _processing = false;
        }
        catch (Exception ex)
        {

        }
    }

    #region Edit form send mail
    [Inject]
    private IEtherealEmailService etherealEmailService {get; set; }
    protected EtherealEmail model = new EtherealEmail();
    protected bool _processing = true;
    protected async void OnValidSubmit(EditContext editContext)
    {
        await etherealEmailService.Send(model);
        snackBar.Add($"Đã gửi email.", Severity.Success);
        _processing = true;
        await CleanForm();
        StateHasChanged();
    }

    protected async Task CleanForm()
    {
        model = new EtherealEmail();
        StateHasChanged();
    }
    #endregion

}
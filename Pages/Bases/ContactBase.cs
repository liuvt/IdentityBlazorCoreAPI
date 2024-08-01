
using Microsoft.AspNetCore.Components;
using MudBlazor;
using IdentityBlazorCoreAPI.Repositories.Interfaces;
using IdentityBlazorCoreAPI.Data.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace IdentityBlazorCoreAPI.Pages.Bases;

public class ContactBase : ComponentBase
{
    [Inject]
    private IEtherealEmailService etherealEmailService {get; set; }
    [Inject]
    private ISnackbar snackBar { get; set; }
    protected EtherealEmail model = new EtherealEmail();
    protected override async Task OnInitializedAsync()
    {
        try
        {
          
        }
        catch (Exception ex)
        {

        }
    }

    protected async void OnValidSubmit(EditContext editContext)
    {
        await etherealEmailService.Send(model);
        StateHasChanged();
    }

    protected async Task CleanForm()
    {
        model = new EtherealEmail();
        StateHasChanged();
    }

}
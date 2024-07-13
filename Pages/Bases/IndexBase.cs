using IdentityBlazorCoreAPI.Data.Models;
using IdentityBlazorCoreAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace IdentityBlazorCoreAPI.Pages.Bases;

public class IndexBase : ComponentBase
{
    [Inject]
    protected IAuthService authService { get; set; }
    protected override async Task OnInitializedAsync()
    {
        try
        {
            if (await authService.CheckAuthenState())
            {
            }
        }
        catch (Exception ex)
        {

        }
    }
}


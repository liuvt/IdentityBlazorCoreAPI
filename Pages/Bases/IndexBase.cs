using IdentityBlazorCoreAPI.Data.Models;
using IdentityBlazorCoreAPI.Helpers;
using IdentityBlazorCoreAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace IdentityBlazorCoreAPI.Pages.Bases;

public class IndexBase : ComponentBase
{
    [Inject]
    protected IJSRuntime Js { get; set; }

    protected bool _isDarkMode { get; set; }
    protected string colorText { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var valueDarkMode = await Js.GetFromLocalStorage("_isDarkMode");
            _isDarkMode = bool.Parse(valueDarkMode.Replace("\"",""));
            if(_isDarkMode == true)
            {
                colorText = "color: black;";
            }
            else
            {
                colorText = "";
            }
        }
        catch (Exception ex)
        {

        }
    }
}


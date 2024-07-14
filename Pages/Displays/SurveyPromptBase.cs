using Microsoft.AspNetCore.Components;

namespace IdentityBlazorCoreAPI.Pages.Displays;
public class SurveyPromptBase : ComponentBase
{
    // Demonstrates how a parent component can supply parameters
    [Parameter]
    public string? Title { get; set; }
}
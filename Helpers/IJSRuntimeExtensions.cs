using Microsoft.JSInterop;

namespace IdentityBlazorCoreAPI.Helpers;

public static class IJSRuntimeExtensions
{
    /*
        Local Storage manager
        - GET/SET/REMOVE
    */
    public static ValueTask<string> GetFromLocalStorage(this IJSRuntime jS, string key)
        => jS.InvokeAsync<string>($"localStorage.getItem", key);

    public static ValueTask SetFromLocalStorage(this IJSRuntime jS, string key, string content)
        => jS.InvokeVoidAsync($"localStorage.setItem", key, content);

    public static ValueTask RemoveFromLocalStorage(this IJSRuntime jS, string key)
        => jS.InvokeVoidAsync($"localStorage.removeItem", key);
}
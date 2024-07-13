using IdentityBlazorCoreAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IdentityBlazorCoreAPI.Shared;

public class MainLayoutBase : LayoutComponentBase
{
    [Inject]
    protected IAuthService authService { get; set; }

    protected bool _drawerOpen = false;
    protected Anchor anchor;
    protected string width, height;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender) await authService.GetAuthenState();
        }
        catch (Exception ex)
        {
            await authService.LogOut();
            throw new Exception("Authentication error: " + ex.Message);
        }
    }

    //Mobile swipper slider Drawer Navigation Menu
    protected void OnSwipeEnd(SwipeEventArgs e)
    {
        if (e.SwipeDirection == SwipeDirection.LeftToRight && !_drawerOpen)
        {
            _drawerOpen = true;
            StateHasChanged();
        }
        else if (e.SwipeDirection == SwipeDirection.RightToLeft && _drawerOpen)
        {
            _drawerOpen = false;
            StateHasChanged();
        }
    }

    protected void DrawerToggle(Anchor _anchor)
    {
        _drawerOpen = !_drawerOpen;
        this.anchor = _anchor;
        width = "300px";
        height = "100%";
    }
}
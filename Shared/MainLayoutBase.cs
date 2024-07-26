using IdentityBlazorCoreAPI.Repositories.Interfaces;
using IdentityBlazorCoreAPI.Helpers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Microsoft.JSInterop;

namespace IdentityBlazorCoreAPI.Shared;

public class MainLayoutBase : LayoutComponentBase
{
    [Inject]
    private IAuthService authService { get; set; }

    [Inject]
    protected IJSRuntime Js { get; set; }

    // Lưu biến isdarkmode vào localstorage theo key
    private string localStorageDarkMode = "_isDarkMode";
    protected MudTheme _theme = new ();
    // Trang thái mặt định là Light Mode
    protected bool _isDarkMode = false;
    // Thay đổi Icon darkmode
    protected string modeIcon => _isDarkMode ? Icons.Material.Outlined.DarkMode : Icons.Material.Outlined.LightMode;
    protected string colorIcon => _isDarkMode ? "rgba(255,255,255,0.6980392156862745)" : Colors.Yellow.Accent3;
    // Thay đổi màu thanh App Bar theo darkmode
    protected string modeAppBarColor => _isDarkMode ? "background:rgba(50,51,61,1);" : "background:rgb(245,245,245);";
    // Thay đổi tên logo theo darkmode
    protected string modeNameCompany => _isDarkMode ? "" : "color:rgba(66,66,66,1);";

    // Drawer navigation
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

    protected override async Task OnInitializedAsync()
    {
        // Lấy giá trị Dark Mode trên LocalStorage
        var valueDarkMode = await Js.GetFromLocalStorage(localStorageDarkMode);
        if(valueDarkMode != null)
        {
            // Nếu không rỗng thì lấy giá trị
            _isDarkMode = bool.Parse(valueDarkMode.Replace("\"",""));
        }
        else
        {
            // Nếu rỗng thì set giá trị hiện tại
            await Js.SetFromLocalStorage(localStorageDarkMode, Convert.ToString(_isDarkMode));
        }
    }

    // Thay đổi trạng thái Dark mode và Light mode
    protected async Task DarkLightModeToggle()
    {
        _isDarkMode = !_isDarkMode;
        // Sau khi thay đổi trạng thái lưu vào localstorage
        await Js.SetFromLocalStorage(localStorageDarkMode, Convert.ToString(_isDarkMode));
        StateHasChanged();
    }

    // Trược cảm ứng từ trái sang phải để hiển thị navigation cho mobile và tablet
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

    // Hiển thị thanh navigation
    protected void DrawerToggle(Anchor _anchor)
    {
        _drawerOpen = !_drawerOpen;
        this.anchor = _anchor;
        width = "300px";
        height = "100%";
    }
}
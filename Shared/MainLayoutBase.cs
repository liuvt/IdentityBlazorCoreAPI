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
    private IJSRuntime Js { get; set; }

    // Lưu biến isdarkmode vào localstorage theo key
    private string localStorageDarkMode = "_isDarkMode";
    protected MudTheme _theme = new MudTheme()
    {
        // Thay đổi font mặt định của MudBlazor
        Typography = new Typography()
        {
            Default = new Default()
            {
                FontFamily = new[] { "Trirong", "sans-serif" },
            }
        },
        // Thay đổi trạng thái dark và light
        Palette = new PaletteLight()
        {
            // Đổi màu thanh appbar
            AppbarBackground = Colors.Grey.Lighten2,
            Primary = "#0A7BCF",
            Secondary = "#FFFFFF", // Secondary tham số Color ít dùng
        },
        PaletteDark = new PaletteDark()
        {
            // Đổi màu thanh appbar
            AppbarBackground = Colors.Grey.Darken3,
            Primary = "#6585e0",
            Secondary = "#E0E0E0", // Secondary tham số Color ít dùng
        },
    };
    // Trang thái mặt định là Light Mode
    protected bool _isDarkMode = false;
    // Thay đổi Icon darkmode
    protected string modeIcon => _isDarkMode ? Icons.Material.Outlined.DarkMode : Icons.Material.Outlined.LightMode;
    protected string colorIcon => _isDarkMode ? "#E0E0E0" : "#FDD835";

    // Drawer navigation
    protected bool _drawerOpen = false;
    // Anchor show navigation position
    protected Anchor anchor;
    // Navigation size
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
            StateHasChanged();
        }
        else
        {
            // Nếu rỗng thì set giá trị hiện tại
            await Js.SetFromLocalStorage(localStorageDarkMode, Convert.ToString(_isDarkMode));
            StateHasChanged();
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
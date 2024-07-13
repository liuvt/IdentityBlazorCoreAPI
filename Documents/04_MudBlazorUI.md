<h1>MudBlazor UI - IdentityBlazorCoreAPI v1 👋</h1>

- Using MudBlazor to MainLayout for Blazor Server
- Learn MudBlazor: https://mudblazor.com/

<h3>1. Get start MudBlazor</h3>

<h4>Add Enviroment</h4>

- dotnet add package MudBlazor

<h4>Add javascript and style in: _Host.cshtml</h4>

```html
    <!--MudBlazor style-->
    <link href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap" rel="stylesheet" />

    <!--MudBlazor script-->
    <script src="_content/MudBlazor/MudBlazor.min.js"></script>
```

<h4>Remove bootstrap in: wwwroot folder</h4>

<h4>Add using in: _Imports.razor</h4>

```c#
@using MudBlazor
```

<h4>Register in Program.cs</h4>

```c#
using MudBlazor.Services;

// UI: Add MudBlazor
builder.Services.AddMudServices();
```

<h4>Add MainLayout</h4>
<h4>Add NavMenu</h4>

<h3>2. Tricks</h3>

<h4>Break Points</h4>

![alt text](https://github.com/liuvt/IdentityBlazorCoreAPI/blob/main/Documents/Libraries/04_breakpoints.JPG)

<h4>Hide by Break Points</h4>

- Show size >= sm(<960px): August Center
- Show size < sm(<960px): AugCenter

```html
<MudText Id="augNameCompany" Class="d-none d-sm-flex">August Center</MudText>
<MudText Id="augNameCompany" Class="d-xs-flex d-sm-none">AugCenter</MudText>
```

<h4>Size auto for MudGrid by Break Points</h4>

- Max size: 12
- Mobile: one screen, A top and B bottom
- Tablet: A and B split the screen
- Desktop: A is greater than B

```html
<MudGrid>
    <MudItem xs="12" sm="6" md="8">
        A
    </MudItem>
    <MudItem xs="12" sm="6" md="4">
        B
    </MudItem>
</MudGrid>
```

<h4>Scroll to top</h4>

- Add below @body in MainLayout
```html
<MudScrollToTop>
    <MudFab Color="Color.Primary" StartIcon="@Icons.Material.Filled.KeyboardArrowUp" />
</MudScrollToTop>
```

<h4>MudSwipeArea</h4>

- Mobile trược trên mọi điểm ở Body để show navigation menu
```html
<MudSwipeArea OnSwipeEnd="@OnSwipeEnd" Style="width: 100%;">
    @body
</MudSwipeArea>
```

```c#
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
```
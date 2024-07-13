<h1>MudBlazor UI - IdentityBlazorCoreAPI v1 👋</h1>

- Using MudBlazor to MainLayout for Blazor Server
- Learn MudBlazor: https://mudblazor.com/

<h3>1. Get start MudBlazor</h3>

<h4>Enviroment</h4>

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

<h4>MainLayout</h4>
<h4>NavMenu</h4>

<h3>2. Tricks</h3>

<h4>Break Points</h4>

- MudBlazor use our own implementation of the original specification from Material design.
![alt text](https://github.com/liuvt/IdentityBlazorCoreAPI/blob/main/Documents/Libraries/04_breakpoints.JPG)

<h4>Hide by Break Points</h4>
```html
<!-- Show size >=sm -->
<MudText Id="augNameCompany" Class="d-none d-sm-flex">August Center</MudText>
<!-- Show size <sm -->
<MudText Id="augNameCompany" Class="d-xs-flex d-sm-none">AugCenter</MudText>
```

<h4>Size auto for MudGrid by Break Points</h4>

- Max size: 12
- Mobile: A,B not inline
- Tablet: A = B
- Desktop: A > B

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
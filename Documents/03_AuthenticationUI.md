<h1>Authentication UI - IdentityBlazorCoreAPI v1 👋</h1>

- Create IJSRuntimeExtensions.cs class use to Local Storage
- Create AuthService.cs and IAuthService.cs and AuthenticationStateProvider
- CascadingAuthenticationState for Razor page
- Use to razor page
- Login.razor
- Result

<h3>1. Create IJSRuntimeExtensions.cs class use to Local Storage</h3>

- A helper for Javascript control local storage
```c#
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
```

<h3>2. Create AuthService.cs and IAuthService.cs and AuthenticationStateProvider</h3>

<h4>Init AuthService.cs</h4>

```c#
public class AuthService : AuthenticationStateProvider, IAuthService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly HttpClient httpClient;
    //JavaScript
    private readonly IJSRuntime jS;
    //Key localStorage
    private string key = "_IdentityBlazorCoreAPI";
    //Anonymous authentication state
    private AuthenticationState Anonymous =>
        new AuthenticationState(new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity()));

    //Constructor
    public AuthService(IHttpClientFactory _httpClientFactory, HttpClient _httpClient, IJSRuntime _jS)
    {
        this.httpClientFactory = _httpClientFactory;
        this.httpClient = _httpClient;
        this.jS = _jS;
    }
}
```

<h4>Login</h4>

- We call API from Controller, read token when login and set from Local Storage, build authentication state
```c#
    public async Task Login(AppLoginDTO models)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync<AppLoginDTO>($"api/Auth/Login", models);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                await jS.SetFromLocalStorage(key, token);

                var state = BuildAuthenticationState(token);
                NotifyAuthenticationStateChanged(Task.FromResult(state));
            }
            else
            {
                var mess = await response.Content.ReadAsStringAsync();
                throw new Exception(mess);
            }
        }
        catch (System.Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
```

<h4>LogOut</h4>

- Remove token in Local Storage, then build an Anonymous authentication state
```c#
    public async Task LogOut()
    {
        try
        {
            await jS.RemoveFromLocalStorage(key);

            httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
        }
        catch (System.Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }
```

<h4>We need create an AuthenticationStateProvider custom to Login and LogOut</h4>

```c#
#region Authentication State
    /*
        Authentication
        - Get token in localStorage by key
        - Check token by ValidationToken(): bool
        - return BuildAuthenticationState(token)
    */
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        //Lấy token từ LocalStorage
        var token = await jS.GetFromLocalStorage(key);

        //Kiểm tra xem token 
        if (!ValidateToken(token))
        {
            return Anonymous;
        }

        //Build AuthenticationState
        return BuildAuthenticationState(token);
    }

    /*
        Build authentication state
        - Check authorization by token
        - Create ParseClaimsFromJwt get claims
        - Get Notify authentication state
        - return authenticationstate
    */
    private AuthenticationState BuildAuthenticationState(string token)
    {
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    //Parse Claims From Jwt
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    //Parse Base64 Without Padding
    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    //Validate
    private bool ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        //Check can read token
        var handler = new JwtSecurityTokenHandler();
        bool readToken = handler.CanReadToken(token.Replace("\"", ""));
        return readToken;
    }
    #endregion
```

<h4>IAuthService.cs interface we call Login and LogOut</h4>

```c#
public interface IAuthService
{   
    Task Login(AppLoginDTO models);
    Task LogOut();
}
```

<h4>We need to register in: Program.cs file</h4>

```c#
// UI: Register Repository
builder.Services.AddScoped<IAuthService, AuthService>();
// UI: Authentication
builder.Services.AddScoped<AuthenticationStateProvider, AuthService>();
builder.Services.AddAuthorizationCore();
```

<h3>3. CascadingAuthenticationState for Razor page</h3>

<h4>We need to register in: Program.cs file</h4>

```c#
// UI: Authentication
builder.Services.AddCascadingAuthenticationState();
```

<h4>Add in: App.razor</h4>

```c#
<CascadingAuthenticationState>
    <Router AppAssembly="@typeof(App).Assembly">
        <Found Context="routeData">
            <AuthorizeRouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)">
                <NotAuthorized>
                    <h1>Sorry, You'er Not Authorized</h1>
                    <p><a href="/login">Click here! to login</a></p>
                </NotAuthorized>
                <Authorizing>
                    <MudProgressCircular Color="Color.Secondary" Indeterminate="true" />
                </Authorizing>
            </AuthorizeRouteView>
            <FocusOnNavigate RouteData="@routeData" Selector="h1" />
        </Found>
        <NotFound>
            <PageTitle>Not found</PageTitle>
            <LayoutView Layout="@typeof(MainLayout)">
                <p role="alert">Sorry, there's nothing at this address.</p>
            </LayoutView>
        </NotFound>
    </Router>
</CascadingAuthenticationState>
```

<h4>4. Use to razor page</h4>

- The first way
```c#
//Content: Guest and User can see
<AuthorizeView Context="authContext">
//Content: Guest and User can see
    <Authorized>
    //Content: User can see, Guest can't see
    </Authorized>
//Content: Guest and User can see
    <NotAuthorized>
    //Content: Guest can see, User can't see
    </NotAuthorized>
//Content: Guest and User can see
</AuthorizeView>
```
- The second way
```c#
@page "/me"
@attribute [Authorize] 
```

<h3>4. Login razor</h3>

<h4>Component base for razor page</h4>

- Create AppLoginBase.cs componentbase class, Inject IAuthService.cs interface, then call Login method
```c#
public class AppLoginBase : ComponentBase
{
    [Inject]
    private IAuthService authService { get; set; }
    [Inject]
    private ISnackbar snackBar { get; set; }
    [Inject]
    private NavigationManager nav { get; set; }

    private async Task LoginHandler(AppLoginDTO _models)
    {
        try
        {         
            await authService.Login(_models);
            snackBar.Add($"Login success.", Severity.Success);
            //Waiting 3s
            Thread.Sleep(TimeSpan.FromSeconds(3));
            //Navigation to home page 
            nav.NavigateTo("/", true);
        }
        catch (Exception ex)
        {
            snackBar.Add(ex.Message, Severity.Error);
        }
    }
}
```

<h4>Create Login.razor page</h4>

- We need to inherits AppLoginBase.cs component base
```c#
@page "/login"
@inherits AppLoginBase
```
<h3>5 Result</h3>

- Login.razor page
![alt text](https://github.com/liuvt/IdentityBlazorCoreAPI/blob/main/Documents/Libraries/03_login.JPG)

- Check token in localhost after login: https://jwt.io/
![alt text](https://github.com/liuvt/IdentityBlazorCoreAPI/blob/main/Documents/Libraries/03_jwt.JPG)
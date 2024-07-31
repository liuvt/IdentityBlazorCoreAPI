using IdentityBlazorCoreAPI.Data;
using Microsoft.Net.Http.Headers;
using IdentityBlazorCoreAPI.Repositories.Interfaces;
using IdentityBlazorCoreAPI.Repositories.Services;
using MudBlazor.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityBlazorCoreAPI.APIServers.Contracts;
using IdentityBlazorCoreAPI.APIServers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;
using IdentityBlazorCoreAPI.Modules.APIYoutube.Services;
using IdentityBlazorCoreAPI.Modules.APIDrive;
using Microsoft.AspNetCore.Authentication.Cookies;
using Google.Apis.Auth.AspNetCore3;
using IdentityBlazorCoreAPI.Modules.XMLFoods.Services;
using IdentityBlazorCoreAPI.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// API: Connect to Mysql server
builder.Services.AddDbContext<IdentityBlazorCoreAPIDbContext>(
    opt =>
    {
        opt.UseMySql(builder.Configuration.GetConnectionString("LocalDB"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));
        /*
            Fix error: The instance of entity type cannot be tracked because another instance 
                                                with the same key value for { 'ID'} is already being tracked.
            Các truy vấn trên Repository chỉ được xem, không thể cập nhật. Để cập nhật/thêm mới/xóa: 
            - Update: context.Entry<Entities>(_model).State = EntityState.Modified;
            - Add: context.Entry<Entities>(_model).State = EntityState.Added;
            - Delete: context.Entry<Entities>(_model).State = EntityState.Deleted;
        */
        opt.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
    }, ServiceLifetime.Transient
);

// API: Add Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<IdentityBlazorCoreAPIDbContext>()
        .AddDefaultTokenProviders();
// UI: Tăng kích thước bộ nhớ đệm
builder.Services.AddSignalR(e =>
{
    e.MaximumReceiveMessageSize = 102400000;
});

// UI: Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// UI: Add MudBlazor
builder.Services.AddMudServices();

// UI: Register Client Factory
builder.Services.AddHttpClient("IdentityBlazorCoreAPIServer", httpClient =>
    {
        httpClient.BaseAddress = new Uri(builder.Configuration["API:localhost"] ??
                                throw new InvalidOperationException("Can't found [Secret Key] in appsettings.json !"));
        httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "HttpRequestIdentityBlazorCoreAPI");
    });

// UI: Get httpClient API default
builder.Services.AddScoped(
    defaultClient => new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["API:localhost"] ??
                                throw new InvalidOperationException("Can't found [Secret Key] in appsettings.json !"))
    });

// API: Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// API: Add Jwt, Gooogle Authentication
builder.Services.AddAuthentication(authenticationOptions => {
                    authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    //authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    //authenticationOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

                    // This forces challenge results to be handled by Google OpenID Handler, so there's no
                    // need to add an AccountController that emits challenges for Login.
                    authenticationOptions.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                    // This forces forbid results to be handled by Google OpenID Handler, which checks if
                    // extra scopes are required and does automatic incremental auth.
                    authenticationOptions.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                    // Default scheme that will handle everything else.
                    // Once a user is authenticated, the OAuth2 token info is stored in cookies.
                    authenticationOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddGoogleOpenIdConnect(options => {
                    options.SkipUnrecognizedRequests = true;
                    options.ClientId = builder.Configuration["GoogleWebApplication:client_id"]
                                        ?? throw new InvalidOperationException("Can't found [Secret Key] in appsettings.json !");
                    options.ClientSecret = builder.Configuration["GoogleWebApplication:client_secret"]
                                            ?? throw new InvalidOperationException("Can't found [Secret Key] in appsettings.json !");
                })
                .AddJwtBearer(jwtBearerOptions => {
                    jwtBearerOptions.RequireHttpsMetadata = false;
                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters {
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                            ValidAudience = builder.Configuration["JWT:ValidAudience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]
                                                ?? throw new InvalidOperationException("Can't found [Secret Key] in appsettings.json !"))
                            ),
                            ValidateIssuer = false,
                            ValidateAudience = false
                    };
                });

// API: Add SwaggerGen (dotnet add package Swashbuckle.AspNetCore)
builder.Services.AddSwaggerGen(
    opt =>
    {
        /*
        //Fix Identity, Refresh Token, Access Token, 
        //CURD Product, Editor Text, Review, Blog, Image Upload, Login API Facebook, Google
        //AugCenterLibrary
        opt.SwaggerDoc("v3", new OpenApiInfo { Title = "AugCenter API", Version = "v3" });
        //For Identity, Login, Register, Change Mudblazor, AugCenterLog, AugCenterDocs
        opt.SwaggerDoc("v2", new OpenApiInfo { Title = "AugCenter API", Version = "v2" });
        */
        //Init project: CRUD category,order,orderdetail,..., AugCenterModel
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Blazor Server Core API", Version = "v1" });
        opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            In = ParameterLocation.Header,
            Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")"
        });

        //Add filter to block case authorize: Swashbuckle.AspNetCore.Filters
        opt.OperationFilter<SecurityRequirementsOperationFilter>();
    }
);

// API: Register ApiServers
builder.Services.AddScoped<IAuthServer, AuthServer>();
builder.Services.AddScoped<IUserServer, UserServer>();
builder.Services.AddScoped<IYoutubeServer, YoutubeServer>();

// UI: Register Repository
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
// UI: Modules XML
builder.Services.AddScoped<IFoodService, FoodService>();
// UI: Modules Youtube
builder.Services.AddScoped<IYoutubeService, YoutubeService>();
builder.Services.AddScoped<IGoogleDriveService, GoogleDriveService>();
// UI: Authentication
builder.Services.AddScoped<AuthenticationStateProvider, AuthService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// UI: Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else // API: Add run Swagger UI: https://localhost:5187/swagger/index.html
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI(
        opt =>
        {
            opt.SwaggerEndpoint($"/swagger/v1/swagger.json", "Manager Business API V1");
        }
    );
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// API: Add Authoz and Authen
app.UseAuthentication();
app.UseAuthorization();

// API: Add controllers
app.MapControllers();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

<h1> Authentication API - IdentityBlazorCoreAPI v1 👋 </h1>

- Identity connection to mySQL server
- Ef migration with IdentityUser
- Create dependency injection (DI): AuthServer class, IAuthServer interface
- Create AuthController
- Authen and Author from API Server

<h3>Enviroment</h3>
 
````
//Identity
- dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
- dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
- dotnet add package Microsoft.EntityFrameworkCore
- dotnet add package Microsoft.EntityFrameworkCore.Design
- dotnet add package Microsoft.EntityFrameworkCore.InMemory
- dotnet add package Microsoft.EntityFrameworkCore.Relational
- dotnet add package Microsoft.EntityFrameworkCore.SqlServer
- dotnet add package Microsoft.EntityFrameworkCore.Tools
//Authentication
- dotnet add package Microsoft.AspNetCore.Components.Authorization
- dotnet add package Microsoft.AspNetCore.Authentication.Cookies
//Jwt
- dotnet add package Newtonsoft.Json
- dotnet add package System.IdentityModel.Tokens.Jwt
- dotnet add package System.Security.Claims
- dotnet add package Microsoft.IdentityModel.Tokens
//Connector to mySQL server
- dotnet add Pomelo.EntityFrameworkCore.MySql
````

<h3>Identity connection to mySQL server</h3>

<h4> Create model use identity: AppUser.cs</h4>

```c#
using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime? BirthDay { get; set; }
}
```

<h4> Create IdentityBlazorCoreAPIDbContext.cs file</h4>

```c#
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public partial class IdentityBlazorCoreAPIDbContext : IdentityDbContext<AppUser>
{
    //Get config in appsetting
    private readonly IConfiguration configuration;
    //Default constructor
    public IdentityBlazorCoreAPIDbContext()
    {
    }

    //Constructor with parameter
    public IdentityBlazorCoreAPIDbContext(DbContextOptions<IdentityBlazorCoreAPIDbContext> options, IConfiguration _configuration) : base(options)
    {
        //Models - Etityties
        this.configuration = _configuration;
    }

    //Config to connection mysql server
    //configuration["ConnectionStrings:LocalDB"]: in appsettings.json
    protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(configuration["ConnectionStrings:LocalDB"] ?? 
                                throw new InvalidOperationException("Can't found [Secret Key] in appsettings.json !"), 
                ServerVersion.Parse("8.0.31-mysql")
            );
        }
    }

    //Data seeding
    protected override async void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Do something
        base.OnModelCreating(modelBuilder);
    }
}
```

<h4>Add connection string to mysql server in: appsettings.json</h4>

```csharp
"ConnectionStrings": {
    "LocalDB": "server=localhost;user=root;password=123456aA@;Port=3306;database=IdentityBlazorAPI; Persist Security Info=False; Connect Timeout=300"
  }
```

<h4> Register identity and connector mysql: Program.cs</h4>

```c#
/* API: Add Identity
    AppUser: is model inherits IdentityUser
    builder.Configuration.GetConnectionString("LocalDB"): is configuration to appsettings.json, there is connection string to mysql server
*/
builder.Services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<IdentityBlazorCoreAPIDbContext>()
        .AddDefaultTokenProviders();

// API: Connect to Mysql server
builder.Services.AddDbContext<IdentityBlazorCoreAPIDbContext>(
    opt => {
        opt.UseMySql(builder.Configuration.GetConnectionString("LocalDB"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));
        opt.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
    }, ServiceLifetime.Transient
);

```

<h3>Ef migration</h3>

- Create mirations: dotnet ef migrations add Init -o Data/Migrations
- Create database: dotnet ef database update

- We can add data seeding before migration, in OnModelCreating method, in ```IdentityBlazorCoreAPIDbContext.cs``` file
```c#
//Data seeding
protected override async void OnModelCreating(ModelBuilder modelBuilder)
{
    //Role table
    modelBuilder.Entity<IdentityRole>().HasData(
        new IdentityRole[] {
            new IdentityRole
            {
                Id = "1",
                Name = "Owner",
                NormalizedName = """Owner""".ToUpper(),
                ConcurrencyStamp = Convert.ToString(DateTime.Now)
            },
            new IdentityRole
            {
                Id = "2",
                Name = "Administrator",
                NormalizedName = """Administructor""".ToUpper(),
                ConcurrencyStamp = Convert.ToString(DateTime.Now)
            },
        }
    );

        //User table
    var passwordHasher = new PasswordHasher<AppUser>();
    var userOwner = new AppUser
    {
        Id = "OWNER-AUGCENTER-2023",
        UserName = "owner@augcenter.com",
        Email = "owner@augcenter.com",
        NormalizedUserName = "OWNER@AUGCENTER.COM",
        FirstName = "Aug",
        LastName = "Center",
        NormalizedEmail = "OWNER@AUGCENTER.COM",
        PhoneNumber = "0868752251",
        LockoutEnabled = false
    };
    userOwner.PasswordHash = passwordHasher.HashPassword(userOwner, "Owner@123");
    var userAdmin = new AppUser
    {
        Id = "ADMIN-AUGCENTER-2023",
        UserName = "administructor@augcenter.com",
        Email = "administructor@augcenter.com",
        NormalizedUserName = "ADMINISTRUCTOR@AUGCENTER.COM",
        FirstName = "Aug",
        LastName = "Center",
        NormalizedEmail = "ADMINISTRUCTOR@AUGCENTER.COM",
        PhoneNumber = "0868752251",
        LockoutEnabled = false
    };
    userAdmin.PasswordHash = passwordHasher.HashPassword(userAdmin, "Admin@123");
    modelBuilder.Entity<AppUser>().HasData(new AppUser[] { userOwner, userAdmin, userSeller, userBuyer });

    //UserRole table
    modelBuilder.Entity<IdentityUserRole<string>>().HasData(
        new IdentityUserRole<string>[] {
            new IdentityUserRole<string>
            {
                UserId = "OWNER-AUGCENTER-2023",
                RoleId = "1",
            },
            new IdentityUserRole<string>
            {
                UserId = "ADMIN-AUGCENTER-2023",
                RoleId = "2",
            }
        }
    );

    base.OnModelCreating(modelBuilder);
}
```

<h3>Create dependency injection (DI): AuthApiServer class, IAuthApiServer interface</h3>

- Create interface: IAuthApiServer.cs
```c#
public interface IAuthApiServer
{
    //Get me
    Task<AppUser> GetMe(string userId);
}
```

- Create class inherits interface. We use Identity by UserManager and SignInManager to manager: AuthApiServer.cs
```c#
public class AuthApiServer : IAuthApiServer
{

    //User Manager
    protected readonly UserManager<AppUser> userManager;
    protected readonly SignInManager<AppUser> loginManager;
    protected readonly IConfiguration configuration;

    //Constructor
    public AuthApiServer(UserManager<AppUser> _userManager, SignInManager<AppUser> _loginManager,
                        IConfiguration _configuration)
    {
        this.userManager = _userManager;
        this.loginManager = _loginManager;
        this.configuration = _configuration;
    }

    /* Get me when we login success */
    public async Task<AppUser> GetMe(string userId)
        => await this.userManager.FindByIdAsync(userId) ?? throw new Exception("Not found!");
}
```

- Register dependency inject(DI) in: ```Program.cs``` file
```c#
// API: Register ApiServers
builder.Services.AddScoped<IAuthApiServer, AuthApiServer>();
```

<h3>Create AuthController.cs</h3>

```c#
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    //Get API Server
    private readonly IAuthApiServer context;
    private readonly ILogger<AuthController> logger;
    public AuthController(IAuthApiServer _context, ILogger<AuthController> _logger)
    {
        this.context = _context;
        this.logger = _logger;
    }

    [HttpGet("Me"), Authorize]
    public async Task<ActionResult<AppUser>> GetMe()
    {
        try
        {
            /*
                Console.WriteLine("User name: " + User.FindFirstValue(ClaimTypes.NameIdentifier)); //User name
                Console.WriteLine("Email: " + User.FindFirstValue(ClaimTypes.Email));        //Email
                Console.WriteLine("Role: " + User.FindFirstValue(ClaimTypes.Role));             //Role
                Console.WriteLine("User Id: " + User.FindFirstValue("ObjectIdentifier"));    //User Id
            */
            return Ok(await this.context.GetMe(this.User.FindFirstValue("ObjectIdentifier")
                                                    ?? throw new Exception("Not found ID")));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                                                                "Error: " + ex.Message);
        }
    }
}
```

<h3>Authen and Author from API Server</h3>

- We need to add Authentication to use Identity. Create infomation in: ```appsettings.json``` file
```c#
"JWT":{
    "ValidAudience": "Identity Blazor Core API",
    "ValidIssuer": "http://localhost:5187",
    "Secret": "This is my Secret, my infomation: mrdurk@gmail.com , the key size must be greater than: 512 bits"
  },
```

- Register Authentication and read json: Program.cs file
```c#
// API: Add Jwt Authentication
builder.Services.AddAuthentication(
    opt => 
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }
)
.AddJwtBearer(opt =>
    {
        opt.RequireHttpsMetadata = false;
        opt.SaveToken = true;
        opt.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                ValidAudience = builder.Configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    builder.Configuration["JWT:Secret"] ?? throw new InvalidOperationException("Can't found [Secret Key] in appsettings.json !"))

                ),
                ValidateIssuer = false,
                ValidateAudience = false
            };
    }
);

// API: Add Authoz and Authen
app.UseAuthentication();
app.UseAuthorization();
```

- Create method in: AuthApiServer.cs class
```c#
    //Login app
    public async Task<AppUser> Login(AppLoginDTO login)
    {
        try
        {
            var user = await this.userManager.FindByEmailAsync(login.Email);
            if (user == null)
                throw new Exception("Wrong Email or Password");

            var result = await this.loginManager.PasswordSignInAsync(
                                                login.Email, login.Password, false, false);
            if (!result.Succeeded)
                throw new Exception("Wrong Email or Password");

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    //Create token when login
    public async Task<string> CreateToken(InfomationUserSaveInToken user)
    {
        try
        {
            var listClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, user.userEmail),
                            new Claim(ClaimTypes.NameIdentifier, user.userName is not null ? user.userName : string.Empty),
                            new Claim("ObjectIdentifier", user.userId),
                            new Claim(ClaimTypes.Role, user.userRole),
                            new Claim(JwtRegisteredClaimNames.Jti, user.userGuiId)
                        };

            var autKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]
                ?? throw new InvalidOperationException("Can't found [Secret Key] in appsettings.json !")));

            var signCredentials = new SigningCredentials(autKey, SecurityAlgorithms.HmacSha512Signature);

            var autToken = new JwtSecurityToken(
                claims: listClaims, //Thông tin User
                issuer: configuration["JWT:ValidIssuer"], //In file appsetting.json
                audience: configuration["JWT:ValidAudience"], //In file appsetting.json
                expires: DateTime.Now.AddDays(7), //Thời gian tồn tại Token
                signingCredentials: signCredentials //Chữ ký
            );

            return new JwtSecurityTokenHandler().WriteToken(autToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    //Get role to create token
    public async Task<string> GetRoleName(AppUser user)
    {
        try
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var rolename = userRoles.Select(e => e).FirstOrDefault();
            return (rolename == null) ? string.Empty : rolename;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

```

- Create method in: IAuthApiServer.cs interface
```c#
    //Login
    Task<AppUser> Login(AppLoginDTO login);
    //Create Token
    Task<string> CreateToken(InfomationUserSaveInToken user);

    //Get Role name
    Task<string> GetRoleName(AppUser user);
```

- Create method in: AuthController.cs file
```c#
    [HttpPost("Login")]
    public async Task<ActionResult<string>> Login(AppLoginDTO appLogin)
    {
        try
        {
            //Login
            var appUser = await this.context.Login(appLogin);
            if (appUser == null)
                throw new Exception("Wrong Email or Password");

            //Role
            var role = await this.context.GetRoleName(appUser);

            //Create token
            var userClaim = new InfomationUserSaveInToken()
            {
                userId = appUser.Id is not null ? appUser.Id : string.Empty,
                userEmail = appUser.Email is not null ? appUser.Email : string.Empty,
                userName = appUser.UserName is not null ? appUser.UserName : string.Empty,
                userRole = role,
                userGuiId = Guid.NewGuid().ToString()
            };

            var token = await this.context.CreateToken(userClaim);

            return Ok(token);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                                                                "Error: " + ex.Message);
        }
    }
```

- Create Model in AppUser.cs and AppUserDTO.cs
```c#
//Login: create token
public partial class InfomationUserSaveInToken
{
    public string userId { get; set; } = string.Empty;
    public string userEmail { get; set; } = string.Empty;
    public string userName { get; set; } = string.Empty;
    public string userRole { get; set; } = string.Empty;
    public string userGuiId { get; set; } = string.Empty;
}

//Login
public class AppLoginDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
```

<h3>Result</h3>

- We use data seeding in Ef migration 
![alt text](https://github.com/liuvt/IdentityBlazorCoreAPI/blob/main/Documents/Libraries/02_paramater.JPG)

- Result
![alt text](https://github.com/liuvt/IdentityBlazorCoreAPI/blob/main/Documents/Libraries/02_result.JPG)

- Check information token: https://jwt.io/
![alt text](https://github.com/liuvt/IdentityBlazorCoreAPI/blob/main/Documents/Libraries/02_jwt.JPG)
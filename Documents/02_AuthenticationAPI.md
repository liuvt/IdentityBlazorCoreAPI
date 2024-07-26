<h1>Authentication API - IdentityBlazorCoreAPI v1 👋</h1>

- Enviroment nuget package
- Identity connection to mySQL server
- Ef migration with IdentityUser
- Create dependency injection (DI): AuthServer class, IAuthServer interface, AuthController, Entities
- Authen and Author from API Server
- Result

<h3>1. Enviroment</h3>
 
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

<h3>2. Identity connection to mySQL server</h3>

<h4> Create model use identity: AppUser.cs</h4>

```c#
public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime? BirthDay { get; set; }
}

//Register: set default role for new User
public partial class UserRoles
{
    public string RoleId { get; set; } = "5";
    public string RoleName { get; set; } = "Buyer";
    public bool IsSelected { get; set; } = true;
}

//Login: create token 
public partial class InfomationUserSaveInToken
{
    public string id { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string userName { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public string giveName { get; set; } = string.Empty;
    public string userRole { get; set; } = string.Empty;
    public string userGuiId { get; set; } = string.Empty;
}
```

<h4> Create IdentityBlazorCoreAPIDbContext.cs file</h4>

```c#
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
    protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(
                configuration["ConnectionStrings:LocalDB"] ?? 
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

```

<h3>3. Ef migration</h3>

- Create mirations: dotnet ef migrations add Init -o Data/Migrations
- Create database: dotnet ef database update

- We can add data seeding before migration, in OnModelCreating method, in ```IdentityBlazorCoreAPIDbContext.cs``` file
```c#
//Data seeding
protected override async void OnModelCreating(ModelBuilder modelBuilder)
{
    //Seeding Data bảng Roles
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
                    new IdentityRole
                    {
                        Id = "3",
                        Name = "Manager",
                        NormalizedName = """Manager""".ToUpper(),
                        ConcurrencyStamp = Convert.ToString(DateTime.Now)
                    },
                    new IdentityRole
                    {
                        Id = "4",
                        Name = "Seller",
                        NormalizedName = """Seller""".ToUpper(),
                        ConcurrencyStamp = Convert.ToString(DateTime.Now)
                    },
                    new IdentityRole
                    {
                        Id = "5",
                        Name = "Buyer",
                        NormalizedName = """Buyer""".ToUpper(),
                        ConcurrencyStamp = Convert.ToString(DateTime.Now)
                    },
            }
        );
        //Seeding Data bảng User
        var passwordHasher = new PasswordHasher<AppUser>();
        //Owner
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
        //Admin
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
        //Seller
        var userSeller = new AppUser
        {
            Id = "SELLER-AUGCENTER-2023",
            UserName = "seller@augcenter.com",
            Email = "seller@augcenter.com",
            NormalizedUserName = "SELLER@AUGCENTER.COM",
            FirstName = "Aug",
            LastName = "Center",
            NormalizedEmail = "SELLER@AUGCENTER.COM",
            PhoneNumber = "0868752251",
            LockoutEnabled = false
        };
        userSeller.PasswordHash = passwordHasher.HashPassword(userSeller, "Seller@123");
        //Buyer
        var userBuyer = new AppUser
        {
            Id = "BUYER-AUGCENTER-2023",
            UserName = "buyer@augcenter.com",
            Email = "buyer@augcenter.com",
            NormalizedUserName = "BUYER@AUGCENTER.COM",
            FirstName = "Aug",
            LastName = "Center",
            NormalizedEmail = "BUYER@AUGCENTER.COM",
            PhoneNumber = "0868752251",
            LockoutEnabled = false
        };
        userBuyer.PasswordHash = passwordHasher.HashPassword(userBuyer, "Buyer@123");
        modelBuilder.Entity<AppUser>().HasData(new AppUser[] { userOwner, userAdmin, userSeller, userBuyer });

        //Seeding Data bảng UserRole
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
                    },
                    new IdentityUserRole<string>
                    {
                        UserId = "SELLER-AUGCENTER-2023",
                        RoleId = "4",
                    },
                    new IdentityUserRole<string>
                    {
                        UserId = "BUYER-AUGCENTER-2023",
                        RoleId = "5",
                    }
            }
        );

    base.OnModelCreating(modelBuilder);
}
```

<h3>4. Create dependency injection (DI): AuthServer class, IAuthServer interface, AuthController</h3>

- Create interface: IAuthServer.cs
```c#
public interface IAuthServer
{
    //Login
    Task<AppUser> Login(AppLoginDTO login);
    //Register
    Task<IdentityResult> Register(AppRegisterDTO register);

    //Create Token
    Task<string> CreateToken(InfomationUserSaveInToken user);

    //Get Role name
    Task<string> GetRoleName(AppUser user);

    //Get me
    Task<AppUser> GetMe(string userId);

    //Edit 
    Task<IdentityResult> EditMe(AppEditDTO models, string userId);
    //Delete
    Task<IdentityResult> DeleteMe(string userId);
    Task<IdentityResult> ChangeCurrentPassword(string userId, string oldPassword, string newPassword);
}
```

- Create class inherits interface. We use Identity by UserManager and SignInManager to manager: AuthApiServer.cs
```c#
public class AuthServer : IAuthServer
{

    //User Manager
    protected readonly UserManager<AppUser> userManager;
    protected readonly SignInManager<AppUser> loginManager;
    protected readonly IConfiguration configuration;

    //Constructor
    public AuthServer(UserManager<AppUser> _userManager, SignInManager<AppUser> _loginManager,
                        IConfiguration _configuration)
    {
        this.userManager = _userManager;
        this.loginManager = _loginManager;
        this.configuration = _configuration;
    }

    /* Lấy thông tin bản thân */
    public async Task<AppUser> GetMe(string userId)
        => await this.userManager.FindByIdAsync(userId) ?? throw new Exception("Lỗi người dùng vui lòng đăng nhập lại!");

    /* Xóa tài khoản */
    public async Task<IdentityResult> DeleteMe(string userId)
    {
        var user = await this.userManager.FindByIdAsync(userId);
        if(user == null) throw new Exception("Lỗi người dùng vui lòng đăng nhập lại!");
        var result = await this.userManager.DeleteAsync(user);
        return result;
    }

    /* Đăng nhập */
    public async Task<AppUser> Login(AppLoginDTO login)
    {
        try
        {
            //Kiểm tra email
            var user = await this.userManager.FindByEmailAsync(login.Email);
            if (user == null)
                throw new Exception("Wrong Email or Password");

            //Đăng nhập với Email và Password
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

    /* Đăng ký */
    public async Task<IdentityResult> Register(AppRegisterDTO register)
    {
        //Kiểm tra email
        var user = await this.userManager.FindByEmailAsync(register.Email);
        if (user != null)
            throw new Exception("Email đã tồn tại");

        var newUser = new AppUser
        {
            Email = register.Email,
            FirstName = register.FirstName,
            LastName = register.LastName,
            UserName = register.Email,
            Gender = register.Gender
        };

        //Create password
        var createUser = await userManager.CreateAsync(newUser, register.Password);

        //Kiểm tra trạng thái đăng ký thành công
        if (!createUser.Succeeded)
            throw new Exception("Đăng ký không thành công vui lòng làm lại!");
        else
        {
            #region Set role USER mặt định cho người dùng nếu đăng ký thành công
            //Khởi tạo IEnumerable<string> roles của hàm AddToRolesAsync
            List<UserRoles> roles = new List<UserRoles>();
            //Mặt định khi tạo mới tài khoản là quyền User
            var userRoles = new UserRoles() { RoleId = "5", RoleName = "Buyer", IsSelected = true };
            roles.Add(userRoles);

            //Đăng ký role cho user vừa được tạo trên bảng aspnetuserroles [Database]
            await userManager.AddToRolesAsync(newUser,
                            roles.Where(x => x.IsSelected).Select(y => y.RoleName));
            #endregion
                                           
            //Đăng ký thành công trả về thông tin IdentityResult
            return createUser;
        }
    }

    /* Cập nhật */
    public async Task<IdentityResult> EditMe(AppEditDTO models, string userId)
    {
        var user = await this.userManager.FindByIdAsync(userId) ?? throw new Exception("Lỗi người dùng vui lòng đăng nhập lại");
        
        user.FirstName = models.FirstName;
        user.LastName = models.LastName;
        user.PhoneNumber = models.PhoneNumber;
        user.Address = models.Address;
        user.Gender = models.Gender;
        user.BirthDay = models.BirthDay;

        return await userManager.UpdateAsync(user);
    }

    /* Change currentPassword*/
    public async Task<IdentityResult> ChangeCurrentPassword(string userId, string currentPassword, string newPassword)
    {
        var user = await this.userManager.FindByIdAsync(userId) ?? throw new Exception("Lỗi người dùng vui lòng đăng nhập lại");
        
        var changePassword = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if(!changePassword.Succeeded) throw new Exception("Mật khẩu củ không đúng");

        return changePassword;
    }

    /* Tạo token*/
    public async Task<string> CreateToken(InfomationUserSaveInToken user)
    {
        try
        {
            //Thông tin User đưa vào Token
            var listClaims = new List<Claim>
                        {
                            new Claim("id", user.id),
                            new Claim("username", user.userName),
                            new Claim("email", user.email),
                            new Claim("name", user.name),
                            new Claim("give_name", user.giveName),
                            new Claim(ClaimTypes.Role, user.userRole),
                            new Claim(JwtRegisteredClaimNames.Jti, user.userGuiId)
                        };

            //Khóa bí mật
            var autKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]
                ?? throw new InvalidOperationException("Can't found [Secret Key] in appsettings.json !")));

            //Tạo chữ ký với khóa bí mật
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

    /* Lấy thông tin quyền truy cập */
    public async Task<string> GetRoleName(AppUser user)
    {
        try
        {
            //Lấy Role của User
            var userRoles = await userManager.GetRolesAsync(user);
            var rolename = userRoles.Select(e => e).FirstOrDefault();
            return (rolename == null) ? string.Empty : rolename;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
```

- Register dependency inject(DI) in: ```Program.cs``` file
```c#
// API: Register ApiServers
builder.Services.AddScoped<IAuthServer, AuthServer>();
```

<h3>4. Create AuthController.cs</h3>

```c#
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    //Get API Server
    private readonly IAuthServer context;
    private readonly ILogger<AuthController> logger;
    public AuthController(IAuthServer _context, ILogger<AuthController> _logger)
    {
        this.context = _context;
        this.logger = _logger;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(AppRegisterDTO register)
    {
        try
        {
            var result = await this.context.Register(register);
            if (!result.Succeeded) return Unauthorized();

            return Ok(result.Succeeded);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

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
                id = appUser.Id ?? string.Empty,
                email = appUser.Email ?? string.Empty,
                name = $"{appUser.FirstName} {appUser.LastName}" ?? string.Empty,
                giveName = $"{appUser.FirstName} {appUser.LastName}" ?? string.Empty,
                userName = appUser.UserName ?? string.Empty,
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

    [HttpGet("Me"), Authorize]
    public async Task<ActionResult<AppUser>> GetMe()
    {
        try
        {
            /*
                Console.WriteLine("User name: " + User.FindFirstValue(ClaimTypes.NameIdentifier)); //User name
                Console.WriteLine("Email: " + User.FindFirstValue(email));        //Email
                Console.WriteLine("Role: " + User.FindFirstValue(ClaimTypes.Role));             //Role
                Console.WriteLine("User Id: " + User.FindFirstValue("id"));    //User Id
            */
            return Ok(await this.context.GetMe(this.User.FindFirstValue("id")
                                                    ?? throw new Exception("Not found User ID")));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                                                                "Error: " + ex.Message);
        }
    }

    [HttpPatch("Me/Edit"), Authorize]
    public async Task<IActionResult> EditMe(AppEditDTO models)
    {
        try
        {
            var edit = await this.context.EditMe(models, this.User.FindFirstValue("id")
                                                                ?? throw new Exception("Not found User ID"));
            if (!edit.Succeeded) return BadRequest();

            return Ok(edit.Succeeded);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                                                                "Error: " + ex.Message);
        }
    }

    [HttpPatch("Me/ChangePassword"), Authorize]
    public async Task<IActionResult> ChangeCurrentPassword(AppChangePasswordDTO changePassword)
    {
        try
        {
            var userId = this.User.FindFirstValue("id")
                                        ?? throw new Exception("Not found User ID");
            var resetpassword = await this.context.ChangeCurrentPassword(userId, changePassword.CurrentPassword, changePassword.Password);
            if (!resetpassword.Succeeded) return Unauthorized();

            return Ok(resetpassword.Succeeded);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                                                                "Error: " + ex.Message);
        }
    }

    [HttpDelete("Me"), Authorize]
    public async Task<IActionResult> DeleteMe()
    {
        try
        {
            var userId = this.User.FindFirstValue("id")
                                            ?? throw new Exception("Không tìm thấy User ID");
            var delete = await this.context.DeleteMe(userId);
            if (!delete.Succeeded) return Unauthorized();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                                                            "Error: " + ex.Message);
        }
    }

}
```

<h4>Authen and Author from API Server</h4>

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

<h4>Create Entities AppUserDTO.cs</h4>

```c#
//Login: Using Regular Expression
public class AppLoginDTO
{
    [Required(ErrorMessage = "Email không được bỏ trống.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Mật khẩu không được bỏ trống.")]
    public string Password { get; set; } = string.Empty;
}

//Register
public partial class AppRegisterDTO
{
    [Required(ErrorMessage = "Email không được bỏ trống.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    //Hổ trợ: gmail|homail|yahoo|viettel|outlook|skyper
    [RegularExpression(@"([a-zA-Z0-9_.-]+)@(gmail|homail|yahoo|viettel|outlook|skyper).([a-zA-Z]{2,4}|[0-9]{1,3})?.([a-zA-Z]{2,4}|[0-9]{1,3})"
        , ErrorMessage = "Email hổ trợ: gmail, outlook, homail, yahoo, viettel, skyper.")]
    public string Email { get; set; } = string.Empty;

    
    [Required(ErrorMessage = "Họ không được bỏ trống.")]
    [StringLength(30, ErrorMessage = "Nhập tối thiểu 3 ký tự.", MinimumLength = 3)]
    public string FirstName { get; set; } = string.Empty;

    
    [Required(ErrorMessage = "Tên không được bỏ trống.")]
    [StringLength(30, ErrorMessage = "Nhập tối thiểu 3 ký tự.", MinimumLength = 3)]
    public string LastName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Giới tính không được bỏ trống.")]
    public string Gender { get; set; } = string.Empty;

    
    [Required(ErrorMessage = "Mật khẩu không được bỏ trống.")]
    //Mật khẩu yêu cầu: 8-15 ký tự, 1 ký tự đặt biệt (!,#,$,%,..), 1 ký tự viết hoa, 1 chữ số. Ví dụ: Abc!1234
    [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\\]*+\\/|!\"£$%^&*()#[@~'?><,.=_-]).{8,15}$"
        , ErrorMessage = "Mật khẩu yêu cầu: 8-15 ký tự, 1 ký tự đặt biệt (!,#,$,%,..), 1 ký tự viết hoa, 1 chữ số. Ví dụ: Abc!1234.")]
    public string Password { get; set; } = string.Empty;

    
    [Compare(nameof(Password), ErrorMessage = "Nhập lại mật khẩu không khớp.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

//Update
public partial class AppEditDTO
{
    [Required(ErrorMessage = "Họ không được bỏ trống.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tên không được bỏ trống.")]
    public string LastName { get; set; } = string.Empty;
   
    [Required(ErrorMessage = "Địa chỉ không được bỏ trống.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Giới tính không được bỏ trống.")]
    public string Gender { get; set; } = string.Empty;
    public DateTime? BirthDay { get; set; }
    
    [Required(ErrorMessage = "Số điện thoại không được bỏ trống.")]
    [RegularExpression(@"((84|60|86|02|01|0)[1-9]{1})+(([0-9]{8})|([0-9]{9})|([0-9]{10}))", 
                                                    ErrorMessage = "Số điện thoại không hợp lệ.")]
    public string PhoneNumber { get; set; } = string.Empty;
}

//Change password
public partial class AppChangePasswordDTO
{
    [Required(ErrorMessage = "Không được bỏ trống.")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mật khẩu không được bỏ trống.")]
    //Mật khẩu yêu cầu: 8-15 ký tự, 1 ký tự đặt biệt (!,#,$,%,..), 1 ký tự viết hoa, 1 chữ số. Ví dụ: Abc!1234
    [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\\]*+\\/|!\"£$%^&*()#[@~'?><,.=_-]).{8,15}$"
        , ErrorMessage = "Mật khẩu yêu cầu: 8-15 ký tự, 1 ký tự đặt biệt (!,#,$,%,..), 1 ký tự viết hoa, 1 chữ số. Ví dụ: Abc!1234.")]
    public string Password { get; set; } = string.Empty;

    [Compare(nameof(Password), ErrorMessage = "Nhập lại mật khẩu không khớp.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
```

<h3>6. Result</h3>

- We use data seeding in Ef migration 
![alt text](https://github.com/liuvt/IdentityBlazorCoreAPI/blob/main/Documents/Libraries/02_paramater.JPG)

- Result
![alt text](https://github.com/liuvt/IdentityBlazorCoreAPI/blob/main/Documents/Libraries/02_result.JPG)

- Check information token: https://jwt.io/
![alt text](https://github.com/liuvt/IdentityBlazorCoreAPI/blob/main/Documents/Libraries/02_jwt.JPG)
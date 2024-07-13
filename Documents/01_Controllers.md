<h1> Controller - IdentityBlazorCoreAPI v1 👋 </h1>

- Create Controllers for Blazor Server
- Add SwaggerUI for Blazor Server

<h3>Enviroment</h3>
 
````
- dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer 
- dotnet add package Swashbuckle.AspNetCore
- dotnet add package System.Security.Cryptography.Primitives
- dotnet add package Swashbuckle.AspNetCore.Filters
````

<h3>Create files</h3>

- |Create **Controllers** folder
- |_Create **DemoController.cs** file
- |__Create **Method1Controller** method to HttpGet 

```c#
[ApiController]
[Route("api/[controller]")]
public class DemoControllers : ControllerBase
{


    [HttpGet]
    public async Task<IActionResult> Method1Controller()
    {
        try
        {
           var message = "This test controllers API";

            return Ok(message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

}
```

<h3>Register controller in Program.cs files</h3>

```c#
// API: Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// API: Add SwaggerGen
builder.Services.AddSwaggerGen(
    opt =>
    {
        //Init project: CRUD category,order,orderdetail,..., AugCenterModel
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Manager Business", Version = "v1" });
        opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            In = ParameterLocation.Header,
            Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")"
        });

        //Add filter to block case authorize
        opt.OperationFilter<SecurityRequirementsOperationFilter>();
    }
);

// API: Add run Swagger UI: http://[YourLocalHost]/swagger/index.html
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        opt =>
        {
            opt.SwaggerEndpoint($"/swagger/v1/swagger.json", "Manager Business API V1");
        }
    );
}

// API: Add controllers
app.MapControllers();
```

<h3>Run</h3>

- Swagger UI: http://[Your LocalHost:Port]/swagger/index.html
- Index page: http://[Your LocalHost:Port]

<h3>Result</h3>

![alt text](https://github.com/liuvt/IdentityBlazorCoreAPI/blob/main/Documents/Libraries/01_result.JPG)
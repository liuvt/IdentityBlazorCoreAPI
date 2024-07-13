using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityBlazorCoreAPI.Controllers;

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
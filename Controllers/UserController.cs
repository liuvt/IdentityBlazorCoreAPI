using IdentityBlazorCoreAPI.APIServers.Contracts;
using IdentityBlazorCoreAPI.Data.Entities;
using IdentityBlazorCoreAPI.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityBlazorCoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    //Get API Server
    private readonly IUserServer context;
    private readonly ILogger<UserController> logger;
    public UserController(IUserServer _context, ILogger<UserController> _logger)
    {
        this.context = _context;
        this.logger = _logger;
    }

    [HttpGet("Gets"), Authorize(Roles = "Owner")]
    public async Task<ActionResult<List<AppUser>>> Gets()
    {
        try
        {
            var result = await this.context.Gets();

            if (result == null) return Unauthorized();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost("Create"), Authorize(Roles = "Owner")]
    public async Task<IActionResult> Create(UserCreateDTO userCreateDTO)
    {
        try
        {
            var result = await this.context.Create(userCreateDTO);
            if (!result.Succeeded) return Unauthorized();

            return Ok(result.Succeeded);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("Delete/{userId}"), Authorize(Roles = "Owner")]
    public async Task<ActionResult<bool>> Delete(string userId)
    {
        try
        {
            var delete = await this.context.Delete(userId);
            if (!delete.Succeeded) return Unauthorized();

            return Ok(delete);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                                                            "Error: " + ex.Message);
        }
    }

    [HttpPost("Delete/Select"), Authorize(Roles = "Owner")]
    public async Task<ActionResult<bool>> DeleteSelect([FromBody] IEnumerable<string> userIds)
    {
        try
        {
            var deleteSelect = await this.context.DeleteSelect(userIds);
            if (!deleteSelect) return Unauthorized();

            return Ok(deleteSelect);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                                                            "Error: " + ex.Message);
        }
    }
}
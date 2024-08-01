using IdentityBlazorCoreAPI.APIServers.Contracts;
using IdentityBlazorCoreAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityBlazorCoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EtherealEmailController : ControllerBase
{
    //Get API Server
    private readonly IEtherealEmailServer context;
    private readonly ILogger<EtherealEmailController> logger;
    public EtherealEmailController(IEtherealEmailServer _context, ILogger<EtherealEmailController> _logger)
    {
        this.context = _context;
        this.logger = _logger;
    }

    [HttpPost]
    public IActionResult Send(EtherealEmail model)
    {
        try
        {   
            this.context.Send(model);

            return Ok("Success");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc;
using IdentityBlazorCoreAPI.Modules.Models;
using IdentityBlazorCoreAPI.APIServers.Contracts;

namespace IdentityBlazorCoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class YoutubeController : ControllerBase
{
    //Get API Server
    private readonly ILogger<YoutubeController> logger;
    private readonly IYoutubeServer context;
    public YoutubeController(ILogger<YoutubeController> _logger, IYoutubeServer _context)
    {
        this.logger = _logger;
        this.context = _context;
    }

    [HttpGet("Playlists")]
    public async Task<ActionResult<IEnumerable<YoutubePlayList>>> GetPlaylists()
    {
        try
        {
            var playLists = await context.GetPlaylists();
            return Ok(playLists);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("PlaylistItems/{playListId}")]
    public async Task<ActionResult<YoutubePlayListItem>> GetPlaylistItems(string playListId)
    {
        try
        {
            var playListItems = await context.GetPlaylistItems(playListId);
            return Ok(playListItems);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("SearchChannelVideos")]
    public async Task<ActionResult<YoutubeVideoPagination>> GetSearchChannelVideos(string? pageToken)
    {
        try
        {
            var seachChannel = await context.GetSearchChannelVideos(pageToken);
            return Ok(seachChannel);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("Videos/{videoId}")]
    public async Task<IActionResult> GetVideosById(string videoId)
    {
        try
        {
            var video = await context.GetVideosById(videoId);
            return Ok(video);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
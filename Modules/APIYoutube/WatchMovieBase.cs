using Microsoft.AspNetCore.Components;
using IdentityBlazorCoreAPI.Data.Models;

namespace IdentityBlazorCoreAPI.Modules.APIYoutube;
public class WatchMovieBase : ComponentBase
{
    [Parameter]
    public string listId { get; set; }
    [Inject]
    private IYoutubeService youtubeService {get; set;}
    [Inject]
    private NavigationManager nav { get; set; }
    protected IEnumerable<YoutubeVideo> videos { get; set; } = new List<YoutubeVideo>();
    protected YoutubeVideo video { get; set; } = new YoutubeVideo();
    protected YoutubePlayListItem playListItem { get; set; } = new YoutubePlayListItem();
    protected string focus  { get; set; } = "background-color: #fff;";

    //Start first
    protected override async Task OnInitializedAsync()
    {
        try
        {
            playListItem = await youtubeService.GetPlaylistItems(listId);
            videos = playListItem.Videos.ToList();
            video = videos.Select(e => e).FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception("Error: "+ ex);
        }
    }
    protected async Task selectVideo (string videoId)
    {
        video = videos.Where(e => e.VideoId == videoId).FirstOrDefault();
        StateHasChanged();
    }
}
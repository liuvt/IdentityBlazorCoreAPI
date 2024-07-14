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
    protected YtbPlaylistItem playListItem = new YtbPlaylistItem();
    public IEnumerable<YtbPlaylistItem> playListItems {get; set;} = new List<YtbPlaylistItem>();
    protected string focus  { get; set; } = "background-color: #fff;";

    //Start first
    protected override async Task OnInitializedAsync()
    {
        try
        {
            playListItems = await youtubeService.GetPlaylistItems(listId);
            playListItem = playListItems.Select(e => e).FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception("Error: "+ ex);
        }
    }

    protected async Task GetUrlID (string playItemId)
    {
        playListItem = playListItems.Where(e => e.plItemId == playItemId).FirstOrDefault();
        StateHasChanged();
    }
}
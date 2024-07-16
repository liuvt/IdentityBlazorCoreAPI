using Microsoft.AspNetCore.Components;
using IdentityBlazorCoreAPI.Data.Models;

namespace IdentityBlazorCoreAPI.Modules.APIYoutube;
public class ComponentPlayListsBase : ComponentBase
{
    public IEnumerable<YoutubePlayList> playLists { get; set; } = new List<YoutubePlayList>();
    [Inject]
    private IYoutubeService youtubeService {get; set;}

    protected override async Task OnInitializedAsync()
    {
        playLists = await youtubeService.GetPlaylists();
    }
}
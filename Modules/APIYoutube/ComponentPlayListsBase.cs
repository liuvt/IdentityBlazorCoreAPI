using Microsoft.AspNetCore.Components;
using IdentityBlazorCoreAPI.Modules.Models;
using IdentityBlazorCoreAPI.Modules.APIYoutube.Services;

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
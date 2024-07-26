using Microsoft.AspNetCore.Components;
using IdentityBlazorCoreAPI.Modules.Models;
using IdentityBlazorCoreAPI.Modules.APIYoutube.Services;

namespace IdentityBlazorCoreAPI.Modules.APIYoutube;
public class WatchMovieBase : ComponentBase
{
    [Parameter]
    public string listId { get; set; }
    [Inject]
    private IYoutubeService youtubeService {get; set;}
    protected IEnumerable<YoutubeVideo> videos { get; set; } = new List<YoutubeVideo>();
    protected YoutubeVideo video { get; set; } = new YoutubeVideo();
    protected YoutubePlayListItem playListItem { get; set; } = new YoutubePlayListItem();
    protected string focus  { get; set; } = "background-color: #fff;";

    //Start first
    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Gọi api lấy ds video của một playlist thông qua ID của playlist
            playListItem = await youtubeService.GetPlaylistItems(listId);
            
            // Lấy ds video ra từ playlistItem
            videos = playListItem.Videos.ToList();
            // Lấy một video trong danh sách video
            video = videos.Select(e => e).FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception("Error: "+ ex);
        }
    }

    //Click vào một video theo ID của video trong ds playlistitem
    protected async Task selectVideo (string videoId) =>  video = videos.Where(e => e.VideoId == videoId).FirstOrDefault();

}
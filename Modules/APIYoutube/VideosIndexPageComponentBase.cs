using Microsoft.AspNetCore.Components;
using IdentityBlazorCoreAPI.Modules.Models;
using IdentityBlazorCoreAPI.Modules.APIYoutube.Services;

namespace IdentityBlazorCoreAPI.Modules.APIYoutube;
public class VideosIndexPageComponentBase : ComponentBase
{
    public IEnumerable<YoutubePlayList> playLists { get; set; } = new List<YoutubePlayList>();
    [Inject]
    private IYoutubeService youtubeService {get; set;}
    // Thông báo lỗi khi lần đầu load trang
    protected string message { get; set; }
    protected override async Task OnInitializedAsync()
    {
        try
        {
            playLists = await youtubeService.GetPlaylists();
        }
        catch (Exception ex)
        {
            // Nếu có tham số Paramater là ID, cần bắt lỗi nếu không tìm thấy ID.
            message = $"Lỗi: {ex.Message}";
        }
    }
}
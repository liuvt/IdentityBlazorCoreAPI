using Microsoft.AspNetCore.Components;
using IdentityBlazorCoreAPI.Modules.Models;
using IdentityBlazorCoreAPI.Modules.APIYoutube.Services;

namespace IdentityBlazorCoreAPI.Modules.APIYoutube;
public class VideosDetailBase : ComponentBase
{
    [Parameter] public string listId { get; set; }
    [Inject] private IYoutubeService youtubeService { get; set; }
    // Video hiển thị
    protected IEnumerable<YoutubeVideo> videos { get; set; } = new List<YoutubeVideo>();
    protected YoutubeVideo video { get; set; } = new YoutubeVideo();
    // PlayListItem có chưa ds Video
    protected YoutubePlayListItem playListItem { get; set; } = new YoutubePlayListItem();
    // Thông báo lỗi khi lần đầu load trang
    protected string message { get; set; }
    protected bool isPlayList { get; set; } = true;

    // Start first
    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Gọi api lấy ds video của một playlist thông qua ID của playlist
            playListItem = await youtubeService.GetPlaylistItems(listId);

            // Kiểm tra video trong danh sách
            if (playListItem.Videos == null)
                message = $"Danh sách hiện chưa có video nào!";

            videos = playListItem.Videos.ToList();

            // Khi load trang lần đầu: thông tin tên và mô tả video
            selectVideo(videos.FirstOrDefault());
                
        }
        catch (Exception ex)
        {
            // Nếu có tham số Paramater là ID, cần bắt lỗi nếu không tìm thấy ID.
            message = $"Xin lỗi, trang này không tồn tại. Error: {ex.Message}";
        }
    }

    // Click vào ds video trong listvideo
    protected void selectVideo(YoutubeVideo _video) => video = _video;
}
using IdentityBlazorCoreAPI.Modules.Models;
namespace IdentityBlazorCoreAPI.Modules.APIYoutube.Services;

public interface IYoutubeService
{
    //Lấy toàn bộ playlists
    Task<IEnumerable<YoutubePlayList>> GetPlaylists();
    //Lấy toàn bộ Video trong playlist thông qua id của playlist
    Task<YoutubePlayListItem> GetPlaylistItems(string playListId);
    //Lấy toàn bộ video trong channel, tối đa hiển thị 50 video, truyền mã token để loading tiếp 50video tiếp theo
    Task<YtbResponse> GetChannelVideos(string? pageToken = null, int maxResult = 50);
}
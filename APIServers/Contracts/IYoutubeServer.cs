
using IdentityBlazorCoreAPI.Modules.Models;

namespace IdentityBlazorCoreAPI.APIServers.Contracts;
public interface IYoutubeServer
{
    /* API Reference
        https://developers.google.com/youtube/v3/docs
    */

    //Lấy ds playlists
    Task<IEnumerable<YoutubePlayList>> GetPlaylists();
    //Lây playlist theo playListId
    Task<YoutubePlayList> GetPlaylist(string playListId);
    //Lấy ds Video trong playlist thông qua id của playlist
    Task<YoutubePlayListItem> GetPlaylistItems(string playListId);
    //Lấy toàn bộ video trong channel, tối đa hiển thị 50 video, truyền mã token để loading tiếp 50video tiếp theo
    Task<YoutubeVideoPagination> GetSearchChannelVideos(string? pageToken = null);
    Task<YoutubeVideo> GetVideosById(string videoId);
} 

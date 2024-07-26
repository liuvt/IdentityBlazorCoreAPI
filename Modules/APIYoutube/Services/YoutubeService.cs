using IdentityBlazorCoreAPI.Modules.Models;
namespace IdentityBlazorCoreAPI.Modules.APIYoutube.Services;
public class YoutubeService : IYoutubeService
{
    private readonly HttpClient httpClient;
    public YoutubeService(HttpClient _httpClient)
    {
        this.httpClient = _httpClient;
    }

    public async Task<IEnumerable<YoutubePlayList>> GetPlaylists()
    {
        try
        {
            var response = await httpClient.GetAsync($"api/Youtube/Playlists");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(IEnumerable<YoutubePlayList>);
                }

                return await response.Content.ReadFromJsonAsync<IEnumerable<YoutubePlayList>>();
            }
            else
            {
                var mess = await response.Content.ReadAsStringAsync();
                throw new Exception(mess);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<YoutubePlayListItem> GetPlaylistItems(string playListId)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/Youtube/PlaylistItems/{playListId}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(YoutubePlayListItem);
                }

                return await response.Content.ReadFromJsonAsync<YoutubePlayListItem>();
            }
            else
            {
                var mess = await response.Content.ReadAsStringAsync();
                throw new Exception(mess);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<YoutubeVideoPagination> GetSearchChannelVideos(string? pageToken = null)   
    {
        try
        {
            var response = await httpClient.GetAsync($"api/Youtube/SearchChannelVideos/");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(YoutubeVideoPagination);
                }

                return await response.Content.ReadFromJsonAsync<YoutubeVideoPagination>();
            }
            else
            {
                var mess = await response.Content.ReadAsStringAsync();
                throw new Exception(mess);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<YoutubeVideo> GetVideosById(string videoId)   
    {
        try
        {
            var response = await httpClient.GetAsync($"api/Youtube/Videos/{videoId}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(YoutubeVideo);
                }

                return await response.Content.ReadFromJsonAsync<YoutubeVideo>();
            }
            else
            {
                var mess = await response.Content.ReadAsStringAsync();
                throw new Exception(mess);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}

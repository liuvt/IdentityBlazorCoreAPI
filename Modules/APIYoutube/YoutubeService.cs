using IdentityBlazorCoreAPI.Data.Models;
using IdentityBlazorCoreAPI.Modules.APIYoutube;
namespace IdentityBlazorCoreAPI.Modules.APIYoutube;
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
    public async Task<YtbResponse> GetChannelVideos(string? pageToken = null, int maxResult = 50)   
    {
        try
        {
            var response = await httpClient.GetAsync($"api/Youtube/Search/");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(YtbResponse);
                }

                return await response.Content.ReadFromJsonAsync<YtbResponse>();
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

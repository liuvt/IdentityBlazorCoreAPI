using IdentityBlazorCoreAPI.APIServers.Contracts;
using IdentityBlazorCoreAPI.Data.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace IdentityBlazorCoreAPI.APIServers;

public class YoutubeServer : IYoutubeServer
{
    protected readonly IConfiguration configuration;
    private readonly YouTubeService youtubeService;

    //Constructor
    public YoutubeServer(IConfiguration _configuration)
    {
        this.configuration = _configuration;
        this.youtubeService = new YouTubeService(new BaseClientService.Initializer
        {
            ApiKey = configuration["YoutubeAPI:ApiKey"],
            ApplicationName = configuration["YoutubeAPI:ApplicationName"]
        });
    }

    public async Task<IEnumerable<YtbPlaylists>> GetPlaylists()
    {
        try
        {
            var request = youtubeService.Playlists.List("snippet");
            request.ChannelId = configuration["YoutubeAPI:ChannelId"];
            request.MaxResults = 50;
            var result = await request.ExecuteAsync();

            var listVideo = result.Items.Select(item => new YtbPlaylists
            {
                plId = item.Id,
                plTitle = item.Snippet.Title,
                plLink = $"https://www.youtube.com/playlist?list={item.Id}",
                plThumbnail = item.Snippet.Thumbnails.Maxres.Url
            });

            return listVideo;
        }
        catch (Exception ex)
        {
            throw new Exception("Get playlists not work by: "+ ex.Message);
        }
    }

    public async Task<IEnumerable<YtbPlaylistItem>> GetPlaylistItems(string playListId)
    {
        try
        {
            var request = youtubeService.PlaylistItems.List("snippet");
            request.PlaylistId = playListId;
            request.MaxResults = 50;
            request.PageToken = null;
            var result = await request.ExecuteAsync();
           
            var listVideo = result.Items.Select(item => new YtbPlaylistItem
            {
                plItemId = item.Snippet.ResourceId.VideoId,
                plItemTitle = item.Snippet.Title,
                plItemLink = $"https://www.youtube.com/watch?v={item.Snippet.ResourceId.VideoId}&list={item.Snippet.PlaylistId}",
                plItemThumbnail = (item.Snippet.Title == "Deleted video" || item.Snippet.Title == "Private video") 
                                    ? string.Empty : item.Snippet.Thumbnails.Maxres.Url
            });

            return listVideo;
        }
        catch (Exception ex)
        {
            throw new Exception("Get videos in playlist not work by: "+ ex.Message);
        }
    }

    public async Task<YtbResponse> GetChannelVideos(string? pageToken = null, int maxResult = 50)
    {
        try
        {
            var request = youtubeService.Search.List("snippet");
            request.ChannelId = configuration["YoutubeAPI:ChannelId"];
            request.Order = SearchResource.ListRequest.OrderEnum.Date;
            request.MaxResults = maxResult; //Tối đa item trả về 50 video
            request.PageToken = pageToken; //Phân trang
            var result = await request.ExecuteAsync();

            var listVideo = result.Items.Select(static item => new YtbMovie
            {
                YbTitle = item.Snippet.Title,
                YbLink = $"https://www.youtube.com/watch?v={item.Id.VideoId}",
                YbThumbnail = item.Snippet.Thumbnails.Medium.Url,
                YbDescription = item.Snippet.Description,
                YbPublishedAt = item.Snippet.PublishedAtDateTimeOffset
            }).OrderByDescending(video => video.YbPublishedAt).ToList();

            var response = new YtbResponse
            {
                YbVideos = listVideo,
                NextPageToken = result.NextPageToken,
                PrevPageToken = result.PrevPageToken,
            };

            return response;
        }
        catch (Exception ex)
        {
            throw new Exception("Get videos in channel not work by: "+ ex.Message);
        }
    }

}
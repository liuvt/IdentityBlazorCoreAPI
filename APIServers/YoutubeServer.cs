using IdentityBlazorCoreAPI.APIServers.Contracts;
using IdentityBlazorCoreAPI.Modules.Models;
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

    public async Task<IEnumerable<YoutubePlayList>> GetPlaylists()
    {
        try
        {
            var request = youtubeService.Playlists.List("snippet");
            request.ChannelId = configuration["YoutubeAPI:ChannelId"];
            request.MaxResults = 50;
            var result = await request.ExecuteAsync();

            var playLists = result.Items.Select(item => new YoutubePlayList
            {
                PlayListId = item.Id,
                PlayListTitle = item.Snippet.Title,
                PlayListDescription = item.Snippet.Description,
                PlayListThumbnail = item.Snippet.Thumbnails.Maxres.Url,
                PlayListPublishedAt = item.Snippet.PublishedAtDateTimeOffset
            }).ToList();

            return playLists;
        }
        catch (Exception ex)
        {
            throw new Exception("Get playlists not work by: "+ ex.Message);
        }
    }

   public async Task<YoutubePlayList> GetPlaylist(string playListId)
    {
        try
        {
            var request = youtubeService.Playlists.List("snippet");
            request.Id = playListId;
            var result = await request.ExecuteAsync();
                
            var playLists = result.Items.Select(item => new YoutubePlayList
            {
                PlayListId = item.Id,
                PlayListTitle = item.Snippet.Title,
                PlayListDescription = item.Snippet.Description,
                PlayListThumbnail = item.Snippet.Thumbnails.Maxres.Url,
                PlayListPublishedAt = item.Snippet.PublishedAtDateTimeOffset
            }).FirstOrDefault();
            if(playLists == null) throw new Exception("Không tìm thấy danh sách video!");

            return playLists;
        }
        catch (Exception ex)
        {
           throw new Exception("Get playlist by id not work: "+ ex.Message);
        }
    }

    public async Task<YoutubePlayListItem> GetPlaylistItems(string playListId)
    {
        try
        {
            //Get playlist by id
            var playList = await GetPlaylist(playListId);

            //Get list video by playlist id
            var request = youtubeService.PlaylistItems.List("snippet");
            request.PlaylistId = playListId;
            request.MaxResults = 50;
            request.PageToken = null;
            var result = await request.ExecuteAsync();

            var videos = result.Items.Select(item => new YoutubeVideo
            {
                VideoId = item.Snippet.ResourceId.VideoId,
                VideoTitle = item.Snippet.Title,
                VideoThumbnail = (item.Snippet.Title == "Deleted video" || item.Snippet.Title == "Private video")
                                    ? string.Empty : item.Snippet.Thumbnails.Maxres.Url,
                VideoDescription = item.Snippet.Description,
                VideoPublishedAt = item.Snippet.PublishedAtDateTimeOffset,
            });

            //Convert playlist and list video
            var playListItems = new YoutubePlayListItem
            {
                PlayListId = playList.PlayListId,
                PlayListTitle = playList.PlayListTitle,
                Videos = videos.ToList()
            };

            return playListItems;
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
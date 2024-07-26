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

            var listVideos = result.Items.Select(item => new YoutubeVideo
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
                Videos = listVideos.Where(v => v.VideoThumbnail != string.Empty).ToList()
            };

            return playListItems;
        }
        catch (Exception ex)
        {
            throw new Exception("Get videos in playlist not work by: "+ ex.Message);
        }
    }

    public async Task<YoutubeVideoPagination> GetSearchChannelVideos(string? pageToken = null)
    {
        try
        {
            var request = youtubeService.Search.List("snippet");
            //Search by ChannelId: https://developers.google.com/youtube/v3/docs/search/list
            request.ChannelId = configuration["YoutubeAPI:ChannelId"];
            request.Order = SearchResource.ListRequest.OrderEnum.Date;
            request.MaxResults = 50; //Tối đa item trả về 50 video
            request.PageToken = pageToken; //Phân trang
            var result = await request.ExecuteAsync();

            var listVideos = result.Items.Select(item => new YoutubeVideo
            {
                VideoId = item.Id.VideoId,
                VideoTitle = item.Snippet.Title,
                VideoThumbnail = (item.Snippet.Title == "Deleted video" || item.Snippet.Title == "Private video")
                                    ? string.Empty : item.Snippet.Thumbnails.High.Url,
                VideoDescription = item.Snippet.Description,
                VideoPublishedAt = item.Snippet.PublishedAtDateTimeOffset,
            }).OrderByDescending(v => v.VideoPublishedAt);

            var response = new YoutubeVideoPagination
            {
                Videos = listVideos.Where(v => v.VideoId != null).ToList(),
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

    public async Task<YoutubeVideo> GetVideosById(string videoId)
    {
        try
        {
            var request = youtubeService.Videos.List("snippet");
            //Video by Id: https://developers.google.com/youtube/v3/docs/videos/list
            request.Id = videoId;

            var result = await request.ExecuteAsync();

            var video = result.Items.Select(item => new YoutubeVideo
            {
                VideoId = item.Id,
                VideoTitle = item.Snippet.Title,
                VideoThumbnail = (item.Snippet.Title == "Deleted video" || item.Snippet.Title == "Private video")
                                    ? string.Empty : item.Snippet.Thumbnails.High.Url,
                VideoDescription = item.Snippet.Description,
                VideoPublishedAt = item.Snippet.PublishedAtDateTimeOffset,
            }).FirstOrDefault();

            return video;
        }
        catch (Exception ex)
        {
            throw new Exception("Get videos in channel not work by: "+ ex.Message);
        }
    }
}
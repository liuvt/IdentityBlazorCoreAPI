namespace IdentityBlazorCoreAPI.Modules.Models;

// Trả về video
public class YoutubeVideo
{
    public string? VideoId {get; set;}
    public string? VideoTitle {get; set;}
    public string? VideoThumbnail {get; set;}
    public string? VideoDescription {get; set;}
    public DateTimeOffset? VideoPublishedAt {get; set;}
}

// Trả về list video và trong đó có Token để phân trang tối 50video/1token
public class YoutubeVideoPagination
{
    public List<YoutubeVideo>? Videos {get; set;} = new List<YoutubeVideo>();
    public string? NextPageToken {get; set;}
    public string? PrevPageToken {get; set;}
}

// Trả về playlist
public class YoutubePlayList
{
    public string? PlayListId {get; set;}
    public string? PlayListTitle {get; set;}
    public string? PlayListDescription {get; set;}
    public string? PlayListThumbnail {get; set;}
    public DateTimeOffset? PlayListPublishedAt {get; set;}
}

// Trả về nội dung một playlist chứa các video
public class YoutubePlayListItem
{
    public string? PlayListId {get; set;}
    public string? PlayListTitle {get; set;}
    public List<YoutubeVideo>? Videos {get; set;}
}
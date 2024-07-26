namespace IdentityBlazorCoreAPI.Modules.Models;
public class YtbMovie 
{
    public string? YbTitle {get; set;}
    public string? YbLink {get; set;}
    public string? YbThumbnail {get; set;}
    public string? YbDescription {get; set;}
    public DateTimeOffset? YbPublishedAt {get; set;}
}

public class YtbResponse 
{
    public List<YtbMovie>? YbVideos {get; set;} = new List<YtbMovie>();
    public string? NextPageToken {get; set;}
    public string? PrevPageToken {get; set;}
}

public class YoutubeVideo
{
    public string? VideoId {get; set;}
    public string? VideoTitle {get; set;}
    public string? VideoThumbnail {get; set;}
    public string? VideoDescription {get; set;}
    public DateTimeOffset? VideoPublishedAt {get; set;}
}

public class YoutubeVideoPagination
{
    public List<YoutubeVideo> Videos {get; set;} = new List<YoutubeVideo>();
    public string? NextPageToken {get; set;}
    public string? PrevPageToken {get; set;}
}

public class YoutubePlayList
{
    public string? PlayListId {get; set;}
    public string? PlayListTitle {get; set;}
    public string? PlayListDescription {get; set;}
    public string? PlayListThumbnail {get; set;}
    public DateTimeOffset? PlayListPublishedAt {get; set;}
}

public class YoutubePlayListItem
{
    public string? PlayListId {get; set;}
    public string? PlayListTitle {get; set;}
    public List<YoutubeVideo>? Videos {get; set;}
}
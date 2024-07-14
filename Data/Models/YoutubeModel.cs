namespace IdentityBlazorCoreAPI.Data.Models;
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

public class YtbPlaylists 
{
    public string? plId {get; set;}
    public string? plTitle {get; set;}
    public string? plLink {get; set;}
    public string? plThumbnail {get; set;}
}

public class YtbPlaylistItem 
{
    public string? plItemId {get; set;}
    public string? plItemTitle {get; set;}
    public string? plItemLink {get; set;}
    public string? plItemThumbnail {get; set;}
}

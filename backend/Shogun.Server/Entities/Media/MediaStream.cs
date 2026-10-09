namespace Shogun.Server.Entities;

public enum StreamType
{
    Video,
    Audio,
    Subtitle
}

public enum SubtitleType
{
    Text,
    Image
}

public class MediaStream
{
    public int Id { get; set; }
    public int MediaFileId { get; set; }
    public required StreamType Type { get; set; }
    public string? Codec { get; set; }
    public string? Language { get; set; }
    public int? Channels { get; set; }
    public int Index { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public bool IsDefaultTrack { get; set; }
    public SubtitleType? SubtitleType { get; set; }
    public MediaFile MediaFile { get; set; } = null!;
}
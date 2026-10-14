namespace Shogun.Server.Entities;

/// <summary>
/// The kind of stream inside a media file.
/// </summary>
public enum StreamType
{
    Video,
    Audio,
    Subtitle
}

/// <summary>
/// Whether a subtitle stream is text or image-based.
/// </summary>
public enum SubtitleType
{
    Text,
    Image
}

/// <summary>
/// One stream inside a media file: a video, audio, or subtitle track.
/// </summary>
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
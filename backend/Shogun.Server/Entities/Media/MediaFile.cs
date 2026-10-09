namespace Shogun.Server.Entities;

public class MediaFile
{
    public int Id { get; set; }
    public int? MovieId { get; set; }
    public int? EpisodeId { get; set; }

    public int? TrackId { get; set; }
    public required string FilePath { get; set; }
    public long FileSize { get; set; }
    public string? Container { get; set; }
    public int? DurationSeconds { get; set; }
    public int? BitrateKbps { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public bool IsMissing { get; set; }
    public Movie? Movie { get; set; }
    public Episode? Episode { get; set; }
    public Track? Track { get; set; }
}
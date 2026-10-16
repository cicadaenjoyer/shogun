namespace Shogun.Server.Entities;

/// <summary>
/// A file on disk, as found by the scanner. Once identified it is linked to the
/// movie, episode, or track it contains; until then it has none.
/// </summary>
public class MediaFile
{
    public int Id { get; set; }
    public int LibraryPathId { get; set; }
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
    public DateTime? MissingSince { get; set; }
    public LibraryPath LibraryPath { get; set; } = null!;
    public Movie? Movie { get; set; }
    public Episode? Episode { get; set; }
    public Track? Track { get; set; }
}

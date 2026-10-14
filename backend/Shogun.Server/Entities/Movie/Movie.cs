namespace Shogun.Server.Entities;

/// <summary>
/// A movie in a library. This is the work itself; its files on disk are
/// <see cref="MediaFile"/> rows.
/// </summary>
public class Movie
{
    public int Id { get; set; }
    public int LibraryId { get; set; }
    public int? TmdbId { get; set; }
    public string? ImdbId { get; set; }
    public required string Title { get; set; }
    public required string SortTitle { get; set; }
    public int? Year { get; set; }
    public int? RuntimeSeconds { get; set; }
    public string? Overview { get; set; }
    public string? PosterImageKey { get; set; }
    public MetadataStatus MetadataStatus { get; set; } = MetadataStatus.Unmatched;
    public bool IsMetadataLocked { get; set; }
    public DateTime AddedAt { get; set; }
    public Library Library { get; set; } = null!;
    public ICollection<Genre> Genres { get; set; } = [];
}

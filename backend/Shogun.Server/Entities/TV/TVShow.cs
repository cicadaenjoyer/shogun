namespace Shogun.Server.Entities;

public class TvShow
{
    public int Id { get; set; }
    public int LibraryId { get; set; }
    public int? TmdbId { get; set; }
    public string? ImdbId { get; set; }
    public required string Title { get; set; }
    public required string SortTitle { get; set; }
    public int? Year { get; set; }
    public string? Overview { get; set; }
    public string? PosterImageKey { get; set; }
    public MetadataStatus MetadataStatus { get; set; } = MetadataStatus.Unmatched;
    public bool IsMetadataLocked { get; set; }
    public Library Library { get; set; } = null!;
    public ICollection<Genre> Genres { get; set; } = [];
}

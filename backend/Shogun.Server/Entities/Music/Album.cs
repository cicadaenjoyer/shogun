namespace Shogun.Server.Entities;

public class Album
{
    public int Id { get; set; }
    public int ArtistId { get; set; }
    public string? MusicBrainzId { get; set; }
    public required string Title { get; set; }
    public required string SortTitle { get; set; }
    public int? Year { get; set; }
    public string? CoverImageKey { get; set; }
    public MetadataStatus MetadataStatus { get; set; } = MetadataStatus.Unmatched;
    public bool IsMetadataLocked { get; set; }

    public Artist Artist { get; set; } = null!;
    public ICollection<Genre> Genres { get; set; } = [];
}

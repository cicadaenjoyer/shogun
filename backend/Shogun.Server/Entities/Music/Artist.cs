namespace Shogun.Server.Entities;

public class Artist
{
    public int Id { get; set; }
    public int LibraryId { get; set; }
    public string? MusicBrainzId { get; set; }
    public required string Name { get; set; }
    public required string SortName { get; set; }
    public string? Bio { get; set; }
    public string? ImageKey { get; set; }
    public bool IsMetadataLocked { get; set; }
    public Library Library { get; set; } = null!;
    public ICollection<Genre> Genres { get; set; } = [];
}

namespace Shogun.Server.Entities;

/// <summary>
/// One episode of a season. Its files on disk are <see cref="MediaFile"/> rows.
/// </summary>
public class Episode
{
    public int Id { get; set; }
    public int SeasonId { get; set; }
    public required string Title { get; set; }
    public int EpisodeNumber { get; set; }
    public int? RuntimeSeconds { get; set; }
    public string? Overview { get; set; }
    public bool IsMetadataLocked { get; set; }

    public Season Season { get; set; } = null!;
}

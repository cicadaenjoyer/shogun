namespace Shogun.Server.Entities;

/// <summary>
/// One season of a TV show.
/// </summary>
public class Season
{
    public int Id { get; set; }
    public int TvShowId { get; set; }
    public int SeasonNumber { get; set; }
    public string? Overview { get; set; }
    public string? PosterImageKey { get; set; }
    public bool IsMetadataLocked { get; set; }

    public TvShow TvShow { get; set; } = null!;
}

namespace Shogun.Server.Entities;

/// <summary>
/// One track on an album. Its files on disk are <see cref="MediaFile"/> rows.
/// The track's own artists are only filled in when they differ from the album artist.
/// </summary>
public class Track
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public int DiscNumber { get; set; } = 1;
    public string? MusicBrainzId { get; set; }
    public required string Title { get; set; }
    public int TrackNumber { get; set; }
    public int? DurationSeconds { get; set; }
    public bool IsMetadataLocked { get; set; }
    public string? ArtistCredit { get; set; }

    public Album Album { get; set; } = null!;
    public ICollection<Artist> Artists { get; set; } = [];
}

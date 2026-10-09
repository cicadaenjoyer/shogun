namespace Shogun.Server.Entities;

public class Track
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public required string Title { get; set; }
    public int TrackNumber { get; set; }
    public int Duration { get; set; }

    public Album Album { get; set; } = null!;
}
namespace Shogun.Models;

public class Track
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public required string Title { get; set; }
    public int TrackNumber { get; set; }
    public int Duration { get; set; }
    public required string FilePath { get; set; }
    public long FileSize { get; set; }

    public Album Album { get; set; } = null!;
}
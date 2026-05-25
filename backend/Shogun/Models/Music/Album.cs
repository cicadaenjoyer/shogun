namespace Shogun.Models;

public class Album
{
    public int Id { get; set; }
    public int ArtistId { get; set; }
    public required string Title { get; set; }
    public int Year { get; set; }
    public string? CoverUrl { get; set; }

    public Artist Artist { get; set; } = null!;
}
namespace Shogun.Models;

public class Episode
{
    public int Id { get; set; }
    public int SeasonId { get; set; }
    public required string Title { get; set; }
    public int EpisodeNumber { get; set; }
    public int Runtime { get; set; }
    public string? Overview { get; set; }
    public required string FilePath { get; set; }
    public long FileSize { get; set; }

    public Season Season { get; set; } = null!;
}
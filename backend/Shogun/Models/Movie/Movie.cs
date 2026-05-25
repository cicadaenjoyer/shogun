namespace Shogun.Models;

public class Movie
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int Year { get; set; }
    public string? Genre { get; set; }
    public int Runtime { get; set; }
    public string? Overview { get; set; }
    public string? PosterUrl { get; set; }
    public required string FilePath { get; set; }
    public long FileSize { get; set; }
    public DateTime AddedAt { get; set; }
}
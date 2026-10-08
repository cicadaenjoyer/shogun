namespace Shogun.Server.Entities;

public class TvShow
{
    public int Id { get; set; }
    public int LibraryId { get; set; }
    public required string Title { get; set; }
    public int Year { get; set; }
    public string? Genre { get; set; }
    public string? Overview { get; set; }
    public string? PosterUrl { get; set; }
    public Library Library { get; set; } = null!;
}
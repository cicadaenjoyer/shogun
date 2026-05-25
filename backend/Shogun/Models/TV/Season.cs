namespace Shogun.Models;

public class Season
{
    public int Id { get; set; }
    public int TvShowId { get; set; }
    public int SeasonNumber { get; set; }
    public string? Overview { get; set; }
    public string? PosterUrl { get; set; }

    public TvShow TvShow { get; set; } = null!;
}
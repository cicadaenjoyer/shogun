namespace Shogun.Server.Entities;

/// <summary>
/// A genre name, shared by movies, shows, artists, and albums.
/// </summary>
public class Genre
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

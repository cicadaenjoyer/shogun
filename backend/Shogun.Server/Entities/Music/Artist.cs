namespace Shogun.Server.Entities;

public class Artist
{
    public int Id { get; set; }
    public int LibraryId { get; set; }
    public required string Name { get; set; }
    public string? Genre { get; set; }
    public string? Bio { get; set; }
    public string? ImageUrl { get; set; }
    public Library Library { get; set; } = null!;
}
namespace Shogun.Models;

public class Artist
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Genre { get; set; }
    public string? Bio { get; set; }
    public string? ImageUrl { get; set; }
}
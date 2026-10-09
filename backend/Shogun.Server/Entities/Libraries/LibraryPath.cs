namespace Shogun.Server.Entities;

public class LibraryPath
{
    public int Id { get; set; }
    public int LibraryId { get; set; }
    public required string Path { get; set; }
    public Library Library { get; set; } = null!;

}
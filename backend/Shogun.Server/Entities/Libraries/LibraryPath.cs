namespace Shogun.Server.Entities;

/// <summary>
/// One folder on disk that belongs to a library. A folder belongs to exactly
/// one library.
/// </summary>
public class LibraryPath
{
    public int Id { get; set; }
    public int LibraryId { get; set; }
    public required string Path { get; set; }
    public Library Library { get; set; } = null!;

}

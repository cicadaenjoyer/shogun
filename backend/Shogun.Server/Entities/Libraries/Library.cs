namespace Shogun.Server.Entities;

public enum LibraryType
{
    Movie,
    TV,
    Music
}

public class Library
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Member> Members { get; set; } = [];
    public ICollection<LibraryPath> Folders { get; set; } = [];
    public LibraryType Type { get; set; }
    public DateTime? LastScannedAt { get; set; }
}
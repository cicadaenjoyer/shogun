namespace Shogun.Server.Entities;

/// <summary>
/// The kind of media a library holds.
/// </summary>
public enum LibraryType
{
    Movie,
    TV,
    Music
}

/// <summary>
/// A named collection of media of one type, built from one or more folders.
/// Members are given access one library at a time.
/// </summary>
public class Library
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Member> Members { get; set; } = [];
    public ICollection<LibraryPath> Folders { get; set; } = [];
    public required LibraryType Type { get; set; }
    public DateTime? LastScannedAt { get; set; }
}
namespace Shogun.Server.Entities;

public enum MemberRole
{
    Owner,
    Member
}
public enum MemberStatus
{
    Active,
    Removed
}

public class Member
{
    public int Id { get; set; }
    public required string FirebaseUid { get; set; }
    public required string DisplayName { get; set; }
    public string? Avatar { get; set; }
    public required MemberRole Role { get; set; }
    public required MemberStatus Status { get; set; }
    public DateTime? LastSeenAt { get; set; }
    // null in this case means "No Limit"
    public int? MaxStreamingBitrateKbps { get; set; }
    public bool DownloadsAllowed { get; set; } = true;
    public ICollection<Library> AllowedLibraries { get; set; } = [];
}
namespace Shogun.Server.Entities;

/// <summary>
/// A member's role on this Server.
/// </summary>
public enum MemberRole
{
    Owner,
    Member
}
/// <summary>
/// Whether a member still belongs to the group.
/// </summary>
public enum MemberStatus
{
    Active,
    Removed
}

/// <summary>
/// The Server's local copy of a group member, plus what that member may do
/// here. Accounts live in Firebase, so no email or password is stored.
/// </summary>
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
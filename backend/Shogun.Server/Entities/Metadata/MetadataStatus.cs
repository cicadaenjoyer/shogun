namespace Shogun.Server.Entities;

/// <summary>
/// How an item's details were filled in: not matched online yet, matched
/// automatically, or edited by the owner.
/// </summary>
public enum MetadataStatus
{
    Unmatched,
    Matched,
    ManuallyEdited
}

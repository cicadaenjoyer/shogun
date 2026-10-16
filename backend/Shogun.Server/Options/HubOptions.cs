using System.ComponentModel.DataAnnotations;

namespace Shogun.Server.Options;

/// <summary>
/// Settings the Server needs in order to talk to the Shogun Hub (the shared
/// Firebase project) and to verify the server passes the Hub issues.
/// Bound to the "Hub" section of configuration in Program.cs.
/// </summary>
public class HubOptions
{
    /// <summary>
    /// The name of the configuration section these settings are read from.
    /// </summary>
    public const string SectionName = "Hub";

    /// <summary>
    /// Base address of the Hub's Cloud Functions, used for register, claim,
    /// heartbeat, and member-list calls.
    /// </summary>
    [Required(ErrorMessage = "The Hub's Cloud Functions base address is required.")]
    [Url(ErrorMessage = "The Hub's Cloud Functions base address must be a URL.")]
    public string FunctionsBaseAddress { get; set; } = string.Empty;

    /// <summary>
    /// The "iss" (issuer) claim every server pass must carry. A pass whose
    /// issuer is anything else is rejected, even if its signature checks out.
    /// </summary>
    [Required(ErrorMessage = "The server pass issuer is required.")]
    public string ServerPassIssuer { get; set; } = string.Empty;

    /// <summary>
    /// Address of the Hub's JWKS document: the public half of the key the Hub
    /// signs server passes with.
    /// </summary>
    [Required(ErrorMessage = "The JWKS address is required.")]
    [Url(ErrorMessage = "The JWKS address must be a URL.")]
    public string JwksAddress { get; set; } = string.Empty;

    /// <summary>
    /// How often to send a heartbeat to the Hub. Each heartbeat is a Firestore
    /// write, and writes cost money above the free quota, so this is deliberately
    /// not "every few seconds". The Hub treats a server as offline after roughly
    /// three missed heartbeats.
    /// </summary>
    [Range(30, 600, ErrorMessage = "The heartbeat interval must be between 30 and 600 seconds.")]
    public int HeartbeatIntervalSeconds { get; set; } = 120;
}

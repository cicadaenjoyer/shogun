using System.ComponentModel.DataAnnotations;

namespace Shogun.Server.Options;

/// <summary>
/// Settings that describe this server to the people using it.
/// Bound to the "Server" section of configuration in Program.cs.
/// </summary>
public class ServerOptions
{
    /// <summary>
    /// The name of the configuration section these settings are read from.
    /// </summary>
    public const string SectionName = "Server";

    /// <summary>
    /// A friendly label chosen by the owner, such as "Living Room". The health
    /// endpoint shows it to anyone, and the Hub shows it in members' server
    /// lists, so it must never default to the machine's hostname.
    /// </summary>
    [Required(ErrorMessage = "The server name can't be empty.")]
    [StringLength(50, ErrorMessage = "The server name must be 50 characters or fewer.")]
    public string Name { get; set; } = "Shogun Server";
}

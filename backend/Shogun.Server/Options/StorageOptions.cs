using System.ComponentModel.DataAnnotations;

namespace Shogun.Server.Options;

/// <summary>
/// Where the Server keeps its own files on the machine it runs on. Bound to the "Storage" section 
/// of configuration in Program.cs.
/// </summary>
/// <remarks>
/// These are the Server's <em>working</em> folders, not the owner's media. Media folder paths
/// belong to a Library, because there can be several of them and they are edited from the app 
/// rather than a config file.
/// </remarks>
public class StorageOptions
{
    /// <summary>
    /// The name of the configuration section these settings are read from.
    /// </summary>
    public const string SectionName = "Storage";

    /// <summary>
    /// The Server's own data folder: the SQLite database, the generated server
    /// ID, and the Hub credential received when the server is claimed.
    /// This is the one folder an owner must back up.
    /// </summary>
    [Required(ErrorMessage = "The data folder is required.")]
    public string DataFolder { get; set; } = string.Empty;

    /// <summary>
    /// Where downloaded and resized artwork is cached. Safe to delete: the
    /// Server refetches anything missing.
    /// </summary>
    [Required(ErrorMessage = "The image cache folder is required.")]
    public string ImageCacheFolder { get; set; } = string.Empty;

    /// <summary>
    /// Scratch space for in-progress transcodes. Everything here is
    /// disposable and is cleaned up when a stream ends.
    /// </summary>
    [Required(ErrorMessage = "The transcode temp folder is required.")]
    public string TranscodeTempFolder { get; set; } = string.Empty;
}

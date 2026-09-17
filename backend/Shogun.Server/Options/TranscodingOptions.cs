using System.ComponentModel.DataAnnotations;

namespace Shogun.Server.Options;

/// <summary>
/// Settings the server needs for transcoding.
/// </summary>
public class TranscodingOptions
{
    /// <summary>
    /// The name of the configuration section these settings are read from.
    /// </summary>
    public const string SectionName = "Transcoding";

    /// <summary>
    /// The maximum number of simultaneous transcodes the server can serve.
    /// </summary>
    [Range(1, 10, ErrorMessage = "The max number of simultaneous transcodes must be between 1 and 10.")]
    public int MaxSimultaneousTranscodes { get; set; } = 5;
}

using System.ComponentModel.DataAnnotations;

namespace Shogun.Server.Options;

/// <summary>
/// Settings describing the Shogun web app, so the Server knows which browsers
/// are allowed to call its API.
/// Bound to the "Web" section of configuration in Program.cs.
/// </summary>
public class WebOptions : IValidatableObject
{
    /// <summary>
    /// The name of the configuration section these settings are read from.
    /// </summary>
    public const string SectionName = "Web";

    /// <summary>
    /// The browser origins allowed to call this Server's API, enforced by CORS.
    /// </summary>
    [MinLength(1, ErrorMessage = "At least one allowed web origin is required.")]
    public string[] AllowedOrigins { get; set; } = [];

    /// <summary>
    /// Rules that a single attribute can't express. Implementing
    /// IValidatableObject is enough for ValidateDataAnnotations() to call this
    /// automatically, after the per-property attributes have run.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var origin in AllowedOrigins)
        {
            if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                yield return new ValidationResult(
                    $"'{origin}' is not a valid web origin. Expected something like " +
                    "https://shogun.web.app or http://localhost:5173.",
                    [nameof(AllowedOrigins)]);
                continue;
            }

            // GetLeftPart(Authority) returns exactly "scheme://host:port", so if
            // it differs from what was configured, something extra was written:
            // a trailing slash, a path, or a query string.
            var authorityOnly = uri.GetLeftPart(UriPartial.Authority);
            if (!string.Equals(origin, authorityOnly, StringComparison.Ordinal))
            {
                yield return new ValidationResult(
                    $"'{origin}' must be an origin only, with no trailing slash or path. " +
                    $"Use '{authorityOnly}'.",
                    [nameof(AllowedOrigins)]);
            }
        }
    }
}

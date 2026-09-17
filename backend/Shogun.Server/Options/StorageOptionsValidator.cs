using Microsoft.Extensions.Options;

namespace Shogun.Server.Options;

/// <summary>
/// Checks the storage folders at startup. These rules can't be written as
/// attributes on <see cref="StorageOptions"/>, because they involve the file
/// system rather than the value alone.
/// </summary>
public class StorageOptionsValidator : IValidateOptions<StorageOptions>
{
    public ValidateOptionsResult Validate(string? name, StorageOptions options)
    {
        var failures = new List<string>();

        Check(nameof(options.DataFolder), options.DataFolder);
        Check(nameof(options.ImageCacheFolder), options.ImageCacheFolder);
        Check(nameof(options.TranscodeTempFolder), options.TranscodeTempFolder);

        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;

        void Check(string settingName, string path)
        {
            // An empty value is already reported by [Required]; don't say it twice.
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            // A relative path is resolved against the current working directory,
            // which differs between "dotnet run" and a systemd service. That makes
            // the database appear to vanish when the app is deployed, so insist on
            // an absolute path.
            if (!Path.IsPathRooted(path))
            {
                failures.Add(
                    $"Setting '{StorageOptions.SectionName}:{settingName}' must be an absolute " +
                    $"path, but was '{path}'.");
                return;
            }

            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                failures.Add(
                    $"Setting '{StorageOptions.SectionName}:{settingName}' ('{path}') could not " +
                    $"be created: {ex.Message}");
            }
        }
    }
}

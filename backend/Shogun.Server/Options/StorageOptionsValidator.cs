using Microsoft.Extensions.Options;

namespace Shogun.Server.Options;

/// <summary>
/// Checks the storage folders at startup. These rules can't be written as
/// attributes on <see cref="StorageOptions"/>, because they involve the file
/// system rather than the value alone.
/// </summary>
public class StorageOptionsValidator : IValidateOptions<StorageOptions>
{
    // Read, write, and enter for the Server's own user; nothing for anyone else (chmod 700)
    private const UnixFileMode OwnerOnly =
        UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute;

    // If any of these is set, other users on the machine can get into the folder
    private const UnixFileMode GroupOrOthers =
        UnixFileMode.GroupRead | UnixFileMode.GroupWrite | UnixFileMode.GroupExecute |
        UnixFileMode.OtherRead | UnixFileMode.OtherWrite | UnixFileMode.OtherExecute;

    private readonly ILogger<StorageOptionsValidator> _logger;

    public StorageOptionsValidator(ILogger<StorageOptionsValidator> logger)
    {
        _logger = logger;
    }

    public ValidateOptionsResult Validate(string? name, StorageOptions options)
    {
        var failures = new List<string>();

        // Only the data folder will hold secrets (the server credential and signing key)
        Check(nameof(options.DataFolder), options.DataFolder, ownerOnly: true);
        Check(nameof(options.ImageCacheFolder), options.ImageCacheFolder, ownerOnly: false);
        Check(nameof(options.TranscodeTempFolder), options.TranscodeTempFolder, ownerOnly: false);

        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;

        void Check(string settingName, string path, bool ownerOnly)
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
                if (!ownerOnly || OperatingSystem.IsWindows())
                {
                    // Unix permissions don't exist on Windows, so there's nothing to set
                    Directory.CreateDirectory(path);
                }
                else if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path, OwnerOnly);
                }
                else
                {
                    // An existing folder keeps its permissions: the owner may have
                    // loosened them on purpose, so warn rather than change them
                    var mode = File.GetUnixFileMode(path);

                    if ((mode & GroupOrOthers) != 0)
                    {
                        _logger.LogWarning(
                            "The data folder {Path} can be accessed by other users on this machine " +
                            "(permissions {Permissions}). Unless that's intended, restrict it with chmod 700",
                            path, Convert.ToString((int)mode, 8));
                    }
                }
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

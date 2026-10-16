using Microsoft.Extensions.Options;
using Shogun.Server.Options;
using YamlDotNet.Core.Tokens;

namespace Shogun.Server.Services;

/// <summary>
/// This server's permanent ID, kept in a file in the data folder. The health
/// endpoint reports it, and the Hub will use it to recognize this server.
/// Loaded once at startup, so a data folder that can't be written to stops
/// the Server before it accepts any request.
/// </summary>
public class ServerIdentity
{
    // The file inside the data folder that holds this server's ID.
    private const string ServerIdFileName = "server-id";
    public string Id { get; }

    public ServerIdentity(IOptions<StorageOptions> storageOptions)
    {
        Id = LoadOrCreateServerId(storageOptions.Value.DataFolder);
    }


    // Reads the server ID from the data folder. On the very first start there
    // is no file yet, so a random ID is generated and saved, and every later
    // start reads that same value back. A file that exists but doesn't hold a
    // valid ID stops the Server instead of being replaced: a new ID would cut
    // the server off from its Hub entry and lock out every member.
    private static string LoadOrCreateServerId(string dataFolder)
    {
        var path = Path.Combine(dataFolder, ServerIdFileName);

        // File doesn't exist, create it
        if (!File.Exists(path))
        {
            var newId = Guid.NewGuid().ToString();

            var options = new FileStreamOptions
            {
                Mode = FileMode.Create,
                Access = FileAccess.Write,
            };

            if (!OperatingSystem.IsWindows())
            {
                options.UnixCreateMode = UnixFileMode.UserRead | UnixFileMode.UserWrite;
            }

            using var stream = new FileStream(path, options);
            using var writer = new StreamWriter(stream);

            writer.Write(newId);
            return newId;
        }
        // File does exist and is healthy
        else if (Guid.TryParse(File.ReadAllText(path).Trim(), out var existingId))
        {
            return existingId.ToString();
        }
        // File does exist but is corrupted
        else
        {
            throw new InvalidOperationException(
                $"The server ID file '{path}' doesn't contain a valid ID. Restore it from a " +
                "backup to keep this server's identity, or delete it to create a new one " +
                "(members will need to be re-invited).");
        }
    }
}
using Microsoft.Extensions.Options;
using Shogun.Server.Options;

namespace Shogun.Server.Services;

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
    // start reads that same value back.
    private static string LoadOrCreateServerId(string dataFolder)
    {
        var path = Path.Combine(dataFolder, ServerIdFileName);

        if (File.Exists(path) && Guid.TryParse(File.ReadAllText(path).Trim(), out var existingId))
        {
            return existingId.ToString();
        }

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
}
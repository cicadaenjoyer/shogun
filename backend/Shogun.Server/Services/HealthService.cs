using System.Reflection;
using Microsoft.Extensions.Options;
using Shogun.Server.DTO;
using Shogun.Server.Options;

namespace Shogun.Server.Services;

/// <summary>
/// Works out the values the health endpoint reports: the Server's version,
/// name, and ID.
/// </summary>
public class HealthService
{
    // The file inside the data folder that holds this server's ID.
    private const string ServerIdFileName = "server-id";

    private readonly string _version;
    private readonly string _serverId;
    
    public HealthService(IOptions<StorageOptions> storageOptions)
    {
        var versionAttr = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        _version = versionAttr?.InformationalVersion.Split('+')[0] ?? "unknown";

        _serverId = LoadOrCreateServerId(storageOptions.Value.DataFolder);
    }

    /// <summary>
    /// Builds the response for the health endpoint from the values worked out
    /// at startup.
    /// </summary>
    public HealthResponse GetHealth()
    {
        return new HealthResponse("ok", _version, Environment.MachineName, _serverId);
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
        File.WriteAllText(path, newId);
        return newId;
    }
}

using System.Reflection;
using Shogun.Server.DTO;

namespace Shogun.Server.Services;

/// <summary>
/// Works out the values the health endpoint reports: the Server's version,
/// name, and ID.
/// </summary>
public class HealthService
{
    private readonly string _version;
    private readonly ServerIdentity _serverIdentity;
    
    public HealthService(ServerIdentity serverIdentity)
    {
        _serverIdentity = serverIdentity;

        var versionAttr = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        _version = versionAttr?.InformationalVersion.Split('+')[0] ?? "unknown";
    }

    /// <summary>
    /// Builds the response for the health endpoint from the values worked out
    /// at startup.
    /// </summary>
    public HealthResponse GetHealth()
    {
        return new HealthResponse("ok", _version, Environment.MachineName, _serverIdentity.Id);
    }
}

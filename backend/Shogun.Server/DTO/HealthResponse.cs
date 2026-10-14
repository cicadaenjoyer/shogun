namespace Shogun.Server.DTO;

/// <summary>
/// What the health endpoint sends back.
/// </summary>
/// <param name="Status">Always "ok". If the server can answer at all, it is up.</param>
/// <param name="Version">The Server's version.</param>
/// <param name="Name">The Server's display name (currently the machine's name).</param>
/// <param name="Id">
/// This server's unique ID, generated on first start and the same for the
/// life of the install.
/// </param>
public record HealthResponse(string Status, string Version, string Name, string Id);
namespace Shogun.Server.DTO;

/// <summary>
/// What the health endpoint sends back. Anyone on the internet can call it,
/// so every field must have a reason to be public. Nothing about the library,
/// the members, the file system, or the hardware belongs here.
/// </summary>
/// <param name="Status">
/// Always "ok". If the server can answer at all, it is up. Public because it's
/// how the web app knows it reached a Shogun server at all.
/// </param>
/// <param name="Version">
/// The Server's version. Public so the web app can warn about an outdated
/// Server. It also tells an attacker which version to look up; that's an
/// accepted trade.
/// </param>
/// <param name="Name">
/// The owner's friendly label for this server, such as "Living Room". Never the
/// machine's hostname, which would reveal a detail of the owner's network.
/// </param>
/// <param name="Id">
/// This server's unique ID, generated on first start and the same for the
/// life of the install. Public so the web app can check it reached the
/// <em>right</em> server. It's an identifier, not a secret: nothing on the
/// Server or the Hub may accept the ID alone as proof of anything.
/// </param>
public record HealthResponse(string Status, string Version, string Name, string Id);

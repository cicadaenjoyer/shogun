using Shogun.Server.Options;
using Shogun.Server.Services;
using Shogun.Server.Tests.Support;
using Shouldly;

namespace Shogun.Server.Tests.Unit;

/// <summary>
/// Unit tests for <see cref="HealthService"/>, using a temporary data folder.
/// </summary>
public class HealthServiceTests : IDisposable
{
    private readonly TempFolder _dataFolder = new();

    public void Dispose()
    {
        _dataFolder.Dispose();
    }

    private ServerIdentity CreateIdentity()
    {
        var storage = Microsoft.Extensions.Options.Options.Create(
            new StorageOptions { DataFolder = _dataFolder.Path });
        var server = Microsoft.Extensions.Options.Options.Create(
            new ServerOptions { Name = "Living Room" });

        return new ServerIdentity(storage, server);
    }

    [Fact]
    public void GetHealth_ReportsStatusVersionNameAndId()
    {
        var identity = CreateIdentity();

        var health = new HealthService(identity).GetHealth();

        health.Status.ShouldBe("ok");
        health.Name.ShouldBe("Living Room");
        // The hostname is a detail of the owner's network; it must never be public
        health.Name.ShouldNotBe(Environment.MachineName);
        health.Version.ShouldNotBe("unknown");
        // The build adds "+<commit>" to the version; the service should have removed it
        health.Version.ShouldNotContain("+");
        health.Id.ShouldBe(identity.Id);
    }
}

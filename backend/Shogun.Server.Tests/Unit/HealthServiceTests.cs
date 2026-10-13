using Shogun.Server.Options;
using Shogun.Server.Services;
using Shogun.Server.Tests.Support;
using Shouldly;

namespace Shogun.Server.Tests.Unit;

public class HealthServiceTests : IDisposable
{
    private readonly TempFolder _dataFolder = new();

    public void Dispose()
    {
        _dataFolder.Dispose();
    }

    private HealthService CreateService()
    {
        var options = Microsoft.Extensions.Options.Options.Create(
            new StorageOptions { DataFolder = _dataFolder.Path });

        return new HealthService(options);
    }

    private string ServerIdFile => Path.Combine(_dataFolder.Path, "server-id");

    [Fact]
    public void GetHealth_ReportsStatusVersionAndName()
    {
        var health = CreateService().GetHealth();

        health.Status.ShouldBe("ok");
        health.Name.ShouldBe(Environment.MachineName);
        health.Version.ShouldNotBe("unknown");
        // The build adds "+<commit>" to the version; the service should have removed it
        health.Version.ShouldNotContain("+");
    }

    [Fact]
    public void FirstStart_CreatesAServerIdAndSavesIt()
    {
        var health = CreateService().GetHealth();

        Guid.TryParse(health.Id, out _).ShouldBeTrue();
        File.ReadAllText(ServerIdFile).ShouldBe(health.Id);
    }

    [Fact]
    public void LaterStarts_KeepTheSameServerId()
    {
        var first = CreateService().GetHealth();
        var second = CreateService().GetHealth();

        second.Id.ShouldBe(first.Id);
    }

    [Fact]
    public void UnreadableServerIdFile_IsReplacedWithANewId()
    {
        File.WriteAllText(ServerIdFile, "not a valid id");

        var health = CreateService().GetHealth();

        Guid.TryParse(health.Id, out _).ShouldBeTrue();
        File.ReadAllText(ServerIdFile).ShouldBe(health.Id);
    }
}

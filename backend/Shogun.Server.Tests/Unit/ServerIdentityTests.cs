using Shogun.Server.Options;
using Shogun.Server.Services;
using Shogun.Server.Tests.Support;
using Shouldly;

namespace Shogun.Server.Tests.Unit;

/// <summary>
/// Unit tests for <see cref="ServerIdentity"/>, using a temporary data folder.
/// </summary>
public class ServerIdentityTests : IDisposable
{
    private readonly TempFolder _dataFolder = new();

    public void Dispose()
    {
        _dataFolder.Dispose();
    }

    private ServerIdentity CreateIdentity()
    {
        var options = Microsoft.Extensions.Options.Options.Create(
            new StorageOptions { DataFolder = _dataFolder.Path });

        return new ServerIdentity(options);
    }

    private string ServerIdFile => Path.Combine(_dataFolder.Path, "server-id");

    [Fact]
    public void FirstStart_CreatesAServerIdAndSavesIt()
    {
        var identity = CreateIdentity();

        Guid.TryParse(identity.Id, out _).ShouldBeTrue();
        File.ReadAllText(ServerIdFile).ShouldBe(identity.Id);
    }

    [Fact]
    public void LaterStarts_KeepTheSameServerId()
    {
        var first = CreateIdentity();
        var second = CreateIdentity();

        second.Id.ShouldBe(first.Id);
    }

    [Fact]
    public void FirstStart_SavesTheServerIdForTheOwnerOnly()
    {
        // Unix permissions don't exist on Windows
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        CreateIdentity();

        File.GetUnixFileMode(ServerIdFile).ShouldBe(UnixFileMode.UserRead | UnixFileMode.UserWrite);
    }

    [Fact]
    public void CorruptServerIdFile_RefusesToStartAndKeepsTheFile()
    {
        File.WriteAllText(ServerIdFile, "not a valid id");

        var error = Should.Throw<InvalidOperationException>(() => CreateIdentity());

        error.Message.ShouldContain("server-id");
        // Nothing was overwritten, so the owner can still inspect or restore it
        File.ReadAllText(ServerIdFile).ShouldBe("not a valid id");
    }
}

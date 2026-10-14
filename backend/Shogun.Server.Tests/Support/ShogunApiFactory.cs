using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Shogun.Server.Options;

namespace Shogun.Server.Tests.Support;

/// <summary>
/// Starts the whole Server in memory for integration tests. Its storage
/// folders point at a temporary folder, so the database and server ID it
/// creates are thrown away afterwards.
/// </summary>
public sealed class ShogunApiFactory : WebApplicationFactory<Program>
{
    private readonly TempFolder _folder = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Not "Development", so a developer's local settings file is never loaded
        builder.UseEnvironment("Testing");

        builder.UseSetting($"{StorageOptions.SectionName}:{nameof(StorageOptions.DataFolder)}",
            Path.Combine(_folder.Path, "data"));
        builder.UseSetting($"{StorageOptions.SectionName}:{nameof(StorageOptions.ImageCacheFolder)}",
            Path.Combine(_folder.Path, "images"));
        builder.UseSetting($"{StorageOptions.SectionName}:{nameof(StorageOptions.TranscodeTempFolder)}",
            Path.Combine(_folder.Path, "transcode"));
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            SqliteConnection.ClearAllPools();
            _folder.Dispose();
        }
    }
}

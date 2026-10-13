using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Shogun.Server.Data;

namespace Shogun.Server.Tests.Support;

/// <summary>
/// A real SQLite database file in a temporary folder, built by applying the
/// migrations exactly as the Server does at startup.
/// </summary>
public sealed class TestDatabase : IDisposable
{
    private readonly TempFolder _folder = new();
    private readonly DbContextOptions<ShogunDbContext> _options;

    public TestDatabase()
    {
        var connection = new SqliteConnectionStringBuilder
        {
            DataSource = Path.Combine(_folder.Path, "test.db")
        };

        _options = new DbContextOptionsBuilder<ShogunDbContext>()
            .UseSqlite(connection.ToString())
            .Options;

        using var db = CreateContext();
        db.Database.Migrate();
    }

    /// <summary>
    /// Opens a new context on the database. Use one to save and a second to
    /// read back, so the read really comes from the file and not from memory.
    /// </summary>
    public ShogunDbContext CreateContext()
    {
        return new ShogunDbContext(_options);
    }

    public void Dispose()
    {
        // Connections are kept open in a pool, which would block deleting the file on Windows
        SqliteConnection.ClearAllPools();
        _folder.Dispose();
    }
}

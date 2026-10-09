using Microsoft.EntityFrameworkCore;
using Shogun.Server.Entities;

namespace Shogun.Server.Data;

public class ShogunDbContext : DbContext
{
    public ShogunDbContext(DbContextOptions<ShogunDbContext> options)
        : base(options)
    {
    }

    // Tables

    // Library
    public DbSet<Library> Libraries { get; set; } = null!;
    public DbSet<LibraryPath> LibraryPaths { get; set; } = null!;

    // Media
    public DbSet<MediaFile> MediaFiles { get; set; } = null!;
    public DbSet<MediaStream> MediaStreams { get; set; } = null!;

    // User
    public DbSet<Member> Members { get; set; } = null!;
    public DbSet<WatchHistory> WatchHistories { get; set; } = null!;

    // TV
    public DbSet<TvShow> TvShows { get; set; } = null!;
    public DbSet<Season> Seasons { get; set; } = null!;
    public DbSet<Episode> Episodes { get; set; } = null!;

    // Music
    public DbSet<Artist> Artists { get; set; } = null!;
    public DbSet<Album> Albums { get; set; } = null!;
    public DbSet<Track> Tracks { get; set; } = null!;

    // Movie
    public DbSet<Movie> Movies { get; set; } = null!;

    #region Required
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // loads every configuration class automatically
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShogunDbContext).Assembly);
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // converts all enums to their String values
        configurationBuilder
            .Properties<Enum>()
            .HaveConversion<string>();
    }
    #endregion
}
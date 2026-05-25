using Microsoft.EntityFrameworkCore;
using Shogun.Models;

namespace Shogun.Data;

public class ShogunDbContext : DbContext
{
    public ShogunDbContext(DbContextOptions<ShogunDbContext> options)
        : base(options)
    {
    }

    // Tables

    // User
    public DbSet<User> Users { get; set; } = null!;
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShogunDbContext).Assembly);
    }
    #endregion
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Entities;

namespace Shogun.Server.Data.Configurations;

/// <summary>
/// Database rules for episodes: one episode number per season, and deletion
/// along with the season.
/// </summary>
public class EpisodeEntityConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.Property(e => e.Title)
            .HasMaxLength(255);

        builder.HasIndex(e => new { e.SeasonId, e.EpisodeNumber })
            .IsUnique();

        builder.HasOne(e => e.Season)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
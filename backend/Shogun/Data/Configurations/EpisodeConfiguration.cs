using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Models;

namespace Shogun.Data.Configurations;

public class EpisodeEntityConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.Property(e => e.Title)
            .HasMaxLength(255);
        
        builder.HasIndex(e => e.FilePath)
            .IsUnique();

        builder.HasIndex(m => new { m.EpisodeNumber, m.SeasonId })
            .IsUnique();
    }
}
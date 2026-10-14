using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Entities;

namespace Shogun.Server.Data.Configurations;

/// <summary>
/// Database rules for seasons: one season number per show, and deletion along
/// with the show.
/// </summary>
public class SeasonEntityConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.HasIndex(s => new { s.TvShowId, s.SeasonNumber })
            .IsUnique();

        builder.HasOne(s => s.TvShow)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
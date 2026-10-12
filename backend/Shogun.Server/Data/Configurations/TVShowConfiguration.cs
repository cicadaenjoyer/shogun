using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Entities;

namespace Shogun.Server.Data.Configurations;

public class TVShowEntityConfiguration : IEntityTypeConfiguration<TvShow>
{
    public void Configure(EntityTypeBuilder<TvShow> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(255);

        builder.Property(t => t.SortTitle)
            .HasMaxLength(255);

        builder.Property(t => t.ImdbId)
            .HasMaxLength(16);

        builder.HasIndex(t => t.TmdbId);

        builder.HasIndex(t => t.ImdbId);

        builder.HasMany(t => t.Genres)
            .WithMany();
    }
}

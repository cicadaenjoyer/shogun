using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Entities;

namespace Shogun.Server.Data.Configurations;

/// <summary>
/// Database rules for TV shows: column lengths, external ID indexes, genre
/// links, and deletion along with the library.
/// </summary>
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

        builder.HasOne(t => t.Library)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}

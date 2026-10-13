using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Entities;

namespace Shogun.Server.Data.Configurations;

public class MovieEntityConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.Property(m => m.Title)
            .HasMaxLength(255);

        builder.Property(m => m.SortTitle)
            .HasMaxLength(255);

        builder.Property(m => m.ImdbId)
            .HasMaxLength(16);

        builder.Property(m => m.AddedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(m => m.TmdbId);

        builder.HasIndex(m => m.ImdbId);

        builder.HasMany(m => m.Genres)
            .WithMany();

        builder.HasOne(m => m.Library)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}

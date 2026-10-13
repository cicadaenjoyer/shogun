using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Entities;

namespace Shogun.Server.Data.Configurations;

public class AlbumEntityConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.Property(a => a.Title)
            .HasMaxLength(255);

        builder.Property(a => a.SortTitle)
            .HasMaxLength(255);

        builder.Property(a => a.MusicBrainzId)
            .HasMaxLength(36);

        builder.HasIndex(a => new { a.ArtistId, a.Title });

        builder.HasIndex(a => a.MusicBrainzId);

        builder.HasMany(a => a.Genres)
            .WithMany();

        builder.HasOne(a => a.Artist)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}

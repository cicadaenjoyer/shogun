using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Entities;

namespace Shogun.Server.Data.Configurations;

/// <summary>
/// Database rules for tracks: one track number per disc of an album, artist
/// links, and deletion along with the album.
/// </summary>
public class TrackEntityConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(255);

        builder.Property(t => t.MusicBrainzId)
            .HasMaxLength(36);

        builder.Property(t => t.ArtistCredit)
            .HasMaxLength(255);

        builder.HasIndex(t => new { t.AlbumId, t.DiscNumber, t.TrackNumber })
            .IsUnique();

        builder.HasIndex(t => t.MusicBrainzId);

        builder.HasMany(t => t.Artists)
            .WithMany();

        builder.HasOne(t => t.Album)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}

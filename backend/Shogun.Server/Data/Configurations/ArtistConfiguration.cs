using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Entities;

namespace Shogun.Server.Data.Configurations;

public class ArtistEntityConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.Property(a => a.Name)
            .HasMaxLength(255);

        builder.Property(a => a.SortName)
            .HasMaxLength(255);

        builder.Property(a => a.MusicBrainzId)
            .HasMaxLength(36);

        builder.HasIndex(a => a.MusicBrainzId);

        builder.HasMany(a => a.Genres)
            .WithMany();
    }
}

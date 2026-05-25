using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Models;

namespace Shogun.Data.Configurations;

public class TrackEntityConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(255);
        
        builder.HasIndex(t => t.FilePath)
            .IsUnique();

        builder.HasIndex(t => new { t.AlbumId, t.TrackNumber })
            .IsUnique();
    }
}
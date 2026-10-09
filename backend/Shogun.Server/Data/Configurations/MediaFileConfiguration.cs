using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Entities;

namespace Shogun.Server.Data.Configurations;

public class MediaFileEntityConfiguration : IEntityTypeConfiguration<MediaFile>
{
    public void Configure(EntityTypeBuilder<MediaFile> builder)
    {
        builder.HasIndex(f => f.FilePath)
            .IsUnique();

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_MediaFiles_AtMostOneWork",
            "(\"MovieId\" IS NOT NULL) + (\"EpisodeId\" IS NOT NULL) + (\"TrackId\" IS NOT NULL) <= 1"));
    }
}

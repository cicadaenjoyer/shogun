using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Entities;

namespace Shogun.Server.Data.Configurations;

public class MediaStreamEntityConfiguration : IEntityTypeConfiguration<MediaStream>
{
    public void Configure(EntityTypeBuilder<MediaStream> builder)
    {
        builder.HasOne(s => s.MediaFile)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}

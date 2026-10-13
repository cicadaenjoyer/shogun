using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Entities;

namespace Shogun.Server.Data.Configurations;

public class LibraryPathEntityConfiguration : IEntityTypeConfiguration<LibraryPath>
{
    public void Configure(EntityTypeBuilder<LibraryPath> builder)
    {
        builder.HasIndex(lp => lp.Path)
            .IsUnique();

        builder.HasOne(lp => lp.Library)
            .WithMany(l => l.Folders)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
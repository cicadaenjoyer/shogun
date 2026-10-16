using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Entities;

namespace Shogun.Server.Data.Configurations;

/// <summary>
/// Database rules for library folders: a path is used by one library only, and
/// folders are deleted along with their library.
/// </summary>
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

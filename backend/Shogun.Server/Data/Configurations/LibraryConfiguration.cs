using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Entities;

namespace Shogun.Server.Data.Configurations;

public class LibraryEntityConfiguration : IEntityTypeConfiguration<Library>
{
    public void Configure(EntityTypeBuilder<Library> builder)
    {
        builder.Property(l => l.Name)
            .HasMaxLength(255);
    }
}

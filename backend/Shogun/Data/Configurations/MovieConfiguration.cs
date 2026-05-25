using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Models;

namespace Shogun.Data.Configurations;

public class MovieEntityConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.Property(m => m.Title)
            .HasMaxLength(255);
        
        builder.HasIndex(m => m.FilePath)
            .IsUnique();

        builder.Property(m => m.AddedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Models;

namespace Shogun.Server.Data.Configurations;

public class TVShowEntityConfiguration : IEntityTypeConfiguration<TvShow>
{
    public void Configure(EntityTypeBuilder<TvShow> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(255);
    }
}
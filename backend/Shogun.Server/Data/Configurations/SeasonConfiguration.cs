using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Models;

namespace Shogun.Server.Data.Configurations;

public class SeasonEntityConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.HasIndex(s => new {s.SeasonNumber, s.TvShowId})
            .IsUnique();
    }
}
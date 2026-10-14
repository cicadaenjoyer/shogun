using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Entities;

namespace Shogun.Server.Data.Configurations;

/// <summary>
/// Database rules for playback progress: one row per member and item, exactly
/// one linked item, and deletion along with the member or the item.
/// </summary>
public class PlaybackStateEntityConfiguration : IEntityTypeConfiguration<PlaybackState>
{
    public void Configure(EntityTypeBuilder<PlaybackState> builder)
    {
        builder.HasIndex(p => new { p.MemberId, p.MovieId })
            .IsUnique();

        builder.HasIndex(p => new { p.MemberId, p.EpisodeId })
            .IsUnique();

        builder.HasIndex(p => new { p.MemberId, p.TrackId })
            .IsUnique();

        builder.HasIndex(p => new { p.MemberId, p.LastPlayedAt });

        builder.ToTable(p => p.HasCheckConstraint(
            "CK_PlaybackStates_ExactlyOneWork",
            "(\"MovieId\" IS NOT NULL) + (\"EpisodeId\" IS NOT NULL) + (\"TrackId\" IS NOT NULL) = 1"));

        builder.HasOne(p => p.Member)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Movie)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.Episode)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Track)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}

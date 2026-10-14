using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shogun.Server.Entities;

namespace Shogun.Server.Data.Configurations;

/// <summary>
/// Database rules for members: column lengths, and one row per Firebase user.
/// </summary>
public class MemberEntityConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.Property(m => m.FirebaseUid)
            .HasMaxLength(128);

        builder.Property(m => m.DisplayName)
            .HasMaxLength(255);

        builder.HasIndex(m => m.FirebaseUid)
            .IsUnique();
    }
}
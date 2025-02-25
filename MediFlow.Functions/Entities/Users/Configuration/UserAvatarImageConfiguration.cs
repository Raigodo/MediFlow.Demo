using MediFlow.Functions.Entities.Users.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFlow.Functions.Entities.Users.Configuration;

public sealed class UserAvatarImageConfiguration() : IEntityTypeConfiguration<UserAvatar>
{
    public void Configure(EntityTypeBuilder<UserAvatar> builder)
    {
        builder.HasKey(a => a.UserId);

        builder.Property(a => a.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder.HasOne(a => a.User)
            .WithOne(u => u.Avatar)
            .HasForeignKey<UserAvatar>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

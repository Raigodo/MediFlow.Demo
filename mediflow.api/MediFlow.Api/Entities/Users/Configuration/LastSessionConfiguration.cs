using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFlow.Api.Entities.Users.Configuration;

public sealed class LastSessionConfiguration() : IEntityTypeConfiguration<LastSession>
{
    public void Configure(EntityTypeBuilder<LastSession> builder)
    {
        builder.HasKey(u => u.UserId);

        builder.Property(u => u.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder.Property(u => u.StructureId)
            .HasConversion(
                id => id.Value,
                value => StructureId.Create(value));


        builder.HasOne(ls => ls.User)
            .WithOne(u => u.LastSession)
            .HasForeignKey<LastSession>(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFlow.Functions.Entities.Users.Configuration;

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

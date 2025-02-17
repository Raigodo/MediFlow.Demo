using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFlow.Api.Entities.Structures.Configuration;

public sealed class StructureConfiguration : IEntityTypeConfiguration<Structure>
{
    public void Configure(EntityTypeBuilder<Structure> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => StructureId.Create(value))
            .IsRequired();

        builder.Property(m => m.ManagerId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value))
            .IsRequired(); ;

        builder.Property(s => s.Name)
            .HasMaxLength(100)
            .IsRequired();


        builder.HasOne(s => s.Manager)
            .WithMany()
            .HasForeignKey(m => m.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne(s => s.DeviceKey)
            .WithOne(x => x.Structure)
            .HasForeignKey<Structure>(s => s.Id);
    }
}

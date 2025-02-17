using MediFlow.Api.Entities.Structures.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFlow.Api.Entities.Structures.Configuration;

public sealed class DeviceKeyConfiguration : IEntityTypeConfiguration<DeviceKey>
{
    public void Configure(EntityTypeBuilder<DeviceKey> builder)
    {
        builder.HasKey(m => m.StructureId);

        builder.Property(m => m.StructureId)
            .HasConversion(
                id => id.Value,
                value => StructureId.Create(value))
            .IsRequired();

        builder.HasOne(m => m.Structure)
            .WithOne(s => s.DeviceKey)
            .HasForeignKey<DeviceKey>(d => d.StructureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFlow.Functions.Entities.Structures.Configuration;

public sealed class StructureManagerConfiguration : IEntityTypeConfiguration<StructureManager>
{
    public void Configure(EntityTypeBuilder<StructureManager> builder)
    {
        builder.HasKey(m => m.StructureId);

        builder.Property(m => m.StructureId)
            .HasConversion(
                id => id.Value,
                value => StructureId.Create(value))
            .IsRequired();

        builder.Property(m => m.ManagerId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value))
            .IsRequired(); ;

        builder.HasOne(m => m.Structure)
            .WithOne()
            .HasForeignKey<StructureManager>(sm => sm.StructureId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Manager)
            .WithMany()
            .HasForeignKey(m => m.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

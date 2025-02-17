using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Structures.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFlow.Api.Entities.Employees.Configuration;

public sealed class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(
                id => id.Value,
                value => InvitationId.Create(value))
            .IsRequired();

        builder.Property(e => e.StructureId)
            .HasConversion(
                structureId => structureId.Value,
                value => StructureId.Create(value));

        builder.HasOne(e => e.Structure)
            .WithMany()
            .HasForeignKey(e => e.StructureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

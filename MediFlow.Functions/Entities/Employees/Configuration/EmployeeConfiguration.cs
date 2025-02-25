using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFlow.Functions.Entities.Employees.Configuration;

public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(
                id => id.Value,
                value => EmployeeId.Create(value))
            .IsRequired();

        builder.Property(e => e.UserId)
            .HasConversion(
                userId => userId!.Value.Value,
                value => UserId.Create(value));

        builder.Property(e => e.StructureId)
            .HasConversion(
                structureId => structureId.Value,
                value => StructureId.Create(value))
            .IsRequired();

        builder.Property(e => e.Role)
            .IsRequired();

        builder.HasOne(e => e.Structure)
            .WithMany(s => s.Employees)
            .HasForeignKey(e => e.StructureId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.User)
            .WithMany(u => u.Employments)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

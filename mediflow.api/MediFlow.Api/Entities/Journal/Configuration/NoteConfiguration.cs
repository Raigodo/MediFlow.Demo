using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Journal.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFlow.Api.Entities.Journal.Configuration;
public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .HasConversion(
                id => id.Value,
                value => NoteId.Create(value))
            .IsRequired();

        builder.Property(n => n.CreatorId)
            .HasConversion(
                creatorId => creatorId.Value,
                value => EmployeeId.Create(value))
            .IsRequired();

        builder.Property(n => n.ClientId)
            .HasConversion(
                clientId => clientId.Value,
                value => ClientId.Create(value))
            .IsRequired();

        builder.Property(n => n.Content)
            .HasMaxLength(1000)
            .IsRequired();

        builder.HasOne(n => n.Creator)
            .WithMany(e => e.CreatedNotes)
            .HasForeignKey(n => n.CreatorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(n => n.Client)
            .WithMany(c => c.JournalNotes)
            .HasForeignKey(n => n.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

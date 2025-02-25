using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Notes.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFlow.Functions.Entities.Notes.Configuration;

public class NoteFileConfiguration : IEntityTypeConfiguration<NoteFile>
{
    public void Configure(EntityTypeBuilder<NoteFile> builder)
    {
        builder.HasKey(nf => nf.Id);

        builder.Property(nf => nf.Id)
            .HasConversion(
                id => id.Value,
                value => NoteFileId.Create(value))
            .IsRequired();

        builder.Property(nf => nf.NoteId)
            .HasConversion(
                noteId => noteId.Value,
                value => NoteId.Create(value))
            .IsRequired();

        builder.Property(nf => nf.CreatorId)
            .HasConversion(
                creatorId => creatorId.Value,
                value => EmployeeId.Create(value))
            .IsRequired();

        builder.Property(nf => nf.FileName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(nf => nf.Bytes)
            .IsRequired();

        builder.Property(nf => nf.CreatedAt)
            .IsRequired();

        builder.HasOne(nf => nf.Note)
            .WithMany(n => n.AttachedFiles)
            .HasForeignKey(nf => nf.NoteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(nf => nf.Creator)
            .WithMany()
            .HasForeignKey(nf => nf.CreatorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

using MediFlow.Api.Data.Services.DataEncryptor;
using MediFlow.Api.Data.Util;
using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Structures.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFlow.Api.Entities.Clients.Configuration;

public sealed class ClientConfiguration(IDataEncryptor encryptor) : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => ClientId.Create(value))
            .IsRequired();

        builder.Property(c => c.StructureId)
            .HasConversion(
                structureId => structureId.Value,
                value => StructureId.Create(value))
            .IsRequired();

        builder.Property(c => c.Name)
            .IsEnrypted(encryptor)
            .IsRequired();

        builder.Property(c => c.Surname)
            .IsEnrypted(encryptor)
            .IsRequired();

        builder.Property(c => c.PersonalCode)
            .IsEnrypted(encryptor)
            .IsRequired();

        builder.Property(c => c.Language)
            .IsEnrypted(encryptor);

        builder.Property(c => c.Religion)
            .IsEnrypted(encryptor);

        builder.Property(c => c.Invalidity)
            .IsEnrypted(encryptor);

        builder.Property(c => c.InvalidityFlag)
            .IsEnrypted(encryptor);

        builder.Property(c => c.InvalidityExpiresOn)
            .IsEnrypted(encryptor);

        builder.Property(c => c.BirthDate)
            .IsEnrypted(encryptor);

        builder.HasOne(c => c.Structure)
            .WithMany(s => s.Clients)
            .HasForeignKey(c => c.StructureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

using MediFlow.Api.Data.Services.DataEncryptor;
using MediFlow.Api.Data.Util;
using MediFlow.Api.Entities.Clients.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFlow.Api.Entities.Clients.Configuration;

public sealed class ClientContactConfiguration(IDataEncryptor encryptor) : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => ContactId.Create(value))
            .IsRequired();

        builder.Property(c => c.ClientId)
            .HasConversion(
                id => id.Value,
                value => ClientId.Create(value))
            .IsRequired();

        builder.Property(c => c.Title)
            .IsEnrypted(encryptor)
            .IsRequired();

        builder.Property(c => c.PhoneNumber)
            .IsEnrypted(encryptor);

        builder.HasOne(c => c.Client)
            .WithMany(s => s.Contacts)
            .HasForeignKey(c => c.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

using MediFlow.Api.Data.Services.DataEncryptor;
using MediFlow.Api.Entities.Clients;
using MediFlow.Api.Entities.Clients.Configuration;
using MediFlow.Api.Entities.Employees;
using MediFlow.Api.Entities.Employees.Configuration;
using MediFlow.Api.Entities.Journal;
using MediFlow.Api.Entities.Journal.Configuration;
using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Entities.Structures.Configuration;
using MediFlow.Api.Entities.Users;
using MediFlow.Api.Entities.Users.Configuration;
using MediFlow.Api.Entities.Users.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFlow.Api.Data;

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IDataEncryptor encryptor) : DbContext(options)
{
#nullable disable
    public DbSet<User> Users { get; set; }
    public DbSet<UserAvatar> UserAvatars { get; set; }
    public DbSet<Structure> Structures { get; set; }
    public DbSet<LastSession> LastSessions { get; set; }
    public DbSet<DeviceKey> StructureDeviceKeys { get; set; }
    public DbSet<StructureManager> StructureManagers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Contact> ClientContacts { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<NoteFile> NoteFiles { get; set; }
#nullable restore


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new UserAvatarImageConfiguration());
        builder.ApplyConfiguration(new StructureConfiguration());
        builder.ApplyConfiguration(new LastSessionConfiguration());
        builder.ApplyConfiguration(new StructureManagerConfiguration());
        builder.ApplyConfiguration(new DeviceKeyConfiguration());
        builder.ApplyConfiguration(new EmployeeConfiguration());
        builder.ApplyConfiguration(new InvitationConfiguration());
        builder.ApplyConfiguration(new ClientConfiguration(encryptor));
        builder.ApplyConfiguration(new ClientContactConfiguration(encryptor));
        builder.ApplyConfiguration(new NoteConfiguration());
        builder.ApplyConfiguration(new NoteFileConfiguration());

        SeedAdmin(builder.Entity<User>());
    }

    private static void SeedAdmin(EntityTypeBuilder<User> configuration)
    {
        configuration.HasData(new User()
        {
            Id = UserId.Create(Guid.Parse("30007b30-bdf0-4e7d-bac3-2bfca69a8c4f")),
            Role = UserRoles.Admin,
            Name = "Admin",
            Surname = "First",
            RefreshToken = Guid.Parse("ea6aea58-b934-48a6-982c-d777145aa077"),
            PasswordHash = "$2a$11$XBJDRZO3cIpRhoFYi.wGieOF9gRcf1FbzyFpwDf9rbAyomQmma.ya",//passwordHasher.Generate("P@55w0rd"),
            Email = "mediflow.noreply@gmail.com",
        });
    }
}
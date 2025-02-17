using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Journal;
using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Entities.Structures.Values;

namespace MediFlow.Api.Entities.Clients;

public sealed class Client
{
    public required ClientId Id { get; init; }
    public required StructureId StructureId { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string PersonalCode { get; set; }
    public required string Language { get; set; }
    public required string Religion { get; set; }
    public required InvalidityGroups Invalidity { get; set; }
    public required InvalidityFlags InvalidityFlag { get; set; }
    public required DateOnly? InvalidityExpiresOn { get; set; }
    public required DateOnly BirthDate { get; set; }
    public DateOnly JoinedOn { get; init; } = DateOnly.FromDateTime(DateTime.UtcNow);


#nullable disable
    public Structure Structure { get; init; } = null;
    public List<Note> JournalNotes { get; init; } = [];
    public List<Contact> Contacts { get; init; } = [];
#nullable restore
}

using MediFlow.Functions.Entities.Clients;
using MediFlow.Functions.Entities.Clients.Values;
using MediFlow.Functions.Entities.Employees;
using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Notes.Values;

namespace MediFlow.Functions.Entities.Notes;

public sealed class Note
{
    public NoteId Id { get; init; } = NoteId.Generate();
    public required EmployeeId CreatorId { get; init; }
    public required ClientId ClientId { get; init; }
    public required string Content { get; set; }
    public bool IsFlagged { get; set; } = false;
    public DateOnly CreatedOn { get; init; } = DateOnly.FromDateTime(DateTime.UtcNow);

#nullable disable
    public Employee Creator { get; init; } = null;
    public Client Client { get; init; } = null;
    public List<NoteFile> AttachedFiles { get; init; } = [];
#nullable restore
}

using MediFlow.Api.Entities.Clients;
using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Employees;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Journal.Values;

namespace MediFlow.Api.Entities.Journal;

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

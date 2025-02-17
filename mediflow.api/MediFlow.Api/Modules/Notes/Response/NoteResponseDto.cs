using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Journal.Values;

namespace MediFlow.Api.Modules.Notes.Response;

public record NoteResponseDto
{
    public required NoteId Id { get; init; }
    public required ClientId ClientId { get; init; }
    public required string Content { get; init; }
    public required bool IsFlagged { get; init; }
    public required DateOnly CreatedOn { get; init; }
    public required NoteCreatorDto Creator { get; init; }

    public readonly record struct NoteCreatorDto
    {
        public required EmployeeId Id { get; init; }
        public required EmployeeRoles Role { get; init; }
        public required string? Name { get; init; }
        public required string? Surname { get; init; }
    }
}

using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Journal;
using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Entities.Employees;

public sealed class Employee
{
    public EmployeeId Id { get; init; } = EmployeeId.Generate();
    public required UserId? UserId { get; init; }
    public required StructureId StructureId { get; init; }
    public required EmployeeRoles Role { get; init; }
    public DateOnly AssignedOn { get; init; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateOnly? UnassignedOn { get; set; } = null;


    public void Unassign()
    {
        if (UnassignedOn != null)
            throw new InvalidOperationException();
        UnassignedOn = DateOnly.FromDateTime(DateTime.UtcNow);
    }


#nullable disable
    public Structure Structure { get; init; }
    public User User { get; init; }
    public List<Note> CreatedNotes { get; init; } = [];
#nullable restore
}
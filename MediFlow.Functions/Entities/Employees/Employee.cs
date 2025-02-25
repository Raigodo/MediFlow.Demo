using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Notes;
using MediFlow.Functions.Entities.Structures;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Entities.Employees;

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
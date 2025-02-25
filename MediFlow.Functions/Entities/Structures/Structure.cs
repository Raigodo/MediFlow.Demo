using MediFlow.Functions.Entities.Clients;
using MediFlow.Functions.Entities.Employees;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Entities.Structures;

public sealed class Structure
{
    public StructureId Id { get; init; } = StructureId.Generate();
    public required UserId ManagerId { get; set; }
    public required string Name { get; set; }


#nullable disable
    public User Manager { get; init; }
    public DeviceKey DeviceKey { get; init; }
    public List<Employee> Employees { get; init; } = [];
    public List<Client> Clients { get; init; } = [];
#nullable restore
}

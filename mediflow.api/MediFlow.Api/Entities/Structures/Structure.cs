using MediFlow.Api.Entities.Clients;
using MediFlow.Api.Entities.Employees;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Entities.Structures;

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

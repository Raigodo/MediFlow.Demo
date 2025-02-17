using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Modules.Structures.Response;

public record StructureResponseDto
{
    public required StructureId Id { get; init; }
    public required string Name { get; init; }
    public required StructureResponseManagerDto Manager { get; init; }

    public record StructureResponseManagerDto
    {
        public required UserId UserId { get; init; }
        public required string Name { get; init; }
        public required string Surname { get; init; }
    }
}

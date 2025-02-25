using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Modules.Structures.Functions.Responses;

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

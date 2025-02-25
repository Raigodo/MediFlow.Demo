using MediFlow.Functions.Entities.Structures;
using MediFlow.Functions.Entities.Users;

namespace MediFlow.Functions.Modules.Structures.Functions.Responses;

public static class ConverterExtensions
{
    public static IEnumerable<StructureResponseDto> ToResponseDto(this IEnumerable<Structure> structures)
    {
        return structures.Select(s => s.ToResponseDto());
    }

    public static StructureResponseDto ToResponseDto(this Structure structure)
    {
        return new StructureResponseDto()
        {
            Id = structure.Id,
            Name = structure.Name,
            Manager = structure.Manager.ToManagerDto(),
        };
    }
    private static StructureResponseDto.StructureResponseManagerDto ToManagerDto(this User manager)
    {
        return new()
        {
            UserId = manager.Id,
            Name = manager.Name,
            Surname = manager.Surname,
        };
    }
}

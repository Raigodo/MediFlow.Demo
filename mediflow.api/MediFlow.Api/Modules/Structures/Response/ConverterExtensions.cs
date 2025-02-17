using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Entities.Users;

namespace MediFlow.Api.Modules.Structures.Response;

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

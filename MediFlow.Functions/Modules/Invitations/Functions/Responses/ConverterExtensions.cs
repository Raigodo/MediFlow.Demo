using MediFlow.Functions.Entities.Employees;

namespace MediFlow.Functions.Modules.Invitations.Functions.Responses;

public static class ConverterExtensions
{

    public static IEnumerable<InvitationRespnseDto> ToResponseDto(this IEnumerable<Invitation> emmployees)
    {
        return emmployees.Select(e => e.ToResponseDto());
    }

    public static InvitationRespnseDto ToResponseDto(this Invitation invitation)
    {
        return new InvitationRespnseDto()
        {
            Id = invitation.Id,
            StructureId = invitation.StructureId,
            StructureName = invitation.Structure.Name,
            EmployeeRole = invitation.Role,
            IssuedAt = invitation.IssuedAt,
        };
    }
}

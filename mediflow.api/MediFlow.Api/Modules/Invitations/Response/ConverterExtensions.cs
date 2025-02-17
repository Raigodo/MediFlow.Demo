using MediFlow.Api.Entities.Employees;

namespace MediFlow.Api.Modules.Invitations.Response;

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

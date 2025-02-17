using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Structures.Values;

namespace MediFlow.Api.Modules.Clients.Response;

public record ClientResponseDto
{
    public required ClientId Id { get; init; }
    public required StructureId StructureId { get; init; }
    public required string PersonalCode { get; init; }
    public required DateOnly BirthDate { get; init; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public required string Language { get; init; }
    public required string Religion { get; init; }
    public required InvalidityGroups InvalidtiyGroup { get; init; }
    public required InvalidityFlags InvalidityFlag { get; init; }
    public required DateOnly? InvalidityExpiresOn { get; init; }
    public required DateOnly JoinedOn { get; init; }
    public required IEnumerable<ClientContactDto> Contacts { get; init; }


    public readonly record struct ClientContactDto
    {
        public required readonly ContactId Id { get; init; }
        public required readonly string Title { get; init; }
        public required readonly string PhoneNumber { get; init; }
    }
}
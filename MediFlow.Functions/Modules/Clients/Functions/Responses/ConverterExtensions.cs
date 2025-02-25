using MediFlow.Functions.Entities.Clients;

namespace MediFlow.Functions.Modules.Clients.Functions.Responses;

public static class ConverterExtensions
{
    public static IEnumerable<ClientResponseDto> ToResponseDto(this IEnumerable<Client> clients)
    {
        return clients.Select(c => c.ToResponseDto());
    }

    public static ClientResponseDto ToResponseDto(this Client client)
    {
        return new ClientResponseDto()
        {
            Id = client.Id,
            StructureId = client.StructureId,
            PersonalCode = client.PersonalCode,
            Name = client.Name,
            Surname = client.Surname,
            Language = client.Language,
            Religion = client.Religion,
            BirthDate = client.BirthDate,
            InvalidtiyGroup = client.Invalidity,
            InvalidityFlag = client.InvalidityFlag,
            InvalidityExpiresOn = client.InvalidityExpiresOn,
            JoinedOn = client.JoinedOn,
            Contacts = client.Contacts.ToResponseDto(),
        };
    }

    private static IEnumerable<ClientResponseDto.ClientContactDto> ToResponseDto(this IEnumerable<Contact> contacts)
    {
        return contacts.Select(c => new ClientResponseDto.ClientContactDto()
        {
            Id = c.Id,
            Title = c.Title,
            PhoneNumber = c.PhoneNumber,
        });
    }
}

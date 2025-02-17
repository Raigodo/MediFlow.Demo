using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Data.Services.UnitOfWork;
using MediFlow.Api.Entities.Clients;
using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules.Clients.Response;

namespace MediFlow.Api.Modules.Clients.Endpoints;

public readonly record struct UpdateClientContactDto(
    ContactId? ClientContactId,
    string ClientContactTitle,
    string ClientContactPhoneNumber);

public record UpdateClientRequest(
    string? ClientName,
    string? ClientSurname,
    string? ClientPersonalCode,
    string? ClientLanguage,
    string? ClientReligion,
    DateOnly? ClientBirthDate,
    InvalidityGroups? ClientInvalidityGroup,
    InvalidityFlags? ClientInvalidityFlag,
    DateOnly? ClientInvalidityExpiresOn,
    IEnumerable<UpdateClientContactDto>? ClientContacts);

public static class UpdateClientEndpoint
{
    public static IEndpointRouteBuilder MapUpdateClientEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPatch("/api/clients/{ClientId}", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        ClientId clientId,
        UpdateClientRequest req,
        IClientRepository clientRepository,
        IClientContactRepository clientContactRepository,
        IUnitOfWork unitOfWork,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var client = await clientRepository.GetOneAsync(clientId);
        if (client is null)
        {
            return responseFactory.NotFound<Client>();
        }

        var hasAccess = await accessGuard.CanWriteAsync(clientId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<Client>();
        }

        if (req.ClientName is not null)
            client.Name = req.ClientName;

        if (req.ClientSurname is not null)
            client.Surname = req.ClientSurname;

        if (req.ClientBirthDate is not null)
            client.BirthDate = req.ClientBirthDate.Value;

        if (req.ClientLanguage is not null)
            client.Language = req.ClientLanguage;

        if (req.ClientReligion is not null)
            client.Religion = req.ClientReligion;

        if (req.ClientPersonalCode is not null)
            client.PersonalCode = req.ClientPersonalCode;

        if (req.ClientInvalidityGroup is not null)
            client.Invalidity = req.ClientInvalidityGroup.Value;

        if (req.ClientInvalidityFlag is not null && client.Invalidity != InvalidityGroups.None)
            client.InvalidityFlag = req.ClientInvalidityFlag.Value;
        else client.InvalidityFlag = InvalidityFlags.None;

        if (req.ClientInvalidityExpiresOn is not null && client.Invalidity != InvalidityGroups.None)
            client.InvalidityExpiresOn = req.ClientInvalidityExpiresOn;
        else client.InvalidityExpiresOn = null;

        if (req.ClientContacts is not null)
        {
            foreach (var contact in client.Contacts)
            {
                if (!req.ClientContacts.Any(c => c.ClientContactId == contact.Id))
                {
                    clientContactRepository.Delete(contact);
                }
            }

            var contacts = req.ClientContacts.ToArray();
            for (int i = 0; i < contacts.Length; i++)
            {
                var contactDto = contacts.ElementAt(i);
                var contactIndex = contactDto.ClientContactId is null
                    ? -1
                    : client.Contacts.FindIndex(c => c.Id == contactDto.ClientContactId);

                if (contactIndex == -1)
                {
                    var contact = new Contact()
                    {
                        Id = ContactId.Generate(),
                        ClientId = clientId,
                        Title = contactDto.ClientContactTitle,
                        PhoneNumber = contactDto.ClientContactPhoneNumber
                    };
                    clientContactRepository.Add(contact);
                }
                else
                {
                    client.Contacts[contactIndex].Title = contactDto.ClientContactTitle;
                    client.Contacts[contactIndex].PhoneNumber = contactDto.ClientContactPhoneNumber;
                }
            }
        }

        await unitOfWork.SaveChangesAsync();

        return responseFactory.Ok(client.ToResponseDto());
    }
}

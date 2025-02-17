using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Data.Services.UnitOfWork;
using MediFlow.Api.Entities.Clients;
using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Api.Modules.Clients.Response;

namespace MediFlow.Api.Modules.Clients.Endpoints;

public readonly record struct AddClientContactDto(string ContactTitle, string ContactPhoneNumber);

public record AddClientRequest(
    ClientId ClientId,
    string ClientName,
    string ClientSurname,
    string ClientPersonalCode,
    string ClientLanguage,
    string ClientReligion,
    DateOnly ClientBirthDate,
    InvalidityGroups ClientInvalidityGroup,
    InvalidityFlags ClientInvalidityFlag,
    DateOnly? ClientInvalidityExpiresOn,
    IEnumerable<AddClientContactDto>? ClientContacts);

public static class AddClientEndpoint
{
    public static IEndpointRouteBuilder MapAddClientEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/clients", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        AddClientRequest req,
        IClientRepository clientRepository,
        IClientContactRepository clientContactRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserAccessor currentUser,
        ResponseFactory responseFactory)
    {
        var clientIdExists = await clientRepository.ExistsAsync(req.ClientId);
        if (clientIdExists)
        {
            return responseFactory.Conflict<Client>();
        }

        var invalidityFlag = req.ClientInvalidityGroup != InvalidityGroups.None
        ? req.ClientInvalidityFlag
            : InvalidityFlags.None;
        var invalidityExpiresOn = req.ClientInvalidityGroup != InvalidityGroups.None
            ? req.ClientInvalidityExpiresOn
            : null;

        var client = new Client
        {
            Id = req.ClientId,
            StructureId = currentUser.StructureId,
            Name = req.ClientName,
            Surname = req.ClientSurname,
            PersonalCode = req.ClientPersonalCode,
            BirthDate = req.ClientBirthDate,
            Language = req.ClientLanguage,
            Religion = req.ClientReligion,
            Invalidity = req.ClientInvalidityGroup,
            InvalidityFlag = invalidityFlag,
            InvalidityExpiresOn = invalidityExpiresOn,
        };

        clientRepository.Add(client);

        var cantacts = req.ClientContacts?.ToArray() ?? [];
        for (int i = 0; i < cantacts.Length; i++)
        {
            var contactDto = cantacts.ElementAt(i);
            var contact = new Contact()
            {
                Id = ContactId.Generate(),
                ClientId = req.ClientId,
                Title = contactDto.ContactTitle,
                PhoneNumber = contactDto.ContactPhoneNumber
            };
            clientContactRepository.Add(contact);
        }

        await unitOfWork.SaveChangesAsync();

        return responseFactory.Ok(client.ToResponseDto());
    }
}

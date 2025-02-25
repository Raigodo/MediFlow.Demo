using Mediator;
using MediFlow.Functions.Data.Services.UnitOfWork;
using MediFlow.Functions.Entities.Clients;
using MediFlow.Functions.Entities.Clients.Values;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.AccessGuard;
using MediFlow.Functions.Modules._Shared.Services.CurrentUserAccessor;

namespace MediFlow.Functions.Modules.Clients.Commands;
using Result = Result<Client, ClientsErrors>;

public readonly record struct CreateClientContactDto(string ContactTitle, string ContactPhoneNumber);
public record CreateClientCommand(
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
    IEnumerable<CreateClientContactDto>? ClientContacts) : ICommand<Result>;

public class CreateClientCommandHandler(
    IClientRepository clientRepository,
    IClientContactRepository clientContactRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserAccessor currentUser,
    IAccessGuard accessGuard) : ICommandHandler<CreateClientCommand, Result>
{
    public async ValueTask<Result> Handle(CreateClientCommand command, CancellationToken cancellationToken)
    {
        var canAccess = await accessGuard.CanCreateClientAsync(currentUser.StructureId);

        var clientIdExists = await clientRepository.ExistsAsync(command.ClientId);
        if (clientIdExists)
        {
            return ClientsErrors.ClientAlreadyExists;
        }

        var invalidityFlag = command.ClientInvalidityGroup != InvalidityGroups.None
            ? command.ClientInvalidityFlag
            : InvalidityFlags.None;
        var invalidityExpiresOn = command.ClientInvalidityGroup != InvalidityGroups.None
            ? command.ClientInvalidityExpiresOn
            : null;

        var client = new Client
        {
            Id = command.ClientId,
            StructureId = currentUser.StructureId,
            Name = command.ClientName,
            Surname = command.ClientSurname,
            PersonalCode = command.ClientPersonalCode,
            BirthDate = command.ClientBirthDate,
            Language = command.ClientLanguage,
            Religion = command.ClientReligion,
            Invalidity = command.ClientInvalidityGroup,
            InvalidityFlag = invalidityFlag,
            InvalidityExpiresOn = invalidityExpiresOn,
        };

        clientRepository.Add(client);

        var cantacts = command.ClientContacts?.ToArray() ?? [];
        for (int i = 0; i < cantacts.Length; i++)
        {
            var contactDto = cantacts.ElementAt(i);
            var contact = new Contact()
            {
                Id = ContactId.Generate(),
                ClientId = command.ClientId,
                Title = contactDto.ContactTitle,
                PhoneNumber = contactDto.ContactPhoneNumber
            };
            clientContactRepository.Add(contact);
        }

        await unitOfWork.SaveChangesAsync();

        return client;
    }
}

using Mediator;
using MediFlow.Functions.Data.Services.UnitOfWork;
using MediFlow.Functions.Entities.Clients;
using MediFlow.Functions.Entities.Clients.Values;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.AccessGuard;

namespace MediFlow.Functions.Modules.Clients.Commands;
using Result = Result<Client, ClientsErrors>;
public readonly record struct UpdateClientContactDto(
    ContactId? ClientContactId,
    string ClientContactTitle,
    string ClientContactPhoneNumber);

public record UpdateClientCommand(
    ClientId ClientId,
    string? ClientName,
    string? ClientSurname,
    string? ClientPersonalCode,
    string? ClientLanguage,
    string? ClientReligion,
    DateOnly? ClientBirthDate,
    InvalidityGroups? ClientInvalidityGroup,
    InvalidityFlags? ClientInvalidityFlag,
    DateOnly? ClientInvalidityExpiresOn,
    IEnumerable<UpdateClientContactDto>? ClientContacts) : ICommand<Result>;

public class UpdateClientCommandHandler(
    IClientContactRepository clientContactRepository,
    IUnitOfWork unitOfWork,
    IAccessGuard accessGuard) : ICommandHandler<UpdateClientCommand, Result>
{
    public async ValueTask<Result> Handle(UpdateClientCommand command, CancellationToken cancellationToken)
    {
        var client = await accessGuard.CheckAccessAndGetAsync(command.ClientId, GuardOptions.Update);
        if (client is null)
        {
            return ClientsErrors.UserCanNotAccessClient;
        }

        if (command.ClientName is not null)
            client.Name = command.ClientName;

        if (command.ClientSurname is not null)
            client.Surname = command.ClientSurname;

        if (command.ClientBirthDate is not null)
            client.BirthDate = command.ClientBirthDate.Value;

        if (command.ClientLanguage is not null)
            client.Language = command.ClientLanguage;

        if (command.ClientReligion is not null)
            client.Religion = command.ClientReligion;

        if (command.ClientPersonalCode is not null)
            client.PersonalCode = command.ClientPersonalCode;

        if (command.ClientInvalidityGroup is not null)
            client.Invalidity = command.ClientInvalidityGroup.Value;

        if (command.ClientInvalidityFlag is not null && client.Invalidity != InvalidityGroups.None)
            client.InvalidityFlag = command.ClientInvalidityFlag.Value;
        else client.InvalidityFlag = InvalidityFlags.None;

        if (command.ClientInvalidityExpiresOn is not null && client.Invalidity != InvalidityGroups.None)
            client.InvalidityExpiresOn = command.ClientInvalidityExpiresOn;
        else client.InvalidityExpiresOn = null;

        if (command.ClientContacts is not null)
        {
            foreach (var contact in client.Contacts)
            {
                if (!command.ClientContacts.Any(c => c.ClientContactId == contact.Id))
                {
                    clientContactRepository.Delete(contact);
                }
            }

            var contacts = command.ClientContacts.ToArray();
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
                        ClientId = command.ClientId,
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

        return client;
    }
}

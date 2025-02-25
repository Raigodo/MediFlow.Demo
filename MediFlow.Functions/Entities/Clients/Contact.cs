using MediFlow.Functions.Entities.Clients.Values;

namespace MediFlow.Functions.Entities.Clients;

public sealed class Contact
{
    public ContactId Id { get; init; }
    public required ClientId ClientId { get; init; }
    public required string Title { get; set; }
    public required string PhoneNumber { get; set; }


#nullable disable
    public Client Client { get; init; } = null;
#nullable restore
}

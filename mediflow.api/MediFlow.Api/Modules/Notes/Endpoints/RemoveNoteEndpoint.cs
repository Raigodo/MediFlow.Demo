using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Clients;
using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Journal;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;

namespace MediFlow.Api.Modules.Notes.Endpoints;

public static class RemoveNoteEndpoint
{
    public static IEndpointRouteBuilder MapRemoveNoteEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapDelete("/api/journal/clients/{clientId}/notes/today", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        ClientId clientId,
        INoteRepository noteRepository,
        IClientRepository clientRepository,
        ICurrentUserAccessor currentUser,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var clientExists = await clientRepository.ExistsAsync(clientId);
        if (!clientExists)
        {
            return responseFactory.NotFound<Client>();
        }

        var note = await noteRepository.GetOneAsync(clientId, currentUser.EmployeeRole);
        if (note is null)
        {
            return responseFactory.NotFound<Note>();
        }

        var hasAccess = await accessGuard.CanWriteAsync(note.Id);
        if (!hasAccess)
        {
            return responseFactory.NotFound<Client>();
        }

        await noteRepository.DeleteAsync(clientId, currentUser.EmployeeRole);

        return responseFactory.NoContent();
    }
}

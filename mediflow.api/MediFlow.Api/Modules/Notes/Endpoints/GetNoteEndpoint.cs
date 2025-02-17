using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Clients;
using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Journal;
using MediFlow.Api.Entities.Journal.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules.Notes.Response;

namespace MediFlow.Api.Modules.Notes.Endpoints;

public static class GetNoteEndpoint
{
    public static IEndpointRouteBuilder MapGetNoteEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/journal/clients/{clientId}/notes/{noteId}", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }
    public static async Task<IResult> Handle(
        ClientId clientId,
        NoteId noteId,
        INoteRepository noteRepository,
        IClientRepository clientRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var clientExists = await clientRepository.ExistsAsync(clientId);
        if (!clientExists)
        {
            return responseFactory.NotFound<Client>();
        }

        var note = await noteRepository.GetOneAsync(noteId);
        if (note is null)
        {
            return responseFactory.NotFound<Client>();
        }

        var hasAccess = await accessGuard.CanViewAsync(noteId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<Note>();
        }

        return responseFactory.Ok(note.ToResponseDto());

    }
}

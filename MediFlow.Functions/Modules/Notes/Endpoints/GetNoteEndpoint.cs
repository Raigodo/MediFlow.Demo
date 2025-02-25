namespace MediFlow.Functions.Modules.Notes.Endpoints;

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

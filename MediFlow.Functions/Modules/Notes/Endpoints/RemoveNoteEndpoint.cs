namespace MediFlow.Functions.Modules.Notes.Endpoints;

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

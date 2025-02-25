namespace MediFlow.Functions.Modules.Notes.Endpoints;

public record WriteNoteRequest(string NoteContent, bool IsImportant);

public static class WriteNoteEndpoint
{
    public static IEndpointRouteBuilder MapWriteNoteEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPut("/api/journal/clients/{clientId}/notes/today", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        ClientId clientId,
        WriteNoteRequest req,
        IClientRepository clientRepository,
        INoteRepository noteRepository,
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserAccessor currentUser,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        if (currentUser.UserRole != UserRoles.Employee)
        {
            return responseFactory.Forbid();
        }

        var clientExists = await clientRepository.ExistsAsync(clientId);
        if (!clientExists)
        {
            return responseFactory.NotFound<Client>();
        }

        var hasAccess = await accessGuard.CanWriteAsync(clientId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<Client>();
        }

        var note = await noteRepository.GetOneAsync(clientId, currentUser.EmployeeRole);
        if (note is null)
        {
            note = new Note
            {
                CreatorId = currentUser.EmployeeId,
                ClientId = clientId,
                Content = req.NoteContent,
                Creator = await employeeRepository.GetOneAsync(currentUser.EmployeeId),
                IsFlagged = req.IsImportant,
            };
            noteRepository.Add(note);
        }
        else
        {
            note.Content = req.NoteContent;
            note.IsFlagged = req.IsImportant;
        }

        await unitOfWork.SaveChangesAsync();


        return responseFactory.Ok(note.ToResponseDto());
    }
}

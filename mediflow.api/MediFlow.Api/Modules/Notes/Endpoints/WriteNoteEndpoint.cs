using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Data.Services.UnitOfWork;
using MediFlow.Api.Entities.Clients;
using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Journal;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Api.Modules.Notes.Response;

namespace MediFlow.Api.Modules.Notes.Endpoints;

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

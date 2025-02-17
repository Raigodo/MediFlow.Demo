using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Clients;
using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Journal;
using MediFlow.Api.Entities.Journal.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Api.Modules.Notes.Response;

namespace MediFlow.Api.Modules.Notes.Endpoints;

public static class GetTodaysNoteEndpoint
{
    public static IEndpointRouteBuilder MapGetTodaysNoteEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/journal/clients/{clientId}/notes/today", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        ClientId clientId,
        IClientRepository clientRepository,
        INoteRepository noteRepository,
        IEmployeeRepository employeeRepository,
        ICurrentUserAccessor currentUser,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var clientExists = await clientRepository.ExistsAsync(clientId);

        if (!clientExists)
        {
            return responseFactory.NotFound<Client>();
        }

        var hasAccess = await accessGuard.CanViewAsync(clientId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<Client>();
        }

        var note = await noteRepository.GetOneAsync(clientId, currentUser.EmployeeRole);
        if (note is null)
        {
            var employee = await employeeRepository.GetOneAsync(currentUser.EmployeeId);
            note = new Note()
            {
                Id = NoteId.Create(Guid.Empty),
                CreatorId = currentUser.EmployeeId,
                ClientId = clientId,
                Content = "",
                Creator = employee,
            };
        }

        return responseFactory.Ok(note.ToResponseDto());
    }
}

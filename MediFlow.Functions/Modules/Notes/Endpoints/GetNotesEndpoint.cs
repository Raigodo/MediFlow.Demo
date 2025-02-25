using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Functions.Modules.Notes.Endpoints;

public static class GetNotesEndpoint
{
    public static IEndpointRouteBuilder MapGetNotesEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/journal/clients/{clientId}/notes", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        ClientId clientId,
        [FromQuery] EmployeeRoles? role,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        [FromQuery] bool? flagged,
        [FromQuery] string? filterMask,
        IClientRepository clientRepository,
        INoteRepository noteRepository,
        ICurrentUserAccessor currentUser,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 7)
    {
        var clientExists = await clientRepository.ExistsAsync(clientId);
        if (!clientExists)
        {
            return responseFactory.NotFound<Client>();
        }

        var hasAccess = await accessGuard.CanViewAsync(clientId);
        var canViewRole = currentUser.EmployeeRole >= role;
        if (!(hasAccess || canViewRole))
        {
            return responseFactory.NotFound<Client>();
        }

        var skipCount = (page - 1) * pageSize;
        var notes = await noteRepository.GetManyAsync(
            clientId,
            role ?? currentUser.EmployeeRole,
            from,
            to,
            flagged,
            skipCount,
            pageSize);

        return responseFactory.Ok(notes.ToResponseDto());
    }
}

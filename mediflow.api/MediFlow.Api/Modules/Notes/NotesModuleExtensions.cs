using FluentValidation;
using MediFlow.Api.Modules.Notes.Endpoints;

namespace MediFlow.Api.Modules.Notes;

public static class NotesModuleExtensions
{
    public static IServiceCollection AddNotesModule(this IServiceCollection services)
    {
        services.AddScoped<IValidator<WriteNoteRequest>, WriteNoteRequestValidator>();
        return services;
    }

    public static IEndpointRouteBuilder MapNotesEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGroup("/")
            .WithTags("Notes")
            .MapGetNoteEndpoint()
            .MapGetTodaysNoteEndpoint()
            .MapGetNotesEndpoint()
            .MapWriteNoteEndpoint()
            .MapRemoveNoteEndpoint();
        return routes;
    }
}

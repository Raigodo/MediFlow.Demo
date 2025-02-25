using FluentValidation;
using MediFlow.Functions.Modules.Notes.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace MediFlow.Functions.Modules.Notes;

public static class NotesModuleExtensions
{
    public static IServiceCollection AddNotesModule(this IServiceCollection services)
    {
        services.AddScoped<IValidator<WriteNoteCommand>, WriteNoteCommandValidator>();
        return services;
    }
}

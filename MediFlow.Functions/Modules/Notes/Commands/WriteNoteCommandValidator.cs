using FluentValidation;

namespace MediFlow.Functions.Modules.Notes.Commands;

public sealed class WriteNoteCommandValidator : AbstractValidator<WriteNoteCommand>
{
    public WriteNoteCommandValidator()
    {

    }
}

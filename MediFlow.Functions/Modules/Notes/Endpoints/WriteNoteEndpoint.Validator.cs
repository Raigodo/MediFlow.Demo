using FluentValidation;

namespace MediFlow.Functions.Modules.Notes.Endpoints;

class WriteNoteRequestValidator : AbstractValidator<WriteNoteRequest>
{
    public WriteNoteRequestValidator()
    {
        RuleFor(x => x.NoteContent)
            .NotEmpty()
                .WithMessage("Note content cannot be empty")
            .MinimumLength(2)
                .WithMessage("Note content must be at least 2 characters long")
            .MaximumLength(1000)
                .WithMessage("Note content cannot exceed 1000 characters");
    }
}
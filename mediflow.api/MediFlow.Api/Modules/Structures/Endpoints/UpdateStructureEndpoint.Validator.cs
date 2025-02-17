using FluentValidation;

namespace MediFlow.Api.Modules.Structures.Endpoints;

public class UpdateStructureRequestValidator : AbstractValidator<UpdateStructureRequest>
{
    public UpdateStructureRequestValidator()
    {
        RuleFor(x => x.StructureName)
            .NotEmpty()
                .WithMessage("Structure name cannot be empty")
            .MinimumLength(2)
                .WithMessage("Structure name must be at least 2 characters long")
            .MaximumLength(100)
                .WithMessage("Structure name cannot exceed 100 characters")
            .Matches(@"^[\p{L}\s\-]+$")
                .WithMessage("Structure name can only contain letters, spaces, and hyphens");
    }
}
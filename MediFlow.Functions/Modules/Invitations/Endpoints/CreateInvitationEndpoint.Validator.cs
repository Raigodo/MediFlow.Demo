using FluentValidation;

namespace MediFlow.Functions.Modules.Invitations.Endpoints;

public class CreateInvitationRequestValidator : AbstractValidator<CreateInvitationRequest>
{
    public CreateInvitationRequestValidator()
    {
        RuleFor(x => x.EmployeeRole)
            .Must(role => role != 0)
                .WithMessage("Employee role must be specified and cannot be the default value.");
    }
}
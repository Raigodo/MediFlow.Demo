using FluentValidation;

namespace MediFlow.Functions.Modules.Auth.Commands;

public class JoinCommandValidator : AbstractValidator<JoinCommand>
{
    public JoinCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("Email is required")
            .EmailAddress()
                .WithMessage("Invalid email");

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage("Password is required")
            .MinimumLength(6)
                .WithMessage("Password must contain at least 6 characters")
            .MaximumLength(16)
                .WithMessage("Password cannot exceed 16 characters")
            .Matches(@"^[a-zA-Z0-9!@#$%^&*()_+=-]+$")
                .WithMessage("Password contains invalid characters");
    }
}

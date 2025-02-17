using FluentValidation;

namespace MediFlow.Api.Modules.Users.Endpoints;

class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
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

        RuleFor(x => x.UserName)
            .NotEmpty()
                .WithMessage("User name is required")
            .MinimumLength(2)
                .WithMessage("User name must be at least 2 characters long")
            .MaximumLength(50)
                .WithMessage("User name cannot exceed 50 characters")
            .Matches(@"^[\p{L} \-]+$")
                .WithMessage("User name can only contain letters");

        RuleFor(x => x.UserSurname)
            .NotEmpty()
                .WithMessage("User surname is required")
            .MinimumLength(2)
                .WithMessage("User surname must be at least 2 characters long")
            .MaximumLength(50)
                .WithMessage("User surname cannot exceed 50 characters")
            .Matches(@"^[\p{L} \-]+$")
                .WithMessage("User surname can only contain letters");

        RuleFor(x => x.UserRole)
            .Must(role => role != 0)
                .WithMessage("User role must be specified and cannot be the default value.");
    }
}
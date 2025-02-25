using FluentValidation;

namespace MediFlow.Functions.Modules.Users.Commands;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {

    }
}

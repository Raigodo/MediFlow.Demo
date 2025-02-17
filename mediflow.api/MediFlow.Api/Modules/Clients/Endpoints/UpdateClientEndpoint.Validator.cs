using FluentValidation;

namespace MediFlow.Api.Modules.Clients.Endpoints;

public class UpdateClientRequestValidator : AbstractValidator<UpdateClientRequest>
{
    public UpdateClientRequestValidator()
    {
        When(x => x.ClientName is not null, () =>
        {
            RuleFor(x => x.ClientName)
                .NotEmpty()
                    .WithMessage("Client name cannot be empty")
                .MinimumLength(2)
                    .WithMessage("Client name must be at least 2 characters long")
                .MaximumLength(50)
                    .WithMessage("Client name cannot exceed 50 characters")
                .Matches(@"^[\p{L} \-]+$")
                    .WithMessage("Client name can only contain letters, spaces, and hyphens");
        });

        When(x => x.ClientSurname is not null, () =>
        {
            RuleFor(x => x.ClientSurname)
                .NotEmpty()
                    .WithMessage("Client surname cannot be empty")
                .MinimumLength(2)
                    .WithMessage("Client surname must be at least 2 characters long")
                .MaximumLength(50)
                    .WithMessage("Client surname cannot exceed 50 characters")
                .Matches(@"^[\p{L} \-]+$")
                    .WithMessage("Client surname can only contain letters, spaces, and hyphens");
        });

        When(x => x.ClientPersonalCode is not null, () =>
        {
            RuleFor(x => x.ClientPersonalCode)
                .NotEmpty()
                    .WithMessage("Client personal code cannot be empty")
                .Matches(@"^\d{6}-\d{5}$")
                    .WithMessage("Client personal code must contain only numbers and be 6-12 digits long");
        });

        When(x => x.ClientLanguage is not null, () =>
        {
            RuleFor(x => x.ClientLanguage)
                .NotEmpty()
                    .WithMessage("Client language cannot be empty")
                .MinimumLength(2)
                    .WithMessage("Client language must be at least 2 characters long")
                .MaximumLength(50)
                    .WithMessage("Client language cannot exceed 50 characters")
                .Matches(@"^[\p{L} \-]+$")
                    .WithMessage("Client language can only contain letters, spaces, and hyphens");
        });

        When(x => x.ClientReligion is not null, () =>
        {
            RuleFor(x => x.ClientReligion)
                .NotEmpty()
                    .WithMessage("Client religion cannot be empty")
                .MinimumLength(2)
                    .WithMessage("Client religion must be at least 2 characters long")
                .MaximumLength(50)
                    .WithMessage("Client religion cannot exceed 50 characters")
                .Matches(@"^[\p{L} \-]+$")
                    .WithMessage("Client religion can only contain letters, spaces, and hyphens");
        });

        When(x => x.ClientBirthDate is not null, () =>
        {
            RuleFor(x => x.ClientBirthDate)
                .LessThan(DateOnly.FromDateTime(DateTime.Today))
                    .WithMessage("Birth date must be in the past");
        });

        When(x => x.ClientInvalidityFlag is not null, () =>
        {
            RuleFor(x => x.ClientInvalidityFlag)
                .Must(value => value != 0)
                    .WithMessage("Invalidity flag must be specified")
                    .When(x => x.ClientInvalidityGroup != 0);
        });

        When(x => x.ClientInvalidityGroup is not null, () =>
        {
            RuleFor(x => x.ClientInvalidityExpiresOn)
                .Must((request, expirationDate) => expirationDate is not null && expirationDate >= DateOnly.FromDateTime(DateTime.Today))
                    .When(x => x.ClientInvalidityGroup != 0)
                    .WithMessage("Expiration date must be in the future if invalidity group is specified");
        });

        When(x => x.ClientContacts is not null, () =>
        {
            RuleForEach(x => x.ClientContacts!).ChildRules(contact =>
            {
                contact.RuleFor(c => c.ClientContactTitle)
                    .NotEmpty()
                        .WithMessage("Contact title is required")
                    .MaximumLength(100)
                        .WithMessage("Contact title cannot exceed 100 characters")
                    .Matches(@"^[\p{L}0-9\s\-_'.,]+$")
                        .WithMessage("Contact title contains invalid characters");

                contact.RuleFor(c => c.ClientContactPhoneNumber)
                    .NotEmpty()
                        .WithMessage("Contact phone number is required")
                    .Matches(@"^\+?[0-9\s\-()]+$")
                        .WithMessage("Invalid phone number format");
            });
        });
    }
}


using Mediator;
using MediFlow.Functions.Data.Services.PasswordHasher;
using MediFlow.Functions.Data.Services.UnitOfWork;
using MediFlow.Functions.Entities.Employees;
using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Entities.Users.Values;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.DeviceCookieAccessor;

namespace MediFlow.Functions.Modules.Auth.Commands;
using Result = Result<User, AuthErrors>;

public record RegisterUserCommand(
    string UserName,
    string UserSurname,
    string Email,
    string Password,
    InvitationId InvitationId) : ICommand<Result>;

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IEmployeeRepository emploeeRepository,
    IStructureRepository structureRepository,
    IInvitationRepository invitationRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IDeviceCookieAccessor deviceCookieAccessor) : ICommandHandler<RegisterUserCommand, Result>
{
    public async ValueTask<Result> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var invitation = await invitationRepository.GetOneAsync(command.InvitationId);
        if (invitation is null)
        {
            return AuthErrors.InvitationInvalid;
        }

        var hasDeviceKey = deviceCookieAccessor.TryGetCookie(out var deviceKey);
        if (!hasDeviceKey)
        {
            return AuthErrors.NoDeviceKey;
        }

        var structure = await structureRepository.GetOneAsync(deviceKey);
        if (structure is null)
        {
            await deviceCookieAccessor.DeleteCookieAsync();
            return AuthErrors.StructurByDeviceKeyNotFound;
        }

        if (invitation.StructureId != structure.Id)
        {
            return AuthErrors.InvitationDoesntMatchStructure;
        }

        var emailTaken = await userRepository.GetOneAsync(command.Email);
        if (emailTaken is not null)
        {
            return AuthErrors.EmailAlreadyTaken;
        }

        var user = new User
        {
            Name = command.UserName,
            Surname = command.UserSurname,
            PasswordHash = passwordHasher.Generate(command.Password),
            Email = command.Email,
            Role = UserRoles.Employee,
        };

        var employee = new Employee()
        {
            UserId = user.Id,
            StructureId = structure.Id,
            Role = invitation.Role,
        };

        userRepository.Add(user);
        emploeeRepository.Add(employee);
        await unitOfWork.SaveChangesAsync();
        await invitationRepository.DeleteAsync(invitation.Id);

        return user;
    }
}

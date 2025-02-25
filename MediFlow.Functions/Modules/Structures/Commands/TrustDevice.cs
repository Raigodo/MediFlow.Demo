using Mediator;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Structures.Commands;
using Result = Result<Unit, AuthErrors>;

public record TrustDeviceCommand() : ICommand<Result>;

public class TrustDeviceCommandHandler() : ICommandHandler<TrustDeviceCommand, Result>
{
    public async ValueTask<Result> Handle(TrustDeviceCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

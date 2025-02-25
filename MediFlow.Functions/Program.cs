using MediFlow.Functions.Modules.Auth;
using MediFlow.Functions.Modules.Clients;
using MediFlow.Functions.Modules.Invitations;
using MediFlow.Functions.Modules.Notes;
using MediFlow.Functions.Modules.Structures;
using MediFlow.Functions.Modules.Users;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((hostContext, services) =>
    {
        services
            .AddApplicationInsightsTelemetryWorkerService()
            .ConfigureFunctionsApplicationInsights();
        services
            .AddHttpContextAccessor()
            .AddMediator();
        services
            .AddAuthModule(hostContext.Configuration)
            .AddClientsModule()
            .AddInvitationsModule()
            .AddNotesModule()
            .AddStructuresModule()
            .AddUsersModule();
    })
    .Build();

host.Run();
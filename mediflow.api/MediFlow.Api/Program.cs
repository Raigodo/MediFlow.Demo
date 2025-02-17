using MediFlow.Api.Application.Auth;
using MediFlow.Api.Application.Extensions;
using MediFlow.Api.Application.Pipelines;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules.Auth;
using MediFlow.Api.Modules.Avatars;
using MediFlow.Api.Modules.Clients;
using MediFlow.Api.Modules.Employees;
using MediFlow.Api.Modules.Invitations;
using MediFlow.Api.Modules.Notes;
using MediFlow.Api.Modules.Structures;
using MediFlow.Api.Modules.Users;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    //.AddDevCors()
    .AddEndpointsApiExplorer()
    .AddSwaggerGenWithJwtAuthSupport()
    .AddHttpContextAccessor()
    .AddSessionDataAccessors()
    .ConfigureResponse()
    .AddRequestValidation();

builder.Services
    .AddDatabaseSupport(builder.Configuration)
    .AddScoped<IAccessGuard, AccessGuard>()
    .AddSingleton<ResponseFactory>()
    .AddTimeLimitedTokenSupport(builder.Configuration)
    .AddEnryptionSupport(builder.Configuration)
    .AddEmailSendingSupport();

builder.Services
    .AddAuthModule(builder.Configuration)
    .AddClientsModule()
    .AddInvitationsModule()
    .AddNotesModule()
    .AddStructuresModule()
    .AddUsersModule();

builder.Services
    .AddJwtAuthentication()
    .AddAuthorizationWithPolicies();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .UseHttpsRedirection()
    //.UseDevCors()
    .UseAuthentication()
    .UseMiddleware<GlobalExceptionHandler>()
    .UseAuthorization();

app.MapGroup("/")
    .DisableAntiforgery()
    .AddFluentValidationAutoValidation()
    .AddEndpointFilter<UserDataExtractorFilter>()
    .MapAuthEndpoints()
    .MapClientsEndpoints()
    .MapEmployeesEndpoints()
    .MapInvitationsEndpoints()
    .MapNotesEndpoints()
    .MapStructuresEndpoints()
    .MapUsersEndpoints()
    .MapAvatarsEndpoints();

app.EnsureDatabaseUpdated();

app.Run();

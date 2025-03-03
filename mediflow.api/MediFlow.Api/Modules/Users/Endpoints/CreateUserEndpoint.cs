﻿using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Data.Services.PasswordHasher;
using MediFlow.Api.Data.Services.UnitOfWork;
using MediFlow.Api.Entities.Users;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules.Users.Response;

namespace MediFlow.Api.Modules.Users.Endpoints;

public record CreateUserRequest(
    string UserName,
    string UserSurname,
    string Email,
    string Password,
    UserRoles UserRole);

public static class CreateUserEndpoint
{
    public static IEndpointRouteBuilder MapCreateUserEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/users", Handle)
            .RequireAuthorization(AuthPolicies.OnlyAdmin);
        return routes;
    }

    public static async Task<IResult> Handle(
        CreateUserRequest req,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ResponseFactory responseFactory)
    {
        var emailTaken = await userRepository.ExistsAsync(req.Email);
        if (emailTaken)
        {
            return responseFactory.Conflict<User>(nameof(req.Email));
        }

        var user = new User
        {
            Name = req.UserName,
            Surname = req.UserSurname,
            PasswordHash = passwordHasher.Generate(req.Password),
            Email = req.Email,
            Role = req.UserRole,
        };

        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync();

        return responseFactory.Ok(user.ToResponseDto());
    }
}

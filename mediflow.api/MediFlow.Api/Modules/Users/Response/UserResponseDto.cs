using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Modules.Users.Response;

public record UserResponseDto
{
    public required UserId Id { get; init; }
    public required UserRoles Role { get; init; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public required string Email { get; init; }
}

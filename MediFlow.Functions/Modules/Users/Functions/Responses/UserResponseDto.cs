using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Modules.Users.Functions.Responses;

public record UserResponseDto
{
    public required UserId Id { get; init; }
    public required UserRoles Role { get; init; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public required string Email { get; init; }
}

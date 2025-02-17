using MediFlow.Api.Entities.Employees;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Entities.Users;

public sealed class User
{
    public UserId Id { get; init; } = UserId.Generate();
    public required UserRoles Role { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string PasswordHash { get; set; }
    public required string Email { get; set; }
    public Guid RefreshToken { get; set; } = Guid.NewGuid();

#nullable disable
    public LastSession LastSession { get; init; }
    public List<Employee> Employments { get; init; } = [];
    public UserAvatar Avatar { get; init; }
#nullable restore
}


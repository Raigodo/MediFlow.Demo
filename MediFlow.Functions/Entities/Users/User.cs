using MediFlow.Functions.Entities.Employees;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Entities.Users;

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


using MediFlow.Api.Entities.Users;

namespace MediFlow.Api.Modules.Users.Response;

public static class ConverterExtensions
{
    public static IEnumerable<UserResponseDto> ToResponseDto(this IEnumerable<User> users)
    {
        return users.Select(u => u.ToResponseDto());
    }

    public static UserResponseDto ToResponseDto(this User user)
    {
        return new UserResponseDto()
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            Role = user.Role,
        };
    }
}

using FluentValidation;
using MediFlow.Api.Application.Converters;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Users.Values;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using System.Text.Json;

namespace MediFlow.Api.Application.Extensions;

public static class ApiInterfaceExtensions
{
    public static IServiceCollection ConfigureResponse(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new TypedIdConverterFactory());
            options.SerializerOptions.Converters.Add(new EnumToNullConverter<UserRoles>());
            options.SerializerOptions.Converters.Add(new EnumToNullConverter<EmployeeRoles>());
            options.SerializerOptions.Converters.Add(new CustomEnumConverterFactory());
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });
        return services;
    }
    public static IServiceCollection AddRequestValidation(this IServiceCollection services)
    {
        ValidatorOptions.Global.PropertyNameResolver = (type, member, expression) =>
        {
            var propertyName = member.Name;
            if (string.IsNullOrEmpty(propertyName) || char.IsLower(propertyName[0]))
                return propertyName;

            return char.ToLower(propertyName[0]) + propertyName.Substring(1);
        };
        services.AddFluentValidationAutoValidation();
        return services;
    }
}

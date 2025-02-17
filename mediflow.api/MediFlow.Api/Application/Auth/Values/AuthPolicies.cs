namespace MediFlow.Api.Application.Auth.Values;

public static class AuthPolicies
{
    public const string SessionRefresh = "refresh session";
    public const string SessionMutation = "mutate session";
    public const string Device = "valid device or manager plus";
    public const string OnlyAdmin = "admin only";
    public const string ManagerPlus = "manager plus";
    public const string EmployeePlus = "employee plus";
}

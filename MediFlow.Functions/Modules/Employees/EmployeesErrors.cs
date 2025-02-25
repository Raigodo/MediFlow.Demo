namespace MediFlow.Functions.Modules.Employees;

public enum EmployeesErrors
{
    None = 0,

    NoUser,
    NoDeviceKey,

    EmployeeNotFound,
    StructureNotFound,

    ForbidAccessToEmployee,
    ForbidAccessToStructure,
}

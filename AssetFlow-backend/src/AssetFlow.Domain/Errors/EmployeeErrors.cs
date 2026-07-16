using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.Errors;

public static class EmployeeErrors
{
    public static readonly Error EmployeeNumberEmpty =
        new("Employee.EmployeeNumberEmpty", "Employee number cannot be empty.");
    public static readonly Error EmployeeNumberTooLong =
        new("Employee.EmployeeNumberTooLong", "Employee number cannot exceed 50 characters.");

    public static readonly Error FirstNameEmpty =
        new("Employee.FirstNameEmpty", "First name cannot be empty.");
    public static readonly Error FirstNameTooLong =
        new("Employee.FirstNameTooLong", "First name cannot exceed 50 characters.");

    public static readonly Error LastNameEmpty =
        new("Employee.LastNameEmpty", "Last name cannot be empty.");
    public static readonly Error LastNameTooLong =
        new("Employee.LastNameTooLong", "Last name cannot exceed 50 characters.");

    public static readonly Error DepartmentEmpty =
        new("Employee.DepartmentEmpty", "Department cannot be empty.");
    public static readonly Error DepartmentTooLong =
        new("Employee.DepartmentTooLong", "Department cannot exceed 50 characters.");
    public static readonly Error AlreadyInactive =
        new("Employee.AlreadyInactive", "Employee is already inactive.");
    
    public static Error NotFound(Guid id) =>
        new ("Employee.NotFound", $"Employee not found with this id: '{id}'.");
    public static Error DuplicateEmployeeNumber(string number) =>
        new("Employee.DuplicateEmployeeNumber", $"Duplicated Employee Number: '{number}'");
}
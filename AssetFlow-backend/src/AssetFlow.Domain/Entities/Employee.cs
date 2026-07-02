using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;
using AssetFlow.Domain.ValueObjects;

namespace AssetFlow.Domain.Entities;

public class Employee
{
    public Guid Id { get; private set; }
    public string EmployeeNumber { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public EmailAddress Email { get; private set; } = null!;
    public string Department { get; private set; } = null!;
    public string? JobTitle { get; private set; }
    public bool IsActive { get; private set; }
    public DateOnly HireDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    [Obsolete("For EF Core only", error: true)]
    protected Employee()
    {
    }

    private Employee(string employeeNumber, string firstName, string lastName,
        EmailAddress email, string department, string? jobTitle, DateOnly hireDate)
    {
        Id = Guid.NewGuid();
        EmployeeNumber = employeeNumber;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Department = department;
        JobTitle = jobTitle;
        HireDate = hireDate;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public static Result<Employee> Create(string employeeNumber, string firstName, string lastName,
        EmailAddress email, string department, string? jobTitle, DateOnly hireDate)
    {
        var validation = ValidateCommon(employeeNumber, firstName, lastName, department);
        if (!validation.IsSuccess)
            return validation.Error!;

        return new Employee(employeeNumber.Trim(), firstName.Trim(), lastName.Trim(),
            email, department.Trim(), jobTitle?.Trim(), hireDate);
    }
    public Result Update(string firstName, string lastName, string department, string? jobTitle)
    {
        var validation = ValidateCommon(EmployeeNumber, firstName, lastName, department);
        if (!validation.IsSuccess)
            return validation.Error!;

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Department = department.Trim();
        JobTitle = jobTitle?.Trim();
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return EmployeeErrors.AlreadyInactive;

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    private static Result ValidateCommon(string employeeNumber, string firstName, string lastName,
        string department)
    {
        if (string.IsNullOrWhiteSpace(employeeNumber))
            return EmployeeErrors.EmployeeNumberEmpty;
        if (employeeNumber.Length > 50)
            return EmployeeErrors.EmployeeNumberTooLong;

        if (string.IsNullOrWhiteSpace(firstName))
            return EmployeeErrors.FirstNameEmpty;
        if (firstName.Length > 50)
            return EmployeeErrors.FirstNameTooLong;

        if (string.IsNullOrWhiteSpace(lastName))
            return EmployeeErrors.LastNameEmpty;
        if (lastName.Length > 50)
            return EmployeeErrors.LastNameTooLong;

        if (string.IsNullOrWhiteSpace(department))
            return EmployeeErrors.DepartmentEmpty;
        if (department.Length > 50)
            return EmployeeErrors.DepartmentTooLong;

        return Result.Success();
    }
}
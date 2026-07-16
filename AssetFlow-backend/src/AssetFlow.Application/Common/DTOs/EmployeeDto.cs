using AssetFlow.Domain.Entities;
using AssetFlow.Domain.ValueObjects;

namespace AssetFlow.Application.Common.DTOs;

public class EmployeeDto
{
    public Guid Id { get;  init; }
    public required string EmployeeNumber { get; init; }
    public required string FirstName { get; init; } 
    public required string LastName { get; init; } 
    public required string Email { get; init; } 
    public required string Department { get;  init; }
    public string? JobTitle { get;  init; }
    public bool IsActive { get;  init; }
    public DateOnly HireDate { get;  init; }
    public DateTime CreatedAt { get;  init; }
    public DateTime? UpdatedAt { get;  init; }

    public static EmployeeDto FromEntity(Employee employee) => new()
    {
        Id = employee.Id,
        EmployeeNumber = employee.EmployeeNumber,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Email = employee.Email.Value,
        Department = employee.Department,
        JobTitle = employee.JobTitle,
        IsActive = employee.IsActive,
        HireDate = employee.HireDate,
        CreatedAt = employee.CreatedAt,
        UpdatedAt = employee.UpdatedAt
    };
}
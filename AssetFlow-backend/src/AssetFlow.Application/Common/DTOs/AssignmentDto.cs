using AssetFlow.Domain.Entities;

namespace AssetFlow.Application.Common.DTOs;

public class AssignmentDto
{
    public Guid Id { get; init; }
    public Guid AssetId { get; init; }
    public string? AssetTag { get; init; }
    public string? AssetName { get; init; }
    public Guid EmployeeId { get; init; }
    public string? EmployeeName { get; init; }
    public DateOnly AssignedDate { get; init; }
    public DateOnly? ReturnedDate { get; init; }
    public required string ConditionAtAssign { get; init; }
    public string? ConditionAtReturn { get; init; }
    public string? Notes { get; init; }
    public string? ReturnNotes { get; init; }
    public bool IsActive { get; init; }

    public static AssignmentDto FromEntity(Assignment assignment) => new()
    {
        Id = assignment.Id,
        AssetId = assignment.AssetId,
        AssetTag = assignment.Asset?.Tag.Value,
        AssetName = assignment.Asset?.Name,
        EmployeeId = assignment.EmployeeId,
        EmployeeName = assignment.Employee is null
            ? null
            : $"{assignment.Employee.FirstName} {assignment.Employee.LastName}",
        AssignedDate = assignment.AssignedDate,
        ReturnedDate = assignment.ReturnedDate,
        ConditionAtAssign = assignment.ConditionAtAssign.ToString(),
        ConditionAtReturn = assignment.ConditionAtReturn?.ToString(),
        Notes = assignment.Notes,
        ReturnNotes = assignment.ReturnNotes,
        IsActive = assignment.IsActive
    };
}
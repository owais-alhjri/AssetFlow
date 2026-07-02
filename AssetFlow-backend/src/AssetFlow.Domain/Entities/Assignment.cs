using AssetFlow.Domain.Common;
using AssetFlow.Domain.Enums;
using AssetFlow.Domain.Errors;

namespace AssetFlow.Domain.Entities;

public class Assignment
{
    public Guid Id { get; private set; }
    public Guid AssetId { get; private set; }
    public Asset Asset { get; private set; } = null!;
    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = null!;
    public DateOnly AssignedDate { get; private set; }
    public DateOnly? ReturnedDate { get; private set; }
    public AssetCondition ConditionAtAssign { get; private set; }
    public AssetCondition? ConditionAtReturn { get; private set; }
    public string? Notes { get; private set; }
    public string? ReturnNotes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    [Obsolete("For EF Core only", error: true)]
    protected Assignment() { }

    private Assignment(Guid assetId, Guid employeeId, DateOnly assignedDate,
        AssetCondition conditionAtAssign, string? notes)
    {
        Id = Guid.NewGuid();
        AssetId = assetId;
        EmployeeId = employeeId;
        AssignedDate = assignedDate;
        ConditionAtAssign = conditionAtAssign;
        Notes = notes;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public static Result<Assignment> Create(Guid assetId, Guid employeeId, DateOnly assignedDate,
        AssetCondition conditionAtAssign, string? notes)
    {
        return new Assignment(assetId, employeeId, assignedDate, conditionAtAssign, notes?.Trim());
    }

    public Result Return(DateOnly returnedDate, AssetCondition conditionAtReturn, string? returnNotes)
    {
        if (ReturnedDate is not null)
            return AssignmentErrors.AlreadyReturned;

        if (returnedDate < AssignedDate)
            return AssignmentErrors.ReturnDateBeforeAssignedDate;

        ReturnedDate = returnedDate;
        ConditionAtReturn = conditionAtReturn;
        ReturnNotes = returnNotes?.Trim();
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public bool IsActive => ReturnedDate is null;
}

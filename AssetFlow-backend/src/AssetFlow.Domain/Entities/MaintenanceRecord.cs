using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;
using AssetFlow.Domain.ValueObjects;

namespace AssetFlow.Domain.Entities;

public class MaintenanceRecord
{
    public Guid Id { get; private set; }
    public Guid AssetId { get; private set; }
    public Asset Asset { get; private set; } = null!;
    public Guid TypeId { get; private set; }
    public MaintenanceType Type { get; private set; } = null!;
    public DateOnly ScheduledDate { get; private set; }
    public DateOnly? CompletedDate { get; private set; }
    public Money Cost { get; private set; } = null!;
    public string? PerformedBy { get; private set; }
    public string? Notes { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    [Obsolete("For EF Core only", error: true)]
    protected MaintenanceRecord() { }

    private MaintenanceRecord(Guid assetId, Guid typeId, DateOnly scheduledDate, Money cost,
        string? performedBy, string? notes)
    {
        Id = Guid.NewGuid();
        AssetId = assetId;
        TypeId = typeId;
        ScheduledDate = scheduledDate;
        Cost = cost;
        PerformedBy = performedBy;
        Notes = notes;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public static Result<MaintenanceRecord> Create(Guid assetId, Guid typeId, DateOnly scheduledDate,
        Money cost, string? performedBy, string? notes)
    {
        return new MaintenanceRecord(assetId, typeId, scheduledDate, cost, performedBy?.Trim(), notes?.Trim());
    }

    public Result Complete(DateOnly completedDate, Money finalCost, string? performedBy)
    {
        if (IsCompleted)
            return MaintenanceRecordErrors.AlreadyCompleted;

        if (completedDate < ScheduledDate)
            return MaintenanceRecordErrors.CompletedDateBeforeScheduledDate;

        CompletedDate = completedDate;
        Cost = finalCost;
        PerformedBy = performedBy?.Trim();
        IsCompleted = true;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }
}

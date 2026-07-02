using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.Errors;

public static class MaintenanceRecordErrors
{
    public static readonly Error AlreadyCompleted =
        new("MaintenanceRecord.AlreadyCompleted", "This maintenance record is already completed.");

    public static readonly Error CompletedDateBeforeScheduledDate =
        new("MaintenanceRecord.CompletedDateBeforeScheduledDate", "Completed date cannot be before the scheduled date.");
}
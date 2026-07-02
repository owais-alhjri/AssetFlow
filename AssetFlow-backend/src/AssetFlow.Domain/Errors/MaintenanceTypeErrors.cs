using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.Errors;

public static class MaintenanceTypeErrors
{
    public static readonly Error NameEmpty =
        new("MaintenanceType.NameEmpty", "Maintenance type name cannot be empty.");
    public static readonly Error NameTooLong =
        new("MaintenanceType.NameTooLong", "Maintenance type name cannot exceed 50 characters.");
    public static readonly Error AlreadyInactive =
        new("MaintenanceType.AlreadyInactive", "Maintenance type is already inactive.");
}
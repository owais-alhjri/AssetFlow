using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.Errors;

public static class AssetStatusErrors
{
    public static readonly Error NameEmpty =
        new("AssetStatus.NameEmpty", "Asset status name cannot be empty.");
    public static readonly Error NameTooLong =
        new("AssetStatus.NameTooLong", "Asset status name cannot exceed 50 characters.");
    public static readonly Error AlreadyInactive =
        new("AssetStatus.AlreadyInactive", "Asset status is already inactive.");
}
using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.Errors;

public static class AssetErrors
{
    public static readonly Error NameEmpty =
        new("Asset.NameEmpty", "Asset name cannot be empty.");

    public static readonly Error NameTooLong =
        new("Asset.NameTooLong", "Asset name cannot exceed 200 characters.");

    public static readonly Error AlreadyRetired =
        new("Asset.AlreadyRetired", "Asset is already retired.");

    public static readonly Error CannotAssignUnavailableAsset =
        new("Asset.CannotAssignUnavailableAsset", "Asset is not available for assignment.");
}
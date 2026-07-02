using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.Errors;

public static class AssetAlertErrors
{
    public static readonly Error MessageEmpty =
        new("AssetAlert.MessageEmpty", "Alert message cannot be empty.");

    public static readonly Error AlreadyAcknowledged =
        new("AssetAlert.AlreadyAcknowledged", "This alert has already been acknowledged.");
}
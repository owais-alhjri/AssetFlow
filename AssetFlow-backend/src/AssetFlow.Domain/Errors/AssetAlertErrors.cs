using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.Errors;

public static class AssetAlertErrors
{
    public static readonly Error MessageEmpty =
        new("AssetAlert.MessageEmpty", "Alert message cannot be empty.");

    public static readonly Error CannotAcknowledgeFromCurrentStatus =
        new("AssetAlert.CannotAcknowledgeFromCurrentStatus", "Only pending alerts can be acknowledged.");

    public static readonly Error AlreadyClosed =
        new("AssetAlert.AlreadyClosed", "This alert has already been resolved or dismissed.");
}
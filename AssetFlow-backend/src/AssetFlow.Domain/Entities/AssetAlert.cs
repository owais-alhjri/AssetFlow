using AssetFlow.Domain.Common;
using AssetFlow.Domain.Enums;
using AssetFlow.Domain.Errors;

namespace AssetFlow.Domain.Entities;

public class AssetAlert
{
    public Guid Id { get; private set; }
    public Guid AssetId { get; private set; }
    public Asset Asset { get; private set; } = null!;
    public AlertType Type { get; private set; }
    public AlertStatus Status { get; private set; }
    public DateOnly TriggerDate { get; private set; }
    public string Message { get; private set; } = null!;
    public DateTime? AcknowledgedAt { get; private set; }
    public Guid? AcknowledgedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }

    [Obsolete("For EF Core only", error: true)]
    protected AssetAlert() { }

    private AssetAlert(Guid assetId, AlertType type, DateOnly triggerDate, string message)
    {
        Id = Guid.NewGuid();
        AssetId = assetId;
        Type = type;
        Status = AlertStatus.Active;
        TriggerDate = triggerDate;
        Message = message;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<AssetAlert> Create(Guid assetId, AlertType type, DateOnly triggerDate, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return AssetAlertErrors.MessageEmpty;

        return new AssetAlert(assetId, type, triggerDate, message.Trim());
    }

    public Result Acknowledge(Guid acknowledgedByUserId)
    {
        if (Status == AlertStatus.Acknowledged)
            return AssetAlertErrors.AlreadyAcknowledged;

        Status = AlertStatus.Acknowledged;
        AcknowledgedAt = DateTime.UtcNow;
        AcknowledgedBy = acknowledgedByUserId;

        return Result.Success();
    }
}


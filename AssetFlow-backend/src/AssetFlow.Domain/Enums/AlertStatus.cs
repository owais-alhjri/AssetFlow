using System.ComponentModel;

namespace AssetFlow.Domain.Enums;

public enum AlertStatus
{
    [Description("Pending")] Pending,
    [Description("Acknowledged")] Acknowledged,
    [Description("Resolved")] Resolved,
    [Description("Dismissed")] Dismissed 
}
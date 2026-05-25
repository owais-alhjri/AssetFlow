using System.ComponentModel;

namespace AssetFlow.Domain.Enums;

public enum AssetCondition
{
    [Description("Excellent")] Excellent,
    [Description("Good")] Good,
    [Description("Fair")] Fair,
    [Description("Poor")] Poor,
    [Description("Damaged")] Damaged
}
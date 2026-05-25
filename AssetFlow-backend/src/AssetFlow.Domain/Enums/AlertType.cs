using System.ComponentModel;

namespace AssetFlow.Domain.Enums;

public enum AlertType
{
    [Description("Maintenance Due")] MaintenanceDue,
    [Description("Warranty Expiring")] WarrantyExpiring,
    [Description("Asset Overdue")] AssetOverdue,
    [Description("Low Stock")] LowStock 
}
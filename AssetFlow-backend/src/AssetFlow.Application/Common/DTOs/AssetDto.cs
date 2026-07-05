namespace AssetFlow.Application.Common.DTOs;

public class AssetDto
{
    public Guid Id { get; init; }
    public string Tag { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string SerialNumber { get; init; } = default!;
    public Guid CategoryId { get; init; }
    public Guid StatusId { get; init; }
    public string Condition { get; init; } = default!;
    public DateOnly PurchaseDate { get; init; }
    public decimal PurchasePrice { get; init; }
    public string Currency { get; init; } = default!;
    public DateOnly? WarrantyExpiryDate { get; init; }
    public DateOnly? NextMaintenanceDate { get; init; }
    public string? Location { get; init; }
    public string? Notes { get; init; }
    public DateTime CreatedAt { get; init; }
}
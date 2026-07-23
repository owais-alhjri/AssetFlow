using AssetFlow.Domain.Entities;

namespace AssetFlow.Application.Common.DTOs;

public class AssetDto
{
    public Guid Id { get; init; }
    public required string Tag { get; init; } 
    public required string Name { get; init; } 
    public required string SerialNumber { get; init; } 
    public Guid CategoryId { get; init; }
    public Guid StatusId { get; init; }
    public required string Condition { get; init; } 
    public DateOnly PurchaseDate { get; init; }
    public decimal PurchasePrice { get; init; }
    public required string Currency { get; init; } 
    public DateOnly? WarrantyExpiryDate { get; init; }
    public DateOnly? NextMaintenanceDate { get; init; }
    public string? Location { get; init; }
    public string? Notes { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public required string CategoryName { get; init; }
    public required string StatusName { get; init; }

    public static AssetDto FromEntity(Asset asset) => new()
    {
        Id = asset.Id,
        Tag = asset.Tag.Value,
        Name = asset.Name,
        SerialNumber = asset.SerialNumber.Value,
        CategoryId = asset.CategoryId,
        StatusId = asset.StatusId,
        Condition = asset.Condition.ToString(),
        PurchaseDate = asset.PurchaseDate,
        PurchasePrice = asset.PurchasePrice.Amount,
        Currency = asset.PurchasePrice.Currency,
        WarrantyExpiryDate = asset.WarrantyExpiryDate,
        NextMaintenanceDate = asset.NextMaintenanceDate,
        Location = asset.Location,
        Notes = asset.Notes,
        CreatedAt = asset.CreatedAt,
        UpdatedAt = asset.UpdatedAt,
        CategoryName = asset.Category.Name,
        StatusName = asset.Status.Name
    };
}
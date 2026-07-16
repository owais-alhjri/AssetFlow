using AssetFlow.Domain.Common;
using AssetFlow.Domain.Enums;
using AssetFlow.Domain.Errors;
using AssetFlow.Domain.ValueObjects;

namespace AssetFlow.Domain.Entities;

public class Asset
{
    public Guid Id { get; private set; }
    public AssetTag Tag { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public SerialNumber SerialNumber { get; private set; } = null!;
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public Guid StatusId { get; private set; }
    public AssetStatus Status { get; private set; } = null!;
    public AssetCondition Condition { get; private set; }
    public DateOnly PurchaseDate { get; private set; }
    public Money PurchasePrice { get; private set; } = null!;
    public DateOnly? WarrantyExpiryDate { get; private set; }
    public DateOnly? NextMaintenanceDate { get; private set; }
    public string? Location { get; private set; }
    public string? Notes { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    [Obsolete("For EF Core only", error: true)]
    protected Asset()
    {
    }

    private Asset(
        AssetTag tag,
        string name,
        SerialNumber serialNumber,
        Guid categoryId,
        Guid statusId,
        AssetCondition condition,
        DateOnly purchaseDate,
        Money purchasePrice,
        DateOnly? warrantyExpiryDate,
        DateOnly? nextMaintenanceDate,
        string? location,
        string? notes)
    {
        Id = Guid.NewGuid();
        Tag = tag;
        Name = name;
        SerialNumber = serialNumber;
        CategoryId = categoryId;
        StatusId = statusId;
        Condition = condition;
        PurchaseDate = purchaseDate;
        PurchasePrice = purchasePrice;
        WarrantyExpiryDate = warrantyExpiryDate;
        NextMaintenanceDate = nextMaintenanceDate;
        Location = location;
        Notes = notes;
        IsDeleted = false;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public static Result<Asset> Create(
        AssetTag tag,
        string name,
        SerialNumber serialNumber,
        Guid categoryId,
        Guid statusId,
        AssetCondition condition,
        DateOnly purchaseDate,
        Money purchasePrice,
        DateOnly? warrantyExpiryDate,
        DateOnly? nextMaintenanceDate,
        string? location,
        string? notes)
    {
        var nameResult = ValidateName(name);
        if (!nameResult.IsSuccess)
            return nameResult.Error!;

        return new Asset(
            tag, name.Trim(), serialNumber, categoryId, statusId, condition,
            purchaseDate, purchasePrice, warrantyExpiryDate, nextMaintenanceDate,
            location?.Trim(), notes?.Trim());
    }

    public Result Update(
        string name,
        AssetCondition condition,
        DateOnly? warrantyExpiryDate,
        DateOnly? nextMaintenanceDate,
        string? location,
        string? notes)
    {
        var nameResult = ValidateName(name);
        if (!nameResult.IsSuccess)
            return nameResult.Error!;
        Name = name;
        Condition = condition;
        WarrantyExpiryDate = warrantyExpiryDate;
        NextMaintenanceDate = nextMaintenanceDate;
        Location = location;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    private static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return AssetErrors.NameEmpty;
        if (name.Length > 200)
            return AssetErrors.NameTooLong;
        return Result.Success();
    }

    public void Delete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public Result Assign(Guid assignedStatusId)
    {
        StatusId = assignedStatusId;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result Return(Guid availableStatusId)
    {
        StatusId = availableStatusId;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}

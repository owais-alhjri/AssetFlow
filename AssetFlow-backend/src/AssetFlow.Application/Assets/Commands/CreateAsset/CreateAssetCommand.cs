using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Enums;
using MediatR;

namespace AssetFlow.Application.Assets.Commands.CreateAsset;
public record CreateAssetCommand(
    string Tag,
    string Name,
    string SerialNumber,
    Guid CategoryId,
    AssetCondition Condition,
    DateOnly PurchaseDate,
    decimal PurchasePrice,
    string Currency,
    DateOnly? WarrantyExpiryDate,
    DateOnly? NextMaintenanceDate,
    string? Location,
    string? Notes
) : IRequest<Result<AssetDto>>;
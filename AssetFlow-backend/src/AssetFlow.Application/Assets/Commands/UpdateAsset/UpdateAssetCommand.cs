using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Enums;
using MediatR;

namespace AssetFlow.Application.Assets.Commands.UpdateAsset;

public record UpdateAssetCommand(
    Guid Id,
    string Name,
    AssetCondition Condition,
    DateOnly? WarrantyExpiryDate,
    DateOnly? NextMaintenanceDate,
    string? Location,
    string? Notes
    ): IRequest<Result<AssetDto>>;
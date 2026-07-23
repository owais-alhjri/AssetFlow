using AssetFlow.Application.Common.DTOs;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Entities;
using AssetFlow.Domain.Errors;  
using AssetFlow.Domain.ValueObjects;
using MediatR;

namespace AssetFlow.Application.Assets.Commands.CreateAsset;

public class CreateAssetCommandHandler(
    IAssetRepository assetRepository,
    IAssetStatusRepository assetStatusRepository,
    IUnitOfWork unitOfWork)
        : IRequestHandler<CreateAssetCommand, Result<AssetDto>>
{
    public async Task<Result<AssetDto>> Handle(
        CreateAssetCommand request,
        CancellationToken cancellationToken
    )
    {
        // 1. Rehydrate value objects — each returns Result<T>, so check each.
        var tagResult = AssetTag.Create(request.Tag);
        if (!tagResult.IsSuccess)
            return tagResult.Error!;

        var serialResult = SerialNumber.Create(request.SerialNumber);
        if (!serialResult.IsSuccess)
            return serialResult.Error!;

        var priceResult = Money.Create(request.PurchasePrice, request.Currency);
        if (!priceResult.IsSuccess)
            return priceResult.Error!;

        // 2. DB-dependent business rule: tag must be unique.
        var tagExists = await assetRepository.ExistsByTagAsync(
            request.Tag, cancellationToken);
        if (tagExists)
            return AssetErrors.DuplicateTag(request.Tag);
        // 3. Resolve the default "Available" status from the lookup table.
        var statusId = await assetStatusRepository.GetIdByNameAsync(
            "Available", cancellationToken);
        if (statusId is null)
            return AssetErrors.DefaultStatusMissing;

        // 4. Create the entity — its factory validates the name and returns Result<Asset>.
        var assetResult = Asset.Create(
            tagResult.Value!,
            request.Name,
            serialResult.Value!,
            request.CategoryId,
            statusId.Value,
            request.Condition,
            request.PurchaseDate,
            priceResult.Value!,
            request.WarrantyExpiryDate,
            request.NextMaintenanceDate,
            request.Location,
            request.Notes);
        if (!assetResult.IsSuccess)
            return assetResult.Error!;

        var asset = assetResult.Value!;

        // 5. Persist and commit.
        await assetRepository.AddAsync(asset, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 6. Re-load with Category/Status navigations so the DTO can map their names.
        var saved = await assetRepository.GetByIdAsync(asset.Id, cancellationToken);
        return AssetDto.FromEntity(saved!);
    }
}
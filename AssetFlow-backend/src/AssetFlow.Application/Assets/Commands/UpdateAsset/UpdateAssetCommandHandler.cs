using AssetFlow.Application.Common.DTOs;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;
using MediatR;

namespace AssetFlow.Application.Assets.Commands.UpdateAsset;

public class UpdateAssetCommandHandler(
    IAssetRepository assetRepository,
    IUnitOfWork unitOfWork
    ): IRequestHandler<UpdateAssetCommand, Result<AssetDto>>
{
    public async Task<Result<AssetDto>> Handle(
        UpdateAssetCommand request,
        CancellationToken cancellationToken
        )
    {
        var asset = await assetRepository.GetByIdAsync(request.Id, cancellationToken);
        if (asset is null)
            return AssetErrors.NotFound(request.Id);
        var updateResult = asset.Update(
            request.Name,
            request.Condition,
            request.WarrantyExpiryDate,
            request.NextMaintenanceDate,
            request.Location,
            request.Notes
        );
        if (!updateResult.IsSuccess)
            return updateResult.Error!;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return AssetDto.FromEntity(asset);
    }
}
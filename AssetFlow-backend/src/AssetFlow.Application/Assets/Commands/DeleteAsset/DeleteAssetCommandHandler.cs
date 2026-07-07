using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;
using MediatR;

namespace AssetFlow.Application.Assets.Commands.DeleteAsset;

public class DeleteAssetCommandHandler(
    IAssetRepository assetRepository,
    IAssetStatusRepository assetStatusRepository,
    IUnitOfWork unitOfWork
    ): IRequestHandler<DeleteAssetCommand, Result>
{
    public async Task<Result> Handle(
        DeleteAssetCommand request,
        CancellationToken cancellationToken
    )
    {
        var asset = await assetRepository.GetByIdAsync(request.Id, cancellationToken);
        if (asset is null)
            return AssetErrors.NotFound(request.Id);
        var assignedStatusId = await assetStatusRepository.GetIdByNameAsync("Assigned", cancellationToken);
        if (asset.StatusId == assignedStatusId)
            return AssetErrors.CannotDeleteAssigned;
        

        asset.Delete();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
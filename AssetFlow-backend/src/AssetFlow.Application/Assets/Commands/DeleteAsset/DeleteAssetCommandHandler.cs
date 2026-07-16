using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;
using MediatR;

namespace AssetFlow.Application.Assets.Commands.DeleteAsset;

public class DeleteAssetCommandHandler(
    IAssetRepository assetRepository,
    IAssignmentRepository assignmentRepository ,
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

        var active = await assignmentRepository.GetActiveByAssetIdAsync(request.Id, cancellationToken);
        if (active is not null)
            return AssetErrors.CannotDeleteAssigned;


        asset.Delete();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
using AssetFlow.Application.Common.DTOs;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;
using MediatR;

namespace AssetFlow.Application.Assignments.Commands.ReturnAsset;

public class ReturnAssetCommandHandler(
    IAssignmentRepository assignmentRepository,
    IAssetRepository assetRepository,
    IAssetStatusRepository assetStatusRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<ReturnAssetCommand, Result<AssignmentDto>>
{
    public async Task<Result<AssignmentDto>> Handle(
        ReturnAssetCommand request,
        CancellationToken ct)
    {
        var assignment = await assignmentRepository.GetByIdAsync(request.AssignmentId, ct);
        if (assignment is null)
            return AssignmentErrors.NotFound(request.AssignmentId);

        var returnResult = assignment.Return(
            request.ReturnedDate,
            request.ConditionAtReturn,
            request.ReturnNotes);
        if (!returnResult.IsSuccess)
            return returnResult.Error!;

        var availableStatusId = await assetStatusRepository.GetIdByNameAsync("Available", ct);
        if (availableStatusId is null)
            return AssetStatusErrors.NotFoundByName("Available");

        var asset = await assetRepository.GetByIdAsync(assignment.AssetId, ct);
        if (asset is null)
            return AssetErrors.NotFound(assignment.AssetId);

        asset.Return(availableStatusId.Value);
        await unitOfWork.SaveChangesAsync(ct);

        return AssignmentDto.FromEntity(assignment);
    }
}
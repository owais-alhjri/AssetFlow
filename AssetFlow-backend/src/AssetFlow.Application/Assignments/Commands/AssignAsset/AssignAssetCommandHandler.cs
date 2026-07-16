using AssetFlow.Application.Common.DTOs;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Entities;
using AssetFlow.Domain.Errors;
using MediatR;

namespace AssetFlow.Application.Assignments.Commands.AssignAsset;

public class AssignAssetCommandHandler(
    IAssetRepository assetRepository,
    IEmployeeRepository employeeRepository,
    IAssignmentRepository assignmentRepository,
    IAssetStatusRepository assetStatusRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<AssignAssetCommand, Result<AssignmentDto>>
{
    public async Task<Result<AssignmentDto>> Handle(
        AssignAssetCommand request,
        CancellationToken ct)
    {
        var asset = await assetRepository.GetByIdAsync(request.AssetId, ct);
        if (asset is null)
            return AssetErrors.NotFound(request.AssetId);

        var employee = await employeeRepository.GetByIdAsync(request.EmployeeId, ct);
        if (employee is null)
            return EmployeeErrors.NotFound(request.EmployeeId);

        var active = await assignmentRepository.GetActiveByAssetIdAsync(request.AssetId, ct);
        if (active is not null)
            return AssignmentErrors.AssetAlreadyAssigned;

        var assignedStatusId = await assetStatusRepository.GetIdByNameAsync("Assigned", ct);
        if (assignedStatusId is null)
            return AssetStatusErrors.NotFoundByName("Assigned");

        var assignmentResult = Assignment.Create(
            request.AssetId,
            request.EmployeeId,
            request.AssignedDate,
            request.ConditionAtAssign,
            request.Notes);
        if (!assignmentResult.IsSuccess)
            return assignmentResult.Error!;

        var assignment = assignmentResult.Value!;

        asset.Assign(assignedStatusId.Value);
        await assignmentRepository.AddAsync(assignment, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return AssignmentDto.FromEntity(assignment);
    }
}
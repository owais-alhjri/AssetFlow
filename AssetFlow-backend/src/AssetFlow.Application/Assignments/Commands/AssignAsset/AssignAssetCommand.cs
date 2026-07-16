using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Enums;
using MediatR;

namespace AssetFlow.Application.Assignments.Commands.AssignAsset;

public record AssignAssetCommand(
    Guid AssetId,
    Guid EmployeeId,
    DateOnly AssignedDate,
    AssetCondition ConditionAtAssign,
    string? Notes
) : IRequest<Result<AssignmentDto>>;
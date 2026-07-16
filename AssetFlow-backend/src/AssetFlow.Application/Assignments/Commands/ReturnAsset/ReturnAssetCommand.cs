using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Enums;
using MediatR;

namespace AssetFlow.Application.Assignments.Commands.ReturnAsset;

public record ReturnAssetCommand(
    Guid AssignmentId,
    DateOnly ReturnedDate,
    AssetCondition ConditionAtReturn,
    string? ReturnNotes
) : IRequest<Result<AssignmentDto>>;
using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Assignments.Queries.GetActiveAssignments;

public sealed record GetActiveAssignmentsQuery : IRequest<Result<IReadOnlyList<AssignmentDto>>>;
using AssetFlow.Application.Common.DTOs;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Assignments.Queries.GetActiveAssignments;

public class GetActiveAssignmentsQueryHandler(IAssignmentRepository assignmentRepository)
    : IRequestHandler<GetActiveAssignmentsQuery, Result<IReadOnlyList<AssignmentDto>>>
{
    public async Task<Result<IReadOnlyList<AssignmentDto>>> Handle(
        GetActiveAssignmentsQuery query,
        CancellationToken ct)
    {
        var assignments = await assignmentRepository.GetActiveAssignmentsAsync(ct);
        var items = assignments.Select(AssignmentDto.FromEntity).ToList();
        return items;
    }
}
using AssetFlow.Domain.Entities;

namespace AssetFlow.Application.Common.Interfaces;

public interface IAssignmentRepository
{
    Task AddAsync(Assignment assignment, CancellationToken ct);
    Task<Assignment?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Assignment?> GetActiveByAssetIdAsync(Guid assetId, CancellationToken ct);
    Task<IReadOnlyList<Assignment>> GetActiveAssignmentsAsync(CancellationToken ct);
}
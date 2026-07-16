using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetFlow.Infrastructure.Data.Repositories;

public class AssignmentRepository(AssetFlowDbContext assetFlowDb) : IAssignmentRepository
{
    public async Task AddAsync(Assignment assignment, CancellationToken ct)
    {
        await assetFlowDb.Assignments.AddAsync(assignment, ct);
    }

    // Tracked on purpose — ReturnAsset mutates this entity and saves it.
    public async Task<Assignment?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await assetFlowDb.Assignments
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    }

    public async Task<Assignment?> GetActiveByAssetIdAsync(Guid assetId, CancellationToken ct)
    {
        return await assetFlowDb.Assignments
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AssetId == assetId && a.ReturnedDate == null, ct);
    }

    public async Task<IReadOnlyList<Assignment>> GetActiveAssignmentsAsync(CancellationToken ct)
    {
        return await assetFlowDb.Assignments
            .AsNoTracking()
            .Include(a => a.Asset)
            .Include(a => a.Employee)
            .Where(a => a.ReturnedDate == null)
            .OrderByDescending(a => a.AssignedDate)
            .ToListAsync(ct);
    }
}
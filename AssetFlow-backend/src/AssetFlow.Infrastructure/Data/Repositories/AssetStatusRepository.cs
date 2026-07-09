using AssetFlow.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AssetFlow.Infrastructure.Data.Repositories;

public class AssetStatusRepository(AssetFlowDbContext assetFlowDb) : IAssetStatusRepository
{
    public async Task<Guid?> GetIdByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await assetFlowDb.AssetStatuses
            .Where(s => s.Name == name)
            .Select(s => (Guid?)s.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
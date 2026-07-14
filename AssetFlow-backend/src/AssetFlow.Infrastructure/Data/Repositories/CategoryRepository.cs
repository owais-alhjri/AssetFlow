using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetFlow.Infrastructure.Data.Repositories;

public class CategoryRepository(AssetFlowDbContext assetFlowDb) : ICategoryRepository
{
    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct)
    {
        return  await assetFlowDb.Categories
            .AsNoTracking()
            .OrderBy(c=>c.Name)
            .ToListAsync(ct);
    }
}
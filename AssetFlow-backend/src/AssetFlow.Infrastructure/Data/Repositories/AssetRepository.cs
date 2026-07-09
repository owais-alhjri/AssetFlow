using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetFlow.Infrastructure.Data.Repositories;

public class AssetRepository(AssetFlowDbContext assetFlowDb) : IAssetRepository
{
    public async Task AddAsync(Asset asset, CancellationToken cancellationToken)
    {
        await assetFlowDb.Assets.AddAsync(asset, cancellationToken);
    }

    public Task<bool> ExistsByTagAsync(string assetTag, CancellationToken cancellationToken)
    {
        return assetFlowDb.Assets
            .IgnoreQueryFilters()
            .AnyAsync(x => x.Tag.Value == assetTag, cancellationToken);
        
    }

    public async Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await assetFlowDb.Assets
            .Include(a => a.Status)
            .Include(a => a.Category)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        
    }

    public async Task<(IReadOnlyList<Asset> items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        Guid? statusId,
        Guid? categoryId,
        CancellationToken cancellationToken)
    {
        IQueryable<Asset> query = assetFlowDb.Assets
            .Include(a => a.Status)
            .Include(a =>a.Category);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(a =>
                EF.Functions.ILike(a.Name, $"%{search}%") ||
                EF.Functions.ILike(a.Tag.Value, $"%{search}%"));
        }
        
        // Filter by status
        if (statusId.HasValue)
        {
            query = query.Where(a => a.StatusId == statusId);
        }

        // Filter by category
        if (categoryId.HasValue)
        {
            query = query.Where(a => a.CategoryId == categoryId);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        // Requested Page
        var item = await query
            .OrderBy(a => a.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (item, totalCount);
    }
}
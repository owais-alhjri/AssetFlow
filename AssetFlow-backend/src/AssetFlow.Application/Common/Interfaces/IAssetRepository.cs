using AssetFlow.Domain.Entities;

namespace AssetFlow.Application.Common.Interfaces;

public interface IAssetRepository
{
    Task AddAsync(Asset asset, CancellationToken cancellationToken);
    Task<bool> ExistsByTagAsync(string assetTag, CancellationToken cancellationToken);
    Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<(IReadOnlyList<Asset> items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? statusId,
        Guid? categoryId,
        CancellationToken cancellationToken);
}
using AssetFlow.Domain.Entities;

namespace AssetFlow.Application.Common.Interfaces;

public interface IAssetRepository
{
    Task AddAsync(Asset asset, CancellationToken cancellationToken);
    Task<bool> ExistsByTagAsync(string assetTag, CancellationToken cancellationToken);
}
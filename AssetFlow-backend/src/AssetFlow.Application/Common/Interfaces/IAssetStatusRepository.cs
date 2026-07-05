namespace AssetFlow.Application.Common.Interfaces;

public interface IAssetStatusRepository
{
    Task<Guid?> GetIdByNameAsync(string name, CancellationToken cancellationToken);
}
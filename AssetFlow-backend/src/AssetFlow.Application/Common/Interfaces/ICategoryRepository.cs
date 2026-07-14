using AssetFlow.Domain.Entities;

namespace AssetFlow.Application.Common.Interfaces;

public interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct);
}
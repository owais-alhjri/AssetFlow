using AssetFlow.Application.Common.DTOs;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;
using MediatR;

namespace AssetFlow.Application.Assets.Queries.GetAssets;

public class GetAssetByIdQueryHandler(IAssetRepository assetRepository)
    : IRequestHandler<GetAssetByIdQuery, Result<AssetDto>>
{
    public async Task<Result<AssetDto>> Handle(
        GetAssetByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        // 1. Fetch. The repo returns the asset or null — no exceptions.
        var asset = await assetRepository.GetByIdAsync(request.Id, cancellationToken);
        // 2. Translate "null" into a domain-meaningful failure.
        if (asset is null)
            return AssetErrors.NotFound(request.Id);
        // 3. Project entity → DTO. Same mapping you wrote in CreateAsset.
        return AssetDto.FromEntity(asset);
    }
}
using AssetFlow.Application.Common.DTOs;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Assets.Queries.GetAssets;

public class GetAssetsQueryHandler(IAssetRepository assetRepository)
: IRequestHandler<GetAssetsQuery, Result<PagedResult<AssetDto>>>
{
    public async Task<Result<PagedResult<AssetDto>>> Handle(
        GetAssetsQuery request,
        CancellationToken cancellationToken
        )
    {
        var (assets, totalCount) = await assetRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.Search,
            request.StatusId,
            request.CategoryId,
            cancellationToken);

        var items = assets.Select(AssetDto.FromEntity).ToList();
        return new PagedResult<AssetDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
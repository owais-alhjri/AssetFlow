using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Assets.Queries.GetAssets;

public record GetAssetsQuery(
    int PageNumber,
    int PageSize,
    string? Search,
    Guid? StatusId,
    Guid? CategoryId
    ): IRequest<Result<PagedResult<AssetDto>>>;
using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Assets.Queries.GetAssets;

public record GetAssetsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null,
    string? Status = null,
    Guid? CategoryId = null
    ): IRequest<Result<PagedResult<AssetDto>>>;
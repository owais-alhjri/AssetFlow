using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Assets.Queries.GetAssets;

public record GetAssetByIdQuery(Guid Id) : IRequest<Result<AssetDto>>;

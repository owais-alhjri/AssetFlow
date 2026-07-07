using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Assets.Commands.DeleteAsset;

public record DeleteAssetCommand(
    Guid Id
    ): IRequest<Result>;
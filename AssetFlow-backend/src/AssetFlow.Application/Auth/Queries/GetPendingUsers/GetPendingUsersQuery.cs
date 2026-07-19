using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Auth.Queries.GetPendingUsers;

public sealed record GetPendingUsersQuery : IRequest<Result<IReadOnlyList<PendingUserDto>>>;
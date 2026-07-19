using AssetFlow.Application.Common.DTOs;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Auth.Queries.GetPendingUsers;

public class GetPendingUsersQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetPendingUsersQuery, Result<IReadOnlyList<PendingUserDto>>>
{
    public async Task<Result<IReadOnlyList<PendingUserDto>>> Handle(
        GetPendingUsersQuery query, CancellationToken ct)
    {
        var users = await userRepository.GetPendingAsync(ct);
        var items = users.Select(PendingUserDto.FromEntity).ToList();
        return items;
    }
}
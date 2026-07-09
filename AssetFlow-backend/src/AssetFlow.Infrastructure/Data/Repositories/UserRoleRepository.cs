using AssetFlow.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AssetFlow.Infrastructure.Data.Repositories;

public class UserRoleRepository(AssetFlowDbContext assetFlowDb) : IUserRoleRepository
{
    public async Task<Guid?> GetIdByNameAsync(string roleName, CancellationToken cancellationToken)
    {
        return await assetFlowDb.UserRoles
            .Where(r => r.Name == roleName)
            .Select(r => (Guid?)r.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetFlow.Infrastructure.Data.Repositories;

public class UserRepository(AssetFlowDbContext assetFlowDb) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await assetFlowDb.Users.AddAsync(user, cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return assetFlowDb.Users
            .AnyAsync(u => u.Email.Value == normalized, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return await assetFlowDb.Users
            .Include(u => u.Role)              // ← without this, user.Role.Name throws on login
            .FirstOrDefaultAsync(u => u.Email.Value == normalized, cancellationToken);
    }
    public Task<bool> ExistsByEmployeeIdAsync(Guid employeeId, CancellationToken ct) =>
        assetFlowDb.Users.AnyAsync(u => u.EmployeeId == employeeId, ct);

}
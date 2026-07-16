using AssetFlow.Domain.Entities;

namespace AssetFlow.Application.Common.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<bool> ExistsByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken);
}
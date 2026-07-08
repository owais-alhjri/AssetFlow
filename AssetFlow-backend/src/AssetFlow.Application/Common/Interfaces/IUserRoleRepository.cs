namespace AssetFlow.Application.Common.Interfaces;

public interface IUserRoleRepository
{
    Task<Guid?> GetIdByNameAsync(string roleName, CancellationToken cancellationToken);
}
using AssetFlow.Domain.Entities;

namespace AssetFlow.Application.Common.Interfaces;

public interface IEmployeeRepository
{
    Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken ct);
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Employee employee, CancellationToken ct);
    Task<bool> ExistsByEmployeeNumberAsync(string employeeNumber, CancellationToken ct);
}
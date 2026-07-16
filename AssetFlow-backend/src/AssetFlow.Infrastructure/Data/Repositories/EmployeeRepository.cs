using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetFlow.Infrastructure.Data.Repositories;

public class EmployeeRepository(AssetFlowDbContext assetFlowDb) : IEmployeeRepository
{
    public async Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken ct)
    {
        return await assetFlowDb.Employees
            .AsNoTracking()
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync(ct);
    }

    public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await assetFlowDb.Employees
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public async Task AddAsync(Employee employee, CancellationToken ct)
    { 
        await assetFlowDb.Employees.AddAsync(employee, ct);
    }

    public async Task<bool> ExistsByEmployeeNumberAsync(string employeeNumber, CancellationToken ct)
    {
        return await assetFlowDb.Employees
            .AnyAsync(e => e.EmployeeNumber == employeeNumber, ct);
    }
    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct) =>
        assetFlowDb.Employees.AnyAsync(e => e.Id == id, ct);
}
using AssetFlow.Application.Common.DTOs;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Employees.Queries.GetEmployee;

public class GetEmployeesQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<GetEmployeesQuery, Result<IReadOnlyList<EmployeeDto>>>
{
    public async Task<Result<IReadOnlyList<EmployeeDto>>> Handle(
        GetEmployeesQuery query,
        CancellationToken ct
        )
    {
        var employees = await employeeRepository.GetAllAsync(ct);
        var items = employees.Select(EmployeeDto.FromEntity).ToList();
        return items;
    }
}
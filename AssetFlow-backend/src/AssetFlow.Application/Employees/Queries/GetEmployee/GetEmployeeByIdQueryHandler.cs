using AssetFlow.Application.Common.DTOs;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;
using MediatR;

namespace AssetFlow.Application.Employees.Queries.GetEmployee;

public class GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDto>>
{
    public async Task<Result<EmployeeDto>> Handle(
        GetEmployeeByIdQuery query,
        CancellationToken ct
        )
    {
        var employee = await employeeRepository.GetByIdAsync(query.Id, ct);

        if (employee is null)
            return EmployeeErrors.NotFound(query.Id);

        return EmployeeDto.FromEntity(employee);
    }
}
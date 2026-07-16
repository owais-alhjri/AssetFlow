using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Employees.Queries.GetEmployee;

public sealed record GetEmployeeByIdQuery(Guid Id) : IRequest<Result<EmployeeDto>>;

    

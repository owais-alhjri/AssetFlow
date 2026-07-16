using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string Email,
    string Department,
    string JobTitle,
    DateOnly HireDate
    ) : IRequest<Result<EmployeeDto>>;

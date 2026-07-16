using AssetFlow.Application.Common.DTOs;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Entities;
using AssetFlow.Domain.Errors;
using AssetFlow.Domain.ValueObjects;
using MediatR;

namespace AssetFlow.Application.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork
    ):IRequestHandler<CreateEmployeeCommand, Result<EmployeeDto>>
{
    public async Task<Result<EmployeeDto>> Handle(
        CreateEmployeeCommand request,
        CancellationToken ct
    )
    {
        var emailResult = EmailAddress.Create(request.Email);
        if (!emailResult.IsSuccess)
            return emailResult.Error!;

        var employeeNumberExists = await employeeRepository.ExistsByEmployeeNumberAsync(request.EmployeeNumber, ct);
        if (employeeNumberExists)
            return EmployeeErrors.DuplicateEmployeeNumber(request.EmployeeNumber);

        var employeeResult = Employee.Create(
            request.EmployeeNumber,
            request.FirstName,
            request.LastName,
            emailResult.Value!,
            request.Department,
            request.JobTitle,
            request.HireDate
        );
        if (!employeeResult.IsSuccess)
            return employeeResult.Error!;
        var employee = employeeResult.Value!;

        await employeeRepository.AddAsync(employee, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return EmployeeDto.FromEntity(employee);
    }
}
using AssetFlow.Application.Common.DTOs;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Entities;
using AssetFlow.Domain.Errors;
using MediatR;

namespace AssetFlow.Application.Auth.Commands.ApproveUser;

public class ApproveUserCommandHandler(
    IUserRepository userRepository,
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<ApproveUserCommand, Result<EmployeeDto>>
{
    public async Task<Result<EmployeeDto>> Handle(
        ApproveUserCommand request, CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, ct);
        if (user is null)
            return UserErrors.NotFound(request.UserId);

        // Fail fast before the duplicate-number DB hit if this was already handled.
        if (user.Status != Domain.Enums.UserStatus.Pending)
            return UserErrors.NotPending;

        var numberExists =
            await employeeRepository.ExistsByEmployeeNumberAsync(request.EmployeeNumber, ct);
        if (numberExists)
            return EmployeeErrors.DuplicateEmployeeNumber(request.EmployeeNumber);

        // Reuse the email the user registered with — already a validated value object.
        var employeeResult = Employee.Create(
            request.EmployeeNumber,
            request.FirstName,
            request.LastName,
            user.Email,
            request.Department,
            request.JobTitle,
            request.HireDate);
        if (!employeeResult.IsSuccess)
            return employeeResult.Error!;

        var employee = employeeResult.Value!;

        var approveResult = user.Approve(employee.Id);
        if (!approveResult.IsSuccess)
            return approveResult.Error!;

        await employeeRepository.AddAsync(employee, ct);
        await unitOfWork.SaveChangesAsync(ct);   // employee insert + user status/link, atomic

        return EmployeeDto.FromEntity(employee);
    }
}
using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Auth.Commands.ApproveUser;

public record ApproveUserCommand(
    Guid UserId,
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string Department,
    string? JobTitle,
    DateOnly HireDate
) : IRequest<Result<EmployeeDto>>;
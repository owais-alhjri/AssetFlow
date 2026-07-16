using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Auth.Commands.RegisterUser;

public record RegisterUserCommand(
    string Email,
    string Password,
    Guid? EmployeeId = null
    ): IRequest<Result<AuthResponseDto>>;
using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Auth.Commands.LoginUser;

public record LoginUserCommand(
    string Email,
    string Password
    ): IRequest<Result<AuthResponseDto>>;
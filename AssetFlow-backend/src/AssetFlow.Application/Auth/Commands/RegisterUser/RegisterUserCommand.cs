using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Auth.Commands.RegisterUser;

public record RegisterUserCommand(
    string Email,
    string Password
) : IRequest<Result<RegisterUserResponse>>;

public record RegisterUserResponse(string Email, string Status);
using AssetFlow.Application.Common.DTOs;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;
using MediatR;

namespace AssetFlow.Application.Auth.Commands.LoginUser;

public class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator
    ): IRequestHandler<LoginUserCommand, Result<AuthResponseDto>>
{
    public async Task<Result<AuthResponseDto>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
            return UserErrors.InvalidCredentials;
        if (!user.IsActive)
            return UserErrors.InvalidCredentials;

        var passwordVerify = passwordHasher.Verify(request.Password, user.PasswordHash.Value);
        if (!passwordVerify)
            return UserErrors.InvalidCredentials;

        var roleName = user.Role.Name;
        var token = jwtTokenGenerator.GenerateToken(user, roleName);


        return AuthResponseDto.FromEntity(user, token, roleName);
    }
}
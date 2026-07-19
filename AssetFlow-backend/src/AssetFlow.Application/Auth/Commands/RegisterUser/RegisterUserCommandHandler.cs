using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Entities;
using AssetFlow.Domain.Errors;
using AssetFlow.Domain.ValueObjects;
using MediatR;

namespace AssetFlow.Application.Auth.Commands.RegisterUser;

public class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IUserRoleRepository userRoleRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork
) : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
{
    public async Task<Result<RegisterUserResponse>> Handle(
        RegisterUserCommand request, CancellationToken ct)
    {
        var emailResult = EmailAddress.Create(request.Email);
        if (!emailResult.IsSuccess)
            return emailResult.Error!;

        var email = emailResult.Value!;

        var exists = await userRepository.ExistsByEmailAsync(email.Value, ct);
        if (exists)
            return UserErrors.EmailAlreadyExists;

        var roleId = await userRoleRepository.GetIdByNameAsync("Employee", ct);
        if (roleId is null)
            return UserRoleErrors.DefaultRoleMissing;

        var hashResult = PasswordHash.Create(passwordHasher.Hash(request.Password));
        if (!hashResult.IsSuccess)
            return hashResult.Error!;

        var userResult = User.Register(email, hashResult.Value!, roleId.Value);
        if (!userResult.IsSuccess)
            return userResult.Error!;

        var user = userResult.Value!;

        await userRepository.AddAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return new RegisterUserResponse(user.Email.Value, user.Status.ToString());
    }
}
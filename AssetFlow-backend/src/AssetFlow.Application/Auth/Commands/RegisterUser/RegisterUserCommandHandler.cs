using AssetFlow.Application.Common.DTOs;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Entities;
using AssetFlow.Domain.Errors;
using AssetFlow.Domain.ValueObjects;
using MediatR;

namespace AssetFlow.Application.Auth.Commands.RegisterUser;

public class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IEmployeeRepository employeeRepository,
    IUserRoleRepository userRoleRepository
    ) :IRequestHandler<RegisterUserCommand , Result<AuthResponseDto>>
{
    public async Task<Result<AuthResponseDto>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Rehydrate email.
        var emailResult = EmailAddress.Create(request.Email);
        if (!emailResult.IsSuccess)
            return emailResult.Error!;

        // 2. Reject duplicates before doing any work.
        var emailExists = await userRepository.ExistsByEmailAsync(request.Email, cancellationToken);
        if (emailExists)
            return UserErrors.EmailAlreadyExists;

        // 3. If an employee link was requested, validate it.
        if (request.EmployeeId is { } employeeId)
        {
            var employeeExists = await employeeRepository.ExistsByIdAsync(employeeId, cancellationToken);
            if (!employeeExists)
                return EmployeeErrors.NotFound(employeeId);

            var alreadyLinked = await userRepository.ExistsByEmployeeIdAsync(employeeId, cancellationToken);
            if (alreadyLinked)
                return UserErrors.EmployeeAlreadyLinked;
        }

        // 4. Resolve the default role.
        var roleId = await userRoleRepository.GetIdByNameAsync("Employee", cancellationToken);
        if (roleId is null)                                  
            return UserRoleErrors.DefaultRoleMissing;

        // 5. Hash, then wrap.
        var hash = passwordHasher.Hash(request.Password);
        var passwordHashResult = PasswordHash.Create(hash);
        if (!passwordHashResult.IsSuccess)
            return passwordHashResult.Error!;

        // 6. Build, add, save.
        var userResult = User.Create(
            emailResult.Value!,
            passwordHashResult.Value!,
            roleId.Value,
            employeeId: request.EmployeeId);
        if (!userResult.IsSuccess)
            return userResult.Error!;

        var user = userResult.Value!;
        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 6. Token + response.
        var token = jwtTokenGenerator.GenerateToken(user, "Employee");
        return AuthResponseDto.FromEntity(user, token, "Employee");
    }
}
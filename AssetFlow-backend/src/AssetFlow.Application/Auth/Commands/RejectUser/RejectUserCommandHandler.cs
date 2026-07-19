using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;
using MediatR;

namespace AssetFlow.Application.Auth.Commands.RejectUser;

public class RejectUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<RejectUserCommand, Result>
{
    public async Task<Result> Handle(RejectUserCommand request, CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, ct);
        if (user is null)
            return UserErrors.NotFound(request.UserId);

        var result = user.Reject();
        if (!result.IsSuccess)
            return result.Error!;

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}